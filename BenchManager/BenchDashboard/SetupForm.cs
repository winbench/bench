using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ConEmu.WinForms;
using Mastersign.Bench.Dashboard.Properties;
using Mastersign.Bench.Markdown;
using System.Threading.Tasks;

namespace Mastersign.Bench.Dashboard
{
    public partial class SetupForm : Form
    {
        private readonly Core core;

        private ConEmuExecutionHost conHost;
        private ConEmuControl conControl;
        
        public SetupForm(Core core)
        {
            this.core = core;
            core.ConfigReloaded += CoreConfigReloadedHandler;
            core.AllAppStateChanged += CoreAllAppStateChangedHandler;
            core.AppActivationChanged += CoreAppActivationChangedHandler;
            core.AppStateChanged += CoreAppStateChangedHandler;
            core.BusyChanged += CoreBusyChangedHandler;
            core.ActionStateChanged += CoreActionStateChangedHandler;
            InitializeComponent();
            InitializeConsole();
            core.WindowPositionManager.RegisterForm(
                this, ConfigPropertyKeys.DashboardSetupPosition,
                DefaultBounds(), FormWindowState.Normal);
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            InitializeDownloadList();
            InitializeAppIndexMenu();
            InitializeAppListColumnsMenu();
            InitializeAppList();
            UpdatePendingCounts();

            if (core.SetupOnStartup)
            {
                StartAutoSetup();
            }
        }

        public void StartAutoSetup()
        {
            if (core.Busy) return;
            core.SetupOnStartup = false;
            Visible = true;
            Application.DoEvents();
            AutoHandler(this, EventArgs.Empty);
        }

        private Rectangle DefaultBounds()
        {
            var region = Screen.PrimaryScreen.WorkingArea;
            var w = Math.Max(MinimumSize.Width, region.Width / 2);
            var h = Math.Max(MinimumSize.Height, region.Height);
            var x = region.Right - w;
            var y = region.Top;
            return new Rectangle(x, y, w, h);
        }

        private void InitializeAppIndexMenu()
        {
            tsmiShowAppIndex.DropDownItems.Clear();
            foreach (var lib in core.Config.AppLibraries)
            {
                var appLibItem = new ToolStripMenuItem("App Library '" + lib.ID + "'");
                appLibItem.Image = Resources.books_16;
                appLibItem.Tag = lib;
                appLibItem.Click += ShowAppIndexHandler;
                tsmiShowAppIndex.DropDownItems.Add(appLibItem);
            }
        }

        private void InitializeAppList()
        {
            appList.Core = core;
            appList.TaskStarted += AppListTaskStartedHandler;
            appList.TaskInfo += AppListTaskInfoHandler;
        }

        private void InitializeAppListColumnsMenu()
        {
            var colLabels = core.Config.GetStringListValue(
                ConfigPropertyKeys.DashboardSetupAppListColumns,
                appList.DefaultColumnLabels);
            foreach (var colLabel in appList.ColumnLabels)
            {
                ToolStripMenuItem item = null;
                if (tsmiColumns.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem i in tsmiColumns.DropDownItems)
                        if (i.Text == colLabel)
                            item = i;
                }
                if (item == null)
                {
                    item = new ToolStripMenuItem(colLabel);
                    item.Click += AppListColumnToggleHandler;
                    tsmiColumns.DropDownItems.Add(item);
                }
                item.Checked = colLabels.Contains(colLabel);
            }
        }

        private void AppListColumnToggleHandler(object sender, EventArgs e)
        {
            var newColLabels = new List<string>();
            foreach (ToolStripMenuItem item in tsmiColumns.DropDownItems)
            {
                if (item == sender) item.Checked = !item.Checked;
                if (item.Checked)
                {
                    newColLabels.Add(string.Format("`{0}`", item.Text));
                }
            }
            var configFile = core.Config.GetStringValue(ConfigPropertyKeys.UserConfigFile);
            MarkdownPropertyEditor.UpdateFile(configFile, new Dictionary<string, string>
                { { ConfigPropertyKeys.DashboardSetupAppListColumns, string.Join(", ", newColLabels) } });
        }

        private void CoreConfigReloadedHandler(object sender, EventArgs e)
        {
            InitializeDownloadList();
            InitializeAppIndexMenu();
            InitializeAppListColumnsMenu();
            UpdatePendingCounts();
        }

