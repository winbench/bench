using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Mastersign.Bench.Dashboard.Properties;
using TsudaKageyu;
using System.Threading.Tasks;

namespace Mastersign.Bench.Dashboard
{
    public partial class MainForm : Form
    {
        private readonly Core core;

        private SetupForm setupForm;
        private ContextMenuStrip docsMenu;

        public MainForm(Core core)
        {
            this.core = core;
            core.ConfigReloaded += ConfigReloadedHandler;
            core.AllAppStateChanged += AppStateChangedHandler;
            core.AppActivationChanged += AppStateChangedHandler;
            core.AppStateChanged += AppStateChangedHandler;
            core.BusyChanged += CoreBusyChangedHandler;
            InitializeComponent();
            InitializePositionManagement();
            InitializeAppLauncherList();
            InitializeDocsMenu();
            InitializeTopPanel();
            InitializeStatusStrip();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateVersion();
            if (core.SetupOnStartup)
            {
                SetupHandler(this, EventArgs.Empty);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (core.Busy)
            {
                core.Cancelation.Cancel();
                e.Cancel = true;
                MessageBox.Show(this,
                    "You can not close this window until the current running setup action has ended. The action has been requested to cancel.",
                    "Closing Setup Window",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (setupForm != null) setupForm.Close();
        }

        private void CoreBusyChangedHandler(object sender, EventArgs e)
        {
            btnAutoSetup.Enabled = !core.Busy;
        }

        private void AppStateChangedHandler(object sender, EventArgs e)
        {
            if (core.Busy) return;
            InitializeAppLauncherList();
            InitializeStatusStrip();
        }

        private void ConfigReloadedHandler(object sender, EventArgs e)
        {
            UpdateShellButtons();
            InitializeAppLauncherList();
            InitializeDocsMenu();
            InitializeStatusStrip();
        }

        private void InitializePositionManagement()
        {
            core.WindowPositionManager.RegisterForm(
                this, ConfigPropertyKeys.DashboardMainPosition);
        }

        private void InitializeAppLauncherList()
        {
            appLauncherList.Core = core;
            appLauncherList.AppIndex = core.Config.Apps;
        }

        private async void InitializeDocsMenu()
        {
            var ctxm = new ContextMenuStrip();

            var benchItem = new ToolStripMenuItem("Bench Website");
            benchItem.Image = new Icon(Icon, new Size(16, 16)).ToBitmap();
            benchItem.Tag = core.Config.GetStringValue(ConfigPropertyKeys.Website);
            benchItem.Click += LinkHandler;
            ctxm.Items.Add(benchItem);

            var appLibsItem = new ToolStripMenuItem("App Libraries");
            appLibsItem.Image = Resources.library_16;
            ctxm.Items.Add(appLibsItem);
            foreach (var lib in core.Config.AppLibraries)
            {
                var appLibItem = new ToolStripMenuItem("App Library '" + lib.ID + "'");
                appLibItem.Image = Resources.books_16;
                appLibItem.Tag = lib;
                appLibItem.Click += AppIndexHandler;
                appLibsItem.DropDownItems.Add(appLibItem);
            }

            var userAppLibItem = new ToolStripMenuItem("User App Library");
            userAppLibItem.Image = Resources.userlibrary_16;
            userAppLibItem.Click += CustomAppIndexHandler;
            ctxm.Items.Add(userAppLibItem);

            ctxm.Items.Add(new ToolStripSeparator());

            var apps = core.Config.Apps.ActiveApps.OrderBy(app => app.Label);
            foreach (var app in apps)
            {
                var label = app.Label;
                var websiteUrl = app.Website;
                var docs = app.Docs;
                if (string.IsNullOrEmpty(websiteUrl) && (docs == null || docs.Count == 0)) continue;
                var item = new ToolStripMenuItem(label);
                item.Image = await ExtractLauncherIcon(app);
                if (!string.IsNullOrEmpty(websiteUrl))
                {
                    if (item.Image == null)
                    {
                        item.Image = Resources.website_16;
                    }
                    item.Tag = websiteUrl;
                    item.Click += LinkHandler;
                }
                if (docs != null)
                {
                    foreach (var n in docs.Keys.OrderBy(k => k))
                    {
                        var docItem = item.DropDownItems.Add(n, Resources.doc_16);
                        docItem.Tag = docs[n];
                        docItem.Click += LinkHandler;
                    }
                }
                ctxm.Items.Add(item);
            }

            docsMenu = ctxm;
        }

        private void AppIndexHandler(object sender, EventArgs e)
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

        private void CustomAppIndexHandler(object sender, EventArgs e)
        {
            var viewer = new MarkdownViewer(core);
            viewer.LoadMarkdown(
                Path.Combine(
                    core.Config.GetStringValue(ConfigPropertyKeys.UserConfigDir),
                    core.Config.GetStringValue(ConfigPropertyKeys.AppLibIndexFileName)),
                "User App Library");
            viewer.Show();
        }

        private Task<Image> ExtractLauncherIcon(AppFacade app)
        {
            return Task.Run<Image>(() =>
            {
                var path = app.LauncherIcon;
                if (string.IsNullOrEmpty(app.Launcher) || string.IsNullOrEmpty(path)) return null;
                Icon icon;
                try
                {
                    icon = Icon.ExtractAssociatedIcon(path);
                }
                catch (Exception)
                {
                    return null;
                }
                return new Icon(icon, new Size(16, 16)).ToBitmap();
            });
        }

        private void InitializeStatusStrip()
        {
            tsslRootPath.Text = core.Config.BenchRootDir;
            tsslAppCount.Text = core.Config.Apps.ActiveApps.Length.ToString();
        }

        private async void InitializeTopPanel()
        {
            UpdateShellButtons();
            var cmdImg = await ExtractIcon(core.CmdPath, "CMD");
            var psImg = await ExtractIcon(core.PowerShellPath, "PowerShell");
            btnShellCmd.Image = cmdImg ?? Resources.missing_app_16;
            btnShellPowerShell.Image = psImg ?? Resources.missing_app_16;
            btnShellBash.Image = Resources.bash_16;
        }

        private static Task<Bitmap> ExtractIcon(string path, string name, int index = 0)
        {
            return Task.Run(() =>
            {
                if (!File.Exists(path)) return null;
                try
                {
                    var extractor = new IconExtractor(path);
                    var icon = extractor.GetIcon(index);
                    return new Icon(icon, new Size(16, 16)).ToBitmap();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Failed to load icon for " + name + ": " + e);
                    return null;
                }
            });
        }

        private void UpdateShellButtons()
        {
            var buttons = new[]
            {
                btnShellCmd,
                btnShellPowerShell,
                btnShellBash,
            };
            var buttonEnabled = new[]
            {
                core.Config.GetBooleanValue(ConfigPropertyKeys.QuickAccessCmd, true),
                core.Config.GetBooleanValue(ConfigPropertyKeys.QuickAccessPowerShell, false),
                core.Config.GetBooleanValue(ConfigPropertyKeys.QuickAccessBash, false),
            };
            var x = buttons[0].Left;
            var y = buttons[0].Top;
            var d = 2;
            for (int i = 0; i < buttons.Length; i++)
            {
                var b = buttons[i];
                b.Visible = buttonEnabled[i];
                b.Location = new Point(x, y);
                Debug.WriteLine("Enabled: " + i + " -> " + buttonEnabled[i]);
                if (buttonEnabled[i])
                {
                    x = x + b.Width + d;
                    Debug.WriteLine("Next Stop: " + x);
                }
            }
        }

        private async void UpdateVersion()
        {
            var config = core.Config;
            var currentVersion = config.GetStringValue(ConfigPropertyKeys.Version);
            tsslVersion.Text = currentVersion;
            tsslVersion.ToolTipText = null;
            if (config.GetBooleanValue(ConfigPropertyKeys.AutoUpdateCheck))
            {
                tsslVersionStatus.Image = Resources.progress_16_animation;
                tsslVersionStatus.ToolTipText = "Determining the latest Bench version...";
                tsslVersion.ToolTipText = tsslVersionStatus.ToolTipText;
                var version = await core.GetLatestVersionNumber();
                if (IsDisposed) return;
                if (version != null)
                {
                    if (string.Equals(currentVersion, version))
                    {
                        tsslVersionStatus.Image = Resources.ok_16;
                        tsslVersionStatus.ToolTipText = "The Bench program is up to date.";
                    }
                    else
                    {
                        tsslVersionStatus.Image = Resources.warning_16;
                        tsslVersionStatus.ToolTipText = "There is a newer version of Bench available: " + version;
                    }
                }
                else
                {
                    tsslVersionStatus.Image = Resources.error_grey_16;
                    tsslVersionStatus.ToolTipText =
                        "Determining the latest Bench version failed due to network issues.";
                }
                tsslVersion.ToolTipText = tsslVersionStatus.ToolTipText;
            }
            else
            {
                tsslVersionStatus.Visible = false;
            }
        }

        private void RootPathClickHandler(object sender, EventArgs e)
        {
            core.ShowPathInExplorer(core.Config.BenchRootDir);
        }

        private void ShellCmdHandler(object sender, EventArgs e)
        {
            new SimpleExecutionHost().StartProcess(core.Env,
                core.Config.GetStringValue(ConfigPropertyKeys.ProjectRootDir),
                core.CmdPath, "", result => { }, ProcessMonitoring.ExitCode);
        }

        private void ShellPowerShellHandler(object sender, EventArgs e)
        {
            new SimpleExecutionHost().StartProcess(core.Env,
                core.Config.GetStringValue(ConfigPropertyKeys.ProjectRootDir),
                core.PowerShellPath, "", result => { }, ProcessMonitoring.ExitCode);
        }

        private void ShellBashHandler(object sender, EventArgs e)
        {
            var bashPath = core.BashPath;
            if (File.Exists(bashPath))
            {
                new SimpleExecutionHost().StartProcess(core.Env,
                    core.Config.GetStringValue(ConfigPropertyKeys.ProjectRootDir),
                    bashPath, "", result => { }, ProcessMonitoring.ExitCode);
            }
            else
            {
                MessageBox.Show(
                    "The executable of Bash was not found in the Git distribution."
                    + Environment.NewLine + Environment.NewLine
                    + bashPath,
                    "Running Bash...",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupHandler(object sender, EventArgs e)
        {
            if (setupForm == null || setupForm.IsDisposed)
            {
                setupForm = new SetupForm(core);
            }
            if (!setupForm.Visible)
            {
                setupForm.Show(this);
                // workaround for the focus jumping back to the main form
                Application.DoEvents();
                setupForm.Activate();
            }
            else
            {
                setupForm.Activate();
            }
        }

        private void AutoSetupHandler(object sender, EventArgs e)
        {
            core.SetupOnStartup = true;
            if (setupForm == null || setupForm.IsDisposed)
            {
                setupForm = new SetupForm(core);
                Application.DoEvents();
            }
            if (!setupForm.Visible)
            {
                setupForm.Show();
            }
            else
            {
                setupForm.StartAutoSetup();
            }
        }

        private void AboutHandler(object sender, EventArgs e)
        {
            new AboutDialog(core).ShowDialog(this);
        }

        private void DocsHandler(object sender, EventArgs e)
        {
            if (docsMenu == null) return;
            docsMenu.Show(btnDocs, new Point(btnDocs.Width, btnDocs.Height),
                ToolStripDropDownDirection.BelowLeft);
        }

        private void LinkHandler(object sender, EventArgs e)
        {
            var url = (sender as ToolStripItem)?.Tag as string;
            Process.Start(url);
        }

        private void KeyDownDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && e.Modifiers == Keys.None)
                core.Reload(configChanged: true);
            else if (e.KeyCode == Keys.F1 && e.Modifiers == Keys.None)
                DocsHandler(this, EventArgs.Empty);
            else if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Alt)
                AboutHandler(this, EventArgs.Empty);
            else if (e.KeyCode == Keys.F6 && e.Modifiers == Keys.None)
                SetupHandler(this, EventArgs.Empty);
            else if (e.KeyCode == Keys.F5 && e.Modifiers == Keys.Control)
                AutoSetupHandler(this, EventArgs.Empty);
            else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Alt)
                ShellCmdHandler(this, EventArgs.Empty);
            else if (e.KeyCode == Keys.P && e.Modifiers == Keys.Alt)
                ShellPowerShellHandler(this, EventArgs.Empty);
            else if (e.KeyCode == Keys.B && e.Modifiers == Keys.Alt)
                ShellBashHandler(this, EventArgs.Empty);
        }
    }
}
