using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    public class Core : IDisposable, IBenchManager
    {
        public IUserInterface UI { get; private set; }

        public IProcessExecutionHost ProcessExecutionHost { get; set; }

        public BenchConfiguration Config { get; private set; }

        public BenchEnvironment Env { get; private set; }

        public Downloader Downloader { get; private set; }

        public Control GuiContext { get; set; }

        public bool SetupOnStartup { get; set; }

        private bool busy;

        private ActionState actionState;

        private Cancelation cancelation;

        public event EventHandler ConfigReloaded;

        public event EventHandler AllAppStateChanged;

        public event EventHandler<AppEventArgs> AppStateChanged;

        public event EventHandler BusyChanged;

        public event EventHandler ActionStateChanged;

        private Action<TaskInfo> CatchTaskInfos(Action<TaskInfo> notify)
        {
            return info =>
            {
                if (info.AppId != null)
                {
                    OnAppStateChanged(info.AppId);
                }
                if (info is TaskError) ActionState = ActionState.BusyWithErrors;
                if (notify != null) notify(info);
            };
        }

        public Core(string benchRoot)
        {
            Debug.WriteLine("Initializing UI Core for Bench...");
            UI = new WinFormsUserInterface();
            Config = new BenchConfiguration(benchRoot, true, true, true);
            Env = new BenchEnvironment(Config);
            Downloader = BenchTasks.InitializeDownloader(Config);
            ProcessExecutionHost = new DefaultExecutionHost();
        }

        public void SyncWithGui(ThreadStart task)
        {
            if (GuiContext != null && GuiContext.InvokeRequired)
            {
                GuiContext.Invoke(task);
            }
            else
            {
                task();
            }
        }

        public bool Busy
        {
            get { return busy; }
            set
            {
                if (value == busy) return;
                busy = value;
                OnBusyChanged();
            }
        }

        private void OnBusyChanged()
        {
            SyncWithGui(() =>
            {
                var handler = BusyChanged;
                if (handler != null) handler(this, EventArgs.Empty);
            });
        }

        public ActionState ActionState
        {
            get { return actionState; }
            set
            {
                if (value == actionState) return;
                actionState = value;
                OnActionStateChanged();
            }
        }

        private void OnActionStateChanged()
        {
            SyncWithGui(() =>
            {
                var handler = ActionStateChanged;
                if (handler != null) handler(this, EventArgs.Empty);
            });
        }

        public Cancelation Cancelation { get { return cancelation; } }

        private void OnConfigReloaded()
        {
            SyncWithGui(() =>
            {
                var handler = ConfigReloaded;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            });
        }

        private void OnAllAppStateChanged()
        {
            SyncWithGui(() =>
            {
                var handler = AllAppStateChanged;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            });
        }

        private void OnAppStateChanged(string appId)
        {
            SyncWithGui(() =>
            {
                var handler = AppStateChanged;
                if (handler != null)
                {
                    handler(this, new AppEventArgs(appId));
                }
            });
        }

        public void Reload(bool configChanged = false)
        {
            Config = Config.Reload();
            Env = new BenchEnvironment(Config);
            if (configChanged)
            {
                Downloader.Dispose();
                Downloader = BenchTasks.InitializeDownloader(Config);
            }
            OnConfigReloaded();
        }

        public void SetAppActivated(string appId, bool value)
        {
            var activationFile = new ActivationFile(Config.GetStringValue(PropertyKeys.AppActivationFile));
            if (value)
            {
                activationFile.SignIn(appId);
            }
            else
            {
                activationFile.SignOut(appId);
            }
            Reload();
        }

        public void SetAppDeactivated(string appId, bool value)
        {
            var deactivationFile = new ActivationFile(Config.GetStringValue(PropertyKeys.AppDeactivationFile));
            if (value)
            {
                deactivationFile.SignIn(appId);
            }
            else
            {
                deactivationFile.SignOut(appId);
            }
            Reload();
        }

        public Task<ActionResult> RunTaskAsync(BenchTaskForAll action,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return Task.Run(() =>
            {
                return action(this, CatchTaskInfos(notify), cancelation);
            });
        }

        public Task<ActionResult> RunTaskAsync(BenchTaskForOne action, string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return Task.Run(() =>
            {
                return action(this, appId, CatchTaskInfos(notify), cancelation);
            });
        }

        private void BeginAction()
        {
            if (Busy) throw new InvalidOperationException("The core is already busy.");
            cancelation = new Cancelation();
            cancelation.Canceled += CancelationCanceledHandler;
            Busy = true;
            ActionState = ActionState.BusyWithoutErrors;
        }

        private void CancelationCanceledHandler(object sender, EventArgs e)
        {
            ActionState = ActionState.BusyCanceled;
        }

        private void EndAction(bool success)
        {
            Busy = false;
            cancelation.Canceled -= CancelationCanceledHandler;
            ActionState = cancelation.IsCanceled
                ? ActionState.Canceled
                : success
                    ? ActionState.FinishedWithoutErrors
                    : ActionState.FinishedWithErrors;
            cancelation = null;
        }

        public async Task<ActionResult> AutoSetupAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoAutoSetup, notify, cancelation).ConfigureAwait(false);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Installing Apps", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Installing Apps", "Finished.");
            }
            else
            {
                UI.ShowWarning("Installing Apps",
                    BuildCombinedErrorMessage(
                        "Installing the following apps failed:",
                        "Installing the apps failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> DownloadAppResourcesAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoDownloadAppResources, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Downloading App Resources", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Downloading App Resources", "Finished.");
            }
            else
            {
                UI.ShowWarning("Downloading App Resources",
                    BuildCombinedErrorMessage(
                        "Downloading resources for the following apps failed:",
                        "Downloading the app resources failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> DownloadAppResourcesAsync(string appId, Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoDownloadAppResources, appId, notify, cancelation);
            EndAction(result.Success);
            if (!result.Success)
            {
                UI.ShowWarning("Downloading App Resource",
                    BuildCombinedErrorMessage(
                        "Downloading app resource failed:",
                        "Downloading the resource for app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAppStateChanged(appId);
            return result;
        }

        public async Task<ActionResult> DownloadAllAppResourcesAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoDownloadAllAppResources, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Downloading All App Resources", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Downloading All App Resources", "Finished.");
            }
            else
            {
                UI.ShowWarning("Downloading All App Resources",
                    BuildCombinedErrorMessage(
                        "Downloading resources for the following apps failed:",
                        "Downloading the app resources failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> DeleteAppResourcesAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoDeleteAppResources, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Deleting App Resources", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Deleting App Resources", "Finished.");
            }
            else
            {
                UI.ShowWarning("Deleting App Resources",
                    BuildCombinedErrorMessage(
                        "Deleting resources for the following apps failed:",
                        "Deleting the app resources failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> DeleteAppResourcesAsync(Action<TaskInfo> notify, string appId)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoDeleteAppResources, appId, notify, cancelation);
            EndAction(result.Success);
            if (!result.Success)
            {
                UI.ShowWarning("Deleting App Resource",
                    BuildCombinedErrorMessage(
                        "Deleting app resource failed:",
                        "Deleting the resource of app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAppStateChanged(appId);
            return result;
        }

        public async Task<ActionResult> CleanUpResourcesAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoCleanUpAppResources, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Clening Up App Resources", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Cleaning Up App Resources", "Finished.");
            }
            else
            {
                UI.ShowWarning("Cleaning Up App Resources",
                    BuildCombinedErrorMessage(
                        "Cleaning up resources failed:",
                        "Cleaning up the app resources failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> InstallAppsAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoInstallApps, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Installing Apps", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Installing Apps", "Finished.");
            }
            else
            {
                UI.ShowWarning("Installing Apps",
                    BuildCombinedErrorMessage(
                        "Installing the following apps failed:",
                        "Installing the apps failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> InstallAppsAsync(Action<TaskInfo> notify, string appId)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoInstallApps, appId, notify, cancelation);
            EndAction(result.Success);
            if (!result.Success)
            {
                UI.ShowWarning("Installing App",
                    BuildCombinedErrorMessage(
                        "Installing the app failed:",
                        "Installing the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAppStateChanged(appId);
            return result;
        }

        public async Task<ActionResult> UninstallAppsAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUninstallApps, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Uninstalling Apps", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Uninstalling Apps", "Finished.");
            }
            else
            {
                UI.ShowWarning("Uninstalling Apps",
                    BuildCombinedErrorMessage(
                        "Uninstalling the following apps failed:",
                        "Uninstalling the apps failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> UninstallAppsAsync(Action<TaskInfo> notify, string appId)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUninstallApps, appId, notify, cancelation);
            EndAction(result.Success);
            if (!result.Success)
            {
                UI.ShowWarning("Uninstalling App",
                    BuildCombinedErrorMessage(
                        "Uninstalling the app failed:",
                        "Uninstalling the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAppStateChanged(appId);
            return result;
        }

        public async Task<ActionResult> ReinstallAppsAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoReinstallApps, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Reinstalling Apps", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Reinstalling Apps", "Finished.");
            }
            else
            {
                UI.ShowWarning("Reinstalling Apps",
                    BuildCombinedErrorMessage(
                        "Reinstalling the following apps failed:",
                        "Reinstalling the apps failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> ReinstallAppsAsync(Action<TaskInfo> notify, string appId)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoReinstallApps, appId, notify, cancelation);
            EndAction(result.Success);
            if (!result.Success)
            {
                UI.ShowWarning("Reinstall App",
                    BuildCombinedErrorMessage(
                        "Reinstalling the app failed:",
                        "Reinstalling the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAppStateChanged(appId);
            return result;
        }

        public async Task<ActionResult> UpgradeAppsAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUpgradeApps, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Upgrading Apps", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Upgrading Apps", "Finished.");
            }
            else
            {
                UI.ShowWarning("Upgrading Apps",
                    BuildCombinedErrorMessage(
                        "Upgrading the following apps failed:",
                        "Upgrading the apps failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> UpgradeAppsAsync(Action<TaskInfo> notify, string appId)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUpgradeApps, appId, notify, cancelation);
            EndAction(result.Success);
            if (!result.Success)
            {
                UI.ShowWarning("Upgrade App",
                    BuildCombinedErrorMessage(
                        "Upgrading the app failed:",
                        "Upgrading the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAppStateChanged(appId);
            return result;
        }

        public async Task<ActionResult> UpdateEnvironmentAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUpdateEnvironment, notify, cancelation);
            EndAction(result.Success);
            if (result.Canceled)
            {
                UI.ShowWarning("Updating Environment", "Canceled.");
            }
            else if (result.Success)
            {
                UI.ShowInfo("Updating Environment", "Finished.");
            }
            else
            {
                UI.ShowWarning("Updating Environment",
                    BuildCombinedErrorMessage(
                        "Updating the bench environment for the following apps failed:",
                        "Updating the bench environment failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        private static string BuildCombinedErrorMessage(string infoWithErrors, string infoWithoutErrors,
            IEnumerable<TaskInfo> errors, int maxLines)
        {
            var sb = new StringBuilder();
            var cnt = 0;
            if (errors != null)
            {
                foreach (var err in errors)
                {
                    cnt++;
                    if (cnt >= maxLines)
                    {
                        sb.AppendLine("...");
                        break;
                    }
                    sb.AppendLine(err.Message);
                }
            }
            return cnt > 0
                ? infoWithErrors + Environment.NewLine + Environment.NewLine + sb.ToString()
                : infoWithoutErrors;
        }

        public Process LaunchApp(string id, params string[] args)
        {
            try
            {
                return BenchTasks.LaunchApp(Config, Env, id, args);
            }
            catch (FileNotFoundException e)
            {
                UI.ShowWarning("Launching App",
                    "The executable of the app could not be found."
                     + Environment.NewLine + Environment.NewLine
                     + e.FileName);
                return null;
            }
        }

        public void StartProcess(string exe, params string[] args)
        {
            ProcessExecutionHost.StartProcess(Env, Config.BenchRootDir, exe,
                CommandLine.FormatArgumentList(args),
                result => { },
                ProcessMonitoring.ExitCode);
        }

        public void ShowPathInExplorer(string path)
        {
            Process.Start(path);
        }

        public string CmdPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetEnvironmentVariable("SystemRoot"),
                    @"System32\cmd.exe");
            }
        }

        public string PowerShellPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetEnvironmentVariable("SystemRoot"),
                    @"System32\WindowsPowerShell\v1.0\powershell.exe");
            }
        }

        public string BashPath
        {
            get
            {
                return Path.Combine(
                    Config.GetStringGroupValue(AppKeys.Git, PropertyKeys.AppDir),
                    @"bin\bash.exe");
            }
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

            Downloader.Dispose();
        }

        [Conditional("DEBUG")]
        public void DisplayError(string message, Exception e)
        {
            UI.ShowError("Unexpected Exception",
                message
                + Environment.NewLine + Environment.NewLine
                + e.ToString());
        }
    }
}