        private void CoreAllAppStateChangedHandler(object sender, EventArgs e)
        {
            UpdatePendingCounts();
        }

        private void CoreAppActivationChangedHandler(object sender, EventArgs e)
        {
            UpdatePendingCounts();
        }

        private void CoreAppStateChangedHandler(object sender, AppEventArgs e)
        {
            UpdatePendingCounts();
        }

        private void UpdatePendingCounts()
        {
            var downloadIDs = new List<string>();
            var uninstallIDs = new List<string>();
            var installIDs = new List<string>();
            foreach (var app in core.Config.Apps)
            {
                if (app.IsActive && app.CanDownloadResource) downloadIDs.Add(app.ID);
                if (!app.IsActive && app.CanUninstall) uninstallIDs.Add(app.ID);
                if (app.IsActive && app.CanInstall) installIDs.Add(app.ID);
            }
            toolTip.SetToolTip(lblPending,
                (downloadIDs.Count > 0 ? "Downloads: " + string.Join(", ", downloadIDs) + Environment.NewLine : "") +
                (uninstallIDs.Count > 0 ? "Uninstalls: " + string.Join(", ", uninstallIDs) + Environment.NewLine : "") +
                (installIDs.Count > 0 ? "Installs: " + string.Join(", ", installIDs) : ""));
            var list = new List<string>();
            if (downloadIDs.Count > 0)
            {
                list.Add(string.Format("{0} {1}",
                    downloadIDs.Count,
                    downloadIDs.Count == 1 ? "Download" : "Downloads"));
            }
            if (uninstallIDs.Count > 0)
            {
                list.Add(string.Format("{0} {1}",
                    uninstallIDs.Count,
                    uninstallIDs.Count == 1 ? "Uninstall" : "Uninstalls"));
            }
            if (installIDs.Count > 0)
            {
                list.Add(string.Format("{0} {1}",
                    installIDs.Count,
                    installIDs.Count == 1 ? "Install" : "Installs"));
            }
            lblPending.Text = list.Count > 0 ? string.Join(", ", list) : "Nothing";
        }

        private void CoreBusyChangedHandler(object sender, EventArgs e)
        {
            var notBusy = !core.Busy;
            foreach (ToolStripItem tsmi in tsmSetup.DropDownItems)
            {
                tsmi.Enabled = notBusy;
            }
            foreach (ToolStripItem tsmi in tsmEdit.DropDownItems)
            {
                tsmi.Enabled = notBusy;
            }
            btnAuto.Image = notBusy
                            ? Resources.do_32
                            : Resources.stop_32;
        }

        private void CoreActionStateChangedHandler(object sender, EventArgs e)
        {
            switch (core.ActionState)
            {
                case ActionState.BusyWithoutErrors:
                    picState.Image = Resources.progress_36_animation;
                    NotifyBusyStateChanged(true);
                    break;
                case ActionState.BusyCanceled:
                    picState.Image = Resources.stop_36_animation;
                    break;
                case ActionState.BusyWithErrors:
                    picState.Image = Resources.warning_36_animation;
                    break;
                case ActionState.FinishedWithoutErrors:
                    picState.Image = Resources.ok_48;
                    NotifyBusyStateChanged(false);
                    break;
                case ActionState.FinishedWithErrors:
                    picState.Image = Resources.warning_48;
                    NotifyBusyStateChanged(false);
                    break;
                case ActionState.Canceled:
                    picState.Image = Resources.cancelled_48;
                    NotifyBusyStateChanged(false);
                    break;
                default:
                    picState.Image = null;
                    break;
            }
        }

        private void NotifyBusyStateChanged(bool busy)
        {
            if (busy)
            {
                toolTip.SetToolTip(picState, null);
                MinimizeBox = false;
            }
            else
            {
                MinimizeBox = true;
            }
        }

        private void InitializeConsole()
        {
            var c = new ConEmuControl();
            c.Dock = DockStyle.Bottom;
            c.Height = 200;
            c.AutoStartInfo = null;
            c.Visible = true;
            Controls.Add(c);
            conControl = c;
            conHost = new ConEmuExecutionHost(core, conControl, core.Config.Apps[AppKeys.ConEmu]?.Exe);
            conHost.StartHost();
            core.ProcessExecutionHost = conHost;
        }

        private void DisposeConsole()
        {
            var oldHost = core.ProcessExecutionHost;
            core.ProcessExecutionHost = new SimpleExecutionHost();
            oldHost.Dispose();
        }

