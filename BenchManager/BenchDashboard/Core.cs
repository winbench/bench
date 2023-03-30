using System;
using System.Linq;
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

        public Form GuiContext { get; set; }

        public WindowPositionManager WindowPositionManager { get; private set; }

        public bool SetupOnStartup { get; set; }

        private bool busy;

        private bool configReloadNecessary;

        private bool activationReloadNecessary;

        private readonly object configReloadLockHandle = new object();

        private FileSystemWatcher[] configFileWatchers;

        private FileSystemWatcher[] activationFileWatchers;

        private ActionState actionState;

        public event EventHandler ConfigReloaded;

        public event EventHandler AppActivationChanged;

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
                notify?.Invoke(info);
            };
        }

        public Core(string benchRoot)
        {
            Debug.WriteLine("Initializing UI Core for Bench...");
            UI = new WinFormsUserInterface();
            Config = new BenchConfiguration(benchRoot, true, true, true);
            Env = new BenchEnvironment(Config);
            Downloader = BenchTasks.InitializeDownloader(Config);
            ProcessExecutionHost = new SimpleExecutionHost();
            WindowPositionManager = new WindowPositionManager(this);
            SetupFileWatchers();
        }

        public void Shutdown()
        {
            GuiContext.Close();
        }

        public void SyncWithGui(ThreadStart task)
        {
            if (GuiContext != null && GuiContext.InvokeRequired)
                GuiContext.Invoke(task);
            else
                task();
        }

        public bool Busy
        {
            get { return busy; }
            set
            {
                if (value == busy) return;
                busy = value;
                if (busy)
                {
                    LastActionResult = null;
                }
                OnBusyChanged();
                if (!busy)
                {
                    if (configReloadNecessary)
                        Reload();
                    else if (activationReloadNecessary)
                        ReloadAppActivation();
                }
            }
        }

        private void OnBusyChanged()
            => SyncWithGui(() => BusyChanged?.Invoke(this, EventArgs.Empty));

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

        public ActionResult LastActionResult { get; private set; }

        private void OnActionStateChanged()
            => SyncWithGui(() => ActionStateChanged?.Invoke(this, EventArgs.Empty));

        public Cancelation Cancelation { get; private set; }

        private void SetupFileWatchers()
        {
            DisposeFileWatchers();

            var configFileSet = ConfigurationFileType.UserConfig
                | ConfigurationFileType.SiteConfig
                | ConfigurationFileType.UserAppLib;
            configFileWatchers = Config
                .GetConfigurationFiles(configFileSet, actuallyLoaded: true, mustExist: true)
                .Select(p => CreateFileWatcher(p.Path, ConfigFileChangedHandler))
                .ToArray();

            var activationFileSet = ConfigurationFileType.AppSelection;
            configFileWatchers = Config
                .GetConfigurationFiles(activationFileSet, actuallyLoaded: true, mustExist: true)
                .Select(p => CreateFileWatcher(p.Path, ActivationFileChangedHandler))
                .ToArray();
        }

        private FileSystemWatcher CreateFileWatcher(string path, FileSystemEventHandler handler)
        {
            var watcher = new FileSystemWatcher(Path.GetDirectoryName(path))
            {
                Filter = Path.GetFileName(path),
                IncludeSubdirectories = false,
                EnableRaisingEvents = true,
            };
            watcher.Changed += handler;
            return watcher;
        }

        private void DisposeFileWatchers()
        {
            if (configFileWatchers != null)
            {
                foreach (var w in configFileWatchers)
                {
                    w.Changed -= ConfigFileChangedHandler;
                    w.Dispose();
                }
                configFileWatchers = null;
            }
            if (activationFileWatchers != null)
            {
                foreach (var w in activationFileWatchers)
                {
                    w.Changed -= ActivationFileChangedHandler;
                    w.Dispose();
                }
                activationFileWatchers = null;
            }
        }

        private void ConfigFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (busy)
                configReloadNecessary = true;
            else
                Task.Run(() => Reload(true));
        }

        private void ActivationFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (busy)
                activationReloadNecessary = true;
            else
                Task.Run(() => ReloadAppActivation());
        }

        private void OnConfigReloaded()
            => SyncWithGui(() => ConfigReloaded?.Invoke(this, EventArgs.Empty));

        private void OnAllAppStateChanged()
        {
            foreach (var app in Config.Apps)
            {
                app.DiscardCachedValues();
            }
            SyncWithGui(() => AllAppStateChanged?.Invoke(this, EventArgs.Empty));
        }

        private void OnAppActivationChanged()
            => SyncWithGui(() => AppActivationChanged?.Invoke(this, EventArgs.Empty));

        private void OnAppStateChanged(string appId)
            => SyncWithGui(() => AppStateChanged?.Invoke(this, new AppEventArgs(appId)));

        public void Reload(bool configChanged = false)
        {
            configReloadNecessary = false;
            activationReloadNecessary = false;
            lock (configReloadLockHandle)
            {
                Config = Config.Reload();
                Env = new BenchEnvironment(Config);
                if (configChanged)
                {
                    Downloader.Dispose();
                    Downloader = BenchTasks.InitializeDownloader(Config);
                }
            }
            OnConfigReloaded();
        }

        public void ReloadAppActivation()
        {
            activationReloadNecessary = false;
            lock (configReloadLockHandle)
            {
                Config.AppProperties.ReloadAppActivation();
            }
            OnAppActivationChanged();
        }

        public void SetAppActivated(string appId, bool value)
        {
            var activationFile = new ActivationFile(Config.GetStringValue(ConfigPropertyKeys.AppActivationFile));
            if (value)
                activationFile.SignIn(appId);
            else
                activationFile.SignOut(appId);
        }

        public void SetAppDeactivated(string appId, bool value)
        {
            var deactivationFile = new ActivationFile(Config.GetStringValue(ConfigPropertyKeys.AppDeactivationFile));
            if (value)
                deactivationFile.SignIn(appId);
            else
                deactivationFile.SignOut(appId);
        }

        public Task<ActionResult> RunTaskAsync(BenchTaskForAll action, Action<TaskInfo> notify, Cancelation cancelation)
            => Task.Run(() => action(this, CatchTaskInfos(notify), cancelation));

        public Task<ActionResult> RunTaskAsync(BenchTaskForOne action, string appId, Action<TaskInfo> notify, Cancelation cancelation)
            => Task.Run(() => action(this, appId, CatchTaskInfos(notify), cancelation));

        private void BeginAction()
        {
            if (Busy) throw new InvalidOperationException("The core is already busy.");
            Cancelation = new Cancelation();
            Cancelation.Canceled += CancelationCanceledHandler;
            Busy = true;
            ActionState = ActionState.BusyWithoutErrors;
        }

        private void CancelationCanceledHandler(object sender, EventArgs e)
        {
            ActionState = ActionState.BusyCanceled;
        }

        private void EndAction(ActionResult result)
        {
            LastActionResult = result;
            Busy = false;
            Cancelation.Canceled -= CancelationCanceledHandler;
            ActionState = Cancelation.IsCanceled
                ? ActionState.Canceled
                : result.Success
                    ? ActionState.FinishedWithoutErrors
                    : ActionState.FinishedWithErrors;
            Cancelation = null;
        }

        public async Task<ActionResult> AutoSetupAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoAutoSetup, notify, Cancelation).ConfigureAwait(false);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Installing Apps", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoDownloadAppResources, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Downloading App Resources", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoDownloadAppResources, appId, notify, Cancelation);
            EndAction(result);
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
            var result = await RunTaskAsync(BenchTasks.DoDownloadAllAppResources, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Downloading All App Resources", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoDeleteAppResources, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Deleting App Resources", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoDeleteAppResources, appId, notify, Cancelation);
            EndAction(result);
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
            var result = await RunTaskAsync(BenchTasks.DoCleanUpAppResources, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Clening Up App Resources", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoInstallApps, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Installing Apps", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoInstallApps, appId, notify, Cancelation);
            EndAction(result);
            if (!result.Success)
            {
                UI.ShowWarning("Installing App",
                    BuildCombinedErrorMessage(
                        "Installing the app failed:",
                        "Installing the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> UninstallAppsAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUninstallApps, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Uninstalling Apps", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoUninstallApps, appId, notify, Cancelation);
            EndAction(result);
            if (!result.Success)
            {
                UI.ShowWarning("Uninstalling App",
                    BuildCombinedErrorMessage(
                        "Uninstalling the app failed:",
                        "Uninstalling the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> ReinstallAppsAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoReinstallApps, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Reinstalling Apps", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoReinstallApps, appId, notify, Cancelation);
            EndAction(result);
            if (!result.Success)
            {
                UI.ShowWarning("Reinstall App",
                    BuildCombinedErrorMessage(
                        "Reinstalling the app failed:",
                        "Reinstalling the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> UpgradeAppsAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUpgradeApps, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Upgrading Apps", "Canceled.");
            }
            else if (!result.Success)
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
            var result = await RunTaskAsync(BenchTasks.DoUpgradeApps, appId, notify, Cancelation);
            EndAction(result);
            if (!result.Success)
            {
                UI.ShowWarning("Upgrade App",
                    BuildCombinedErrorMessage(
                        "Upgrading the app failed:",
                        "Upgrading the app " + appId + " failed.",
                        result.Errors, 10));
            }
            OnAllAppStateChanged();
            return result;
        }

        public async Task<ActionResult> UpdateEnvironmentAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoUpdateEnvironment, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Updating Environment", "Canceled.");
            }
            else if (!result.Success)
            {
                UI.ShowWarning("Updating Environment",
                    BuildCombinedErrorMessage(
                        "Updating the bench environment for the following apps failed:",
                        "Updating the bench environment failed.",
                        result.Errors, 10));
            }
            return result;
        }

        public async Task<ActionResult> UpdateAppLibrariesAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            BenchTasks.DeleteAppLibraries(Config);
            var result = await RunTaskAsync(BenchTasks.DoLoadAppLibraries, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Loading App Libraries", "Canceled");
                EndAction(result);
                return result;
            }
            else if (result.Success)
            {
                Reload();
            }
            else
            {
                UI.ShowWarning("Loading App Libraries",
                    "Loading the app libraries failed.");
            }
            return result;
        }

        public async Task<ActionResult> UpdateAppsAsync(Action<TaskInfo> notify)
        {
            var result = await UpdateAppLibrariesAsync(notify);
            if (!result.Success) return result;
            return await UpgradeAppsAsync(notify);
        }

        private class AsyncVersionNumberResult : IAsyncResult
        {
            public object AsyncState => null;

            public bool Success { get; private set; }
            public string VersionNumber { get; private set; }
            private ManualResetEvent handle;

            public AsyncVersionNumberResult()
            {
                handle = new ManualResetEvent(false);
            }

            public WaitHandle AsyncWaitHandle => handle;

            public bool CompletedSynchronously => false;

            public bool IsCompleted => handle == null;

            public void NotifyResult(bool success, string versionNumber)
            {
                Success = success;
                VersionNumber = versionNumber;
                handle.Set();
                handle = null;
            }
        }

        public Task<string> GetLatestVersionNumber()
        {
            var asyncResult = new AsyncVersionNumberResult();
            BenchTasks.GetLatestVersionAsync(Config, asyncResult.NotifyResult);
            return Task<string>.Factory.FromAsync(asyncResult, r =>
            {
                var result = (AsyncVersionNumberResult)r;
                return result.Success ? result.VersionNumber : null;
            });
        }

        public async Task<ActionResult> DownloadBenchUpdateAsync(Action<TaskInfo> notify)
        {
            BeginAction();
            var result = await RunTaskAsync(BenchTasks.DoDownloadBenchUpdate, notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Downloading Bench update", "Canceled");
            }
            else if (!result.Success)
            {
                UI.ShowWarning("Upgrading Bench System",
                    BuildCombinedErrorMessage(
                        "Downloading the latest Bench update failed.",
                        "Downloading the latest Bench update failed.",
                        null, 1));
            }
            return result;
        }

        public async Task<ActionResult> ExportBenchEnvironmentAsync(Action<TaskInfo> notify, string targetFile, TransferPaths contentSelection)
        {
            BeginAction();
            var result = await RunTaskAsync(
                (m, n, c) => BenchTasks.DoExportBenchEnvironment(m, n, c, targetFile, contentSelection),
                notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Exporting Bench Environment", "Canceled");
            }
            else if (!result.Success)
            {
                UI.ShowWarning("Exporting Bench Environment",
                    BuildCombinedErrorMessage(
                        "Exporting the Bench environment failed.",
                        "Exporting the Bench environment failed.",
                        result.Errors, 10));
            }
            else
            {
                UI.ShowInfo("Exporting Bench Environment", "Exporting the Bench environment finished.");
            }
            return result;
        }

        public async Task<ActionResult> CloneBenchEnvironmentAsync(Action<TaskInfo> notify, string targetDirectory, TransferPaths contentSelection)
        {
            BeginAction();
            var result = await RunTaskAsync(
                (m, n, c) => BenchTasks.DoCloneBenchEnvironment(m, n, c, targetDirectory, contentSelection),
                notify, Cancelation);
            EndAction(result);
            if (result.Canceled)
            {
                UI.ShowWarning("Cloning Bench Environment", "Canceled");
            }
            else if (!result.Success)
            {
                UI.ShowWarning("Cloning Bench Environment",
                    BuildCombinedErrorMessage(
                        "Copying the Bench environment failed.",
                        "Copying the Bench environment failed.",
                        result.Errors, 10));
            }
            else
            {
                UI.ShowInfo(
                    "Cloning Bench Environment",
                    "Copying the Bench environment finished. Started initialization of the cloned environment.");
            }
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
            catch (System.ComponentModel.Win32Exception e)
            {
                UI.ShowWarning("Launching App",
                    "Failed to execute the apps main executable."
                    + Environment.NewLine + Environment.NewLine
                    + e.Message);
                return null;
            }
        }

        public void ShowAppInfo(string id)
        {
            new AppInfoDialog(Config, Config.Apps[id]).ShowDialog(GuiContext);
        }

        public void ShowAppWebsite(string id)
        {
            var app = Config.Apps[id];
            try
            {
                var url = new Uri(app.Website, UriKind.Absolute);
                if (url.Scheme != "http" && url.Scheme != "https")
                {
                    throw new ArgumentException("The given URL does not use the HTTP(S) protocol: "
                        + app.Website);
                }
                Process.Start(url.AbsoluteUri);
            }
            catch (Exception exc)
            {
                MessageBox.Show(GuiContext, exc.Message, "Open Website",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        public string PowerShellCorePath
        {
            get
            {
                return Path.Combine(
                    Config.Apps[AppKeys.PowerShellCore].Dir,
                    @"pwsh.exe");
            }
        }

        public string BashPath
        {
            get
            {
                return Path.Combine(
                    Config.Apps[AppKeys.Git].Dir,
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