        private void InitializeDownloadList()
        {
            if (downloadList.Downloader != null)
            {
                downloadList.Downloader.IsWorkingChanged -= DownloaderIsWorkingChangedHandler;
            }
            downloadList.Downloader = core.Downloader;
            downloadList.Downloader.IsWorkingChanged += DownloaderIsWorkingChangedHandler;
            UpdateDownloadListVisibility();
        }

        private void UpdateProgressBar(float progress)
        {
            if (float.IsNaN(progress) || float.IsInfinity(progress)) progress = 0f;
            progress = Math.Min(1f, Math.Max(0f, progress));
            progressBar.Value = progressBar.Minimum + (int)((progressBar.Maximum - progressBar.Minimum) * progress);
        }

        private void DownloaderIsWorkingChangedHandler(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((EventHandler)DownloaderIsWorkingChangedHandler);
                return;
            }
            UpdateDownloadListVisibility();
        }

        private void UpdateDownloadListVisibility()
        {
            var newVisibility = core.Downloader.IsWorking || tsmiAlwaysShowDownloads.Checked;
            if (IsDownloadListVisible != newVisibility)
            {
                if (newVisibility) UpdateDownloadListHeight();
                IsDownloadListVisible = newVisibility;
            }
        }

        private void UpdateDownloadListHeight()
        {
            var heightDiff = downloadList.Height - downloadList.ClientSize.Height;
            const int itemHeight = 40;
            var maxItems = Math.Min(10, Math.Max(1, core.Config.GetInt32Value(ConfigPropertyKeys.ParallelDownloads)));
            downloadList.Height = itemHeight * maxItems + heightDiff;
        }

        protected bool IsDownloadListVisible
        {
            get { return downloadList.Visible; }
            set
            {
                SuspendLayout();
                splitterBottom.Visible = value;
                downloadList.Visible = value;
                ResumeLayout();
            }
        }

        private void AlwaysShowDownloadsCheckedChanged(object sender, EventArgs e)
        {
            UpdateDownloadListVisibility();
        }

        private static string SystemEditorPath
            => Path.Combine(
                Environment.GetEnvironmentVariable("SystemRoot"),
                "notepad.exe");

        private enum TextFileLanguage { Markdown, ActivationList, Log }

        private void OpenTextFile(string name, string path, TextFileLanguage syntax, bool readOnly)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show(
                    "File of " + name + " not found."
                    + Environment.NewLine + Environment.NewLine
                    + path,
                    "Open " + name + " ...",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            var editorAppId = core.Config.GetStringValue(ConfigPropertyKeys.TextEditorApp);
            var editorApp = core.Config.Apps[editorAppId];
            if (editorApp.IsInstalled)
            {
                if (editorAppId == AppKeys.NotepadPlusPlus)
                {
                    var args = new List<string> { "-multiInst", "-nosession", "-notabbar" };
                    // Notepad++ does currently not recognize user languages with the -l switch
                    //switch (syntax)
                    //{
                    //    case TextFileLanguage.Markdown:
                    //        args.Add("-lBenchConfig");
                    //        break;
                    //    case TextFileLanguage.ActivationList:
                    //        args.Add("-lBenchActivationList");
                    //        break;
                    //    case TextFileLanguage.Log:
                    //        args.Add("-lBenchLog");
                    //        break;
                    //}
                    if (readOnly) args.Add("-ro");
                    args.Add(path);
                    core.LaunchApp(editorAppId, args.ToArray());
                }
                else
                {
                    core.LaunchApp(editorAppId, path);
                }
            }
            else
            {
                System.Diagnostics.Process.Start(SystemEditorPath, path);
            }
        }

        private void EditUserConfigHandler(object sender, EventArgs e)
        {
            OpenTextFile("User Configuration",
                core.Config.GetStringValue(ConfigPropertyKeys.UserConfigFile),
                TextFileLanguage.Markdown,
                readOnly: false);
        }

        private void EditUserAppsHandler(object sender, EventArgs e)
        {
            OpenTextFile("User App Library",
                Path.Combine(
                    core.Config.GetStringValue(ConfigPropertyKeys.UserConfigDir),
                    core.Config.GetStringValue(ConfigPropertyKeys.AppLibIndexFileName)),
                TextFileLanguage.Markdown,
                readOnly: false);
        }

        private void ActivationListHandler(object sender, EventArgs e)
        {
            OpenTextFile("App Activation",
                core.Config.GetStringValue(ConfigPropertyKeys.AppActivationFile),
                TextFileLanguage.ActivationList,
                readOnly: false);
        }

        private void DeactivationListHandler(object sender, EventArgs e)
        {
            OpenTextFile("App Deactivation",
                core.Config.GetStringValue(ConfigPropertyKeys.AppDeactivationFile),
                TextFileLanguage.ActivationList,
                readOnly: false);
        }

        private void AppListTaskInfoHandler(object sender, TaskInfoEventArgs ea)
            => ProcessTaskInfo(ea.TaskInfo);

        private void TaskInfoHandler(TaskInfo info)
        {
            if (InvokeRequired)
                BeginInvoke((Action<TaskInfo>)ProcessTaskInfo, info);
            else
                ProcessTaskInfo(info);
        }

        private void ProcessTaskInfo(TaskInfo info)
        {
            lblInfo.Text = info.Message;
            if (info is TaskProgress progressInfo) UpdateProgressBar(progressInfo.Progress);
            if (info is TaskError taskError) toolTip.SetToolTip(picState, taskError.Message);
        }

        private void AppListTaskStartedHandler(object sender, TaskStartedEventArgs ea)
            => AnnounceTask(ea.Message);

        private void AnnounceTask(string label)
            => lblTask.Text = label;

        private async void AutoHandler(object sender, EventArgs e)
        {
            if (!core.Busy)
            {
                AnnounceTask("Automatic Setup");
                await core.AutoSetupAsync(TaskInfoHandler);
            }
            else
            {
                var cancelation = core.Cancelation;
                if (cancelation != null) cancelation.Cancel();
            }
        }

        private async void DownloadActiveHandler(object sender, EventArgs e)
        {
            AnnounceTask("Download App Resources");
            await core.DownloadAppResourcesAsync(TaskInfoHandler);
        }

        private async void DownloadAllHandler(object sender, EventArgs e)
        {
            AnnounceTask("Download All App Resources");
            await core.DownloadAllAppResourcesAsync(TaskInfoHandler);
        }

        private async void DeleteAllResourcesHandler(object sender, EventArgs e)
        {
            AnnounceTask("Delete App Resources");
            await core.DeleteAppResourcesAsync(TaskInfoHandler);
        }

        private async void CleanUpResourcesHandler(object sender, EventArgs e)
        {
            AnnounceTask("Cleaning Up App Resources");
            await core.CleanUpResourcesAsync(TaskInfoHandler);
        }

        private async void InstallAllHandler(object sender, EventArgs e)
        {
            AnnounceTask("Install Apps");
            await core.InstallAppsAsync(TaskInfoHandler);
        }

        private async void UninstallAllHandler(object sender, EventArgs e)
        {
            AnnounceTask("Uninstall Apps");
            await core.UninstallAppsAsync(TaskInfoHandler);
        }

        private async void ReinstallAllHandler(object sender, EventArgs e)
        {
            AnnounceTask("Reinstall Apps");
            await core.ReinstallAppsAsync(TaskInfoHandler);
        }

        private async void UpgradeAllHandler(object sender, EventArgs e)
        {
            AnnounceTask("Upgrade Apps");
            await core.UpgradeAppsAsync(TaskInfoHandler);
        }

        private async void UpdateEnvironmentHandler(object sender, EventArgs e)
        {
            AnnounceTask("Update Environment");
            await core.UpdateEnvironmentAsync(TaskInfoHandler);
        }

        private async void UpdateAppLibsHandler(object sender, EventArgs e)
        {
            AnnounceTask("Update App Libraries");
            await core.UpdateAppLibrariesAsync(TaskInfoHandler);
        }

        private async void UpdateBenchHandler(object sender, EventArgs e)
        {
            AnnounceTask("Update App Libraries and Apps");
            await core.UpdateAppsAsync(TaskInfoHandler);
        }

        private async void UpgradeBenchSystemHandler(object sender, EventArgs e)
        {
            AnnounceTask("Updating Bench System");

            var version = core.Config.GetStringValue(ConfigPropertyKeys.Version);
            var latestVersion = await core.GetLatestVersionNumber();
            if (latestVersion == null)
            {
                MessageBox.Show(this,
                    "Retrieving the version number of the latest release failed."
                    + Environment.NewLine + Environment.NewLine
                    + "The update process was cancelled.",
                    "Updating Bench System",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.Equals(version, latestVersion))
            {
                var res = MessageBox.Show(this,
                    "There is no update available."
                    + " You are already using the latest release of Bench (v" + version + ")." + Environment.NewLine
                    + Environment.NewLine
                    + "Do you want to reinstall the Bench system with a download of the latest release anyway?",
                    "Updating Bench System",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;
            }
            else
            {
                var res = MessageBox.Show(this,
                    "Are you sure you want to update the Bench system?" + Environment.NewLine
                    + Environment.NewLine
                    + "Current version: " + version + Environment.NewLine
                    + "Update version: " + latestVersion,
                    "Updating Bench System",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res != DialogResult.OK) return;
            }
            var result = await core.DownloadBenchUpdateAsync(TaskInfoHandler);
            if (result.Success)
            {
                BenchTasks.InitiateInstallationBootstrap(core.Config);
                Program.Core.Shutdown();
            }
        }

        private async void ExportCloneHandler(object sender, EventArgs e)
        {
            var dlg = new ExportForm(core);
            var result = dlg.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var export = dlg.ExportMode;
            AnnounceTask(export
                ? "Exporting the Bench environment"
                : "Cloning the Bench environment");
            var targetPath = dlg.TargetPath;
            var selection = dlg.ContentSelection;
            var success = export
                ? await core.ExportBenchEnvironmentAsync(TaskInfoHandler, targetPath, selection)
                : await core.CloneBenchEnvironmentAsync(TaskInfoHandler, targetPath, selection);
        }

        private void RefreshViewHandler(object sender, EventArgs e)
        {
            core.Reload();
        }

        private void ShowAppIndexHandler(object sender, EventArgs e)
        {
            var lib = (sender as ToolStripItem)?.Tag as AppLibrary;
            if (lib != null)
            {
                var viewer = new MarkdownViewer(core);
                viewer.LoadMarkdown(Path.Combine(lib.BaseDir,
                    core.Config.GetStringValue(ConfigPropertyKeys.AppLibIndexFileName)),
                    "App Library '" + lib.ID + "'");
                viewer.Show();
            }
        }

        private void ShowCustomAppIndexHandler(object sender, EventArgs e)
        {
            var viewer = new MarkdownViewer(core);
            viewer.LoadMarkdown(
                Path.Combine(
                    core.Config.GetStringValue(ConfigPropertyKeys.UserConfigDir),
                    core.Config.GetStringValue(ConfigPropertyKeys.AppLibIndexFileName)),
                "User App Library");
            viewer.Show();
        }

        private void ConfigurationInfoHandler(object sender, EventArgs e)
        {
            new ConfigInfoDialog(core.Config).ShowDialog(this);
        }

        private void KeyDownDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && e.Modifiers == Keys.None)
            {
                Close();
            }
            if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
            {
                appList.FocusSearchBox();
            }
        }

        private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (core.Busy)
            {
                e.Cancel = true;
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    core.Cancelation.Cancel();
                    MessageBox.Show(this,
                        "You can not close this window until the current running setup action has ended. The action has been requested to cancel.",
                        "Closing Setup Window",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            DisposeConsole();
            core.ConfigReloaded -= CoreConfigReloadedHandler;
            core.AllAppStateChanged -= CoreAllAppStateChangedHandler;
            core.AppStateChanged -= CoreAppStateChangedHandler;
            core.BusyChanged -= CoreBusyChangedHandler;
            core.ActionStateChanged -= CoreActionStateChangedHandler;
        }

        private void CloseHandler(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowLastLogHandler(object sender, EventArgs e)
        {
            var logFile = core?.LastActionResult?.LogFile;
            if (string.IsNullOrWhiteSpace(logFile)) return;
            if (File.Exists(logFile))
            {
                OpenTextFile("Last Log File", logFile, TextFileLanguage.Log, readOnly: true);
            }
            else
            {
                MessageBox.Show(this,
                    "Could not find log file:" + Environment.NewLine + Environment.NewLine + logFile,
                    "Open Last Log File",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupForm_Shown(object sender, EventArgs e)
        {
            appList.Focus();
            appList.FocusSearchBox();
        }

        private void SetupForm_Activated(object sender, EventArgs e)
        {
            appList.Focus();
            appList.FocusSearchBox();
        }
    }
}
