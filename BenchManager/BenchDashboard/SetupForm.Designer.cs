namespace Mastersign.Bench.Dashboard
{
    partial class SetupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.splitterBottom = new System.Windows.Forms.Splitter();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnAuto = new System.Windows.Forms.Button();
            this.lblPending = new System.Windows.Forms.Label();
            this.lblPendlingLabel = new System.Windows.Forms.Label();
            this.picState = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressLabel = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblInfoLabel = new System.Windows.Forms.Label();
            this.lblTask = new System.Windows.Forms.Label();
            this.lblTaskLabel = new System.Windows.Forms.Label();
            this.splitterConsole = new System.Windows.Forms.Splitter();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelBusy = new System.Windows.Forms.Panel();
            this.btnCloseBusyPanel = new System.Windows.Forms.Button();
            this.btnOpenLogFile = new System.Windows.Forms.Button();
            this.taskInfoList = new Mastersign.Bench.Dashboard.TaskInfoList();
            this.appList = new Mastersign.Bench.Dashboard.AppList();
            this.downloadList = new Mastersign.Bench.Dashboard.DownloadList();
            this.menuStrip = new Mastersign.Bench.Dashboard.ImmediateMenuStrip();
            this.tsmSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAuto = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpdateEnvironment = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpdateAppLibs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpdateBench = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpgradeBench = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportClone = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiInstallAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReinstallAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpgradeAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUninstallAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCleanUpObsoleteResources = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDownloadAllResources = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDownloadAllAppResources = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteAllResources = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditUserConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditUserApps = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditActivationList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditDeactivationList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowAppIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowCustomAppIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlwaysShowDownloads = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConfigurationInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshView = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picState)).BeginInit();
            this.panelBusy.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitterBottom
            // 
            this.splitterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterBottom.Location = new System.Drawing.Point(0, 358);
            this.splitterBottom.Name = "splitterBottom";
            this.splitterBottom.Size = new System.Drawing.Size(684, 5);
            this.splitterBottom.TabIndex = 0;
            this.splitterBottom.TabStop = false;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
            this.panelTop.Controls.Add(this.btnAuto);
            this.panelTop.Controls.Add(this.lblPending);
            this.panelTop.Controls.Add(this.lblPendlingLabel);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 24);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(684, 38);
            this.panelTop.TabIndex = 7;
            // 
            // btnAuto
            // 
            this.btnAuto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAuto.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.do_32;
            this.btnAuto.Location = new System.Drawing.Point(623, 6);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(49, 32);
            this.btnAuto.TabIndex = 8;
            this.toolTip.SetToolTip(this.btnAuto, "Start or cancel auto setup (Ctrl + F5, ESC)");
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.AutoHandler);
            // 
            // lblPending
            // 
            this.lblPending.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPending.Location = new System.Drawing.Point(69, 16);
            this.lblPending.Name = "lblPending";
            this.lblPending.Size = new System.Drawing.Size(546, 15);
            this.lblPending.TabIndex = 7;
            this.lblPending.Text = "Nothing";
            // 
            // lblPendlingLabel
            // 
            this.lblPendlingLabel.AutoSize = true;
            this.lblPendlingLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPendlingLabel.Location = new System.Drawing.Point(12, 16);
            this.lblPendlingLabel.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.lblPendlingLabel.Name = "lblPendlingLabel";
            this.lblPendlingLabel.Size = new System.Drawing.Size(53, 13);
            this.lblPendlingLabel.TabIndex = 6;
            this.lblPendlingLabel.Text = "Pending:";
            // 
            // picState
            // 
            this.picState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picState.Location = new System.Drawing.Point(623, 9);
            this.picState.Name = "picState";
            this.picState.Size = new System.Drawing.Size(48, 48);
            this.picState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picState.TabIndex = 9;
            this.picState.TabStop = false;
            this.picState.Click += new System.EventHandler(this.ShowLastLogHandler);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(72, 47);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(541, 13);
            this.progressBar.TabIndex = 5;
            // 
            // lblProgressLabel
            // 
            this.lblProgressLabel.AutoSize = true;
            this.lblProgressLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblProgressLabel.Location = new System.Drawing.Point(12, 47);
            this.lblProgressLabel.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.lblProgressLabel.Name = "lblProgressLabel";
            this.lblProgressLabel.Size = new System.Drawing.Size(54, 13);
            this.lblProgressLabel.TabIndex = 4;
            this.lblProgressLabel.Text = "Progress:";
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(69, 26);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(546, 13);
            this.lblInfo.TabIndex = 3;
            // 
            // lblInfoLabel
            // 
            this.lblInfoLabel.AutoSize = true;
            this.lblInfoLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblInfoLabel.Location = new System.Drawing.Point(12, 26);
            this.lblInfoLabel.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.lblInfoLabel.Name = "lblInfoLabel";
            this.lblInfoLabel.Size = new System.Drawing.Size(31, 13);
            this.lblInfoLabel.TabIndex = 2;
            this.lblInfoLabel.Text = "Info:";
            // 
            // lblTask
            // 
            this.lblTask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTask.Location = new System.Drawing.Point(69, 5);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(546, 13);
            this.lblTask.TabIndex = 1;
            this.lblTask.Text = "none";
            // 
            // lblTaskLabel
            // 
            this.lblTaskLabel.AutoSize = true;
            this.lblTaskLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTaskLabel.Location = new System.Drawing.Point(12, 5);
            this.lblTaskLabel.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.lblTaskLabel.Name = "lblTaskLabel";
            this.lblTaskLabel.Size = new System.Drawing.Size(31, 13);
            this.lblTaskLabel.TabIndex = 0;
            this.lblTaskLabel.Text = "Task:";
            // 
            // splitterConsole
            // 
            this.splitterConsole.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterConsole.Location = new System.Drawing.Point(0, 516);
            this.splitterConsole.Name = "splitterConsole";
            this.splitterConsole.Size = new System.Drawing.Size(684, 5);
            this.splitterConsole.TabIndex = 9;
            this.splitterConsole.TabStop = false;
            // 
            // panelBusy
            // 
            this.panelBusy.Controls.Add(this.btnOpenLogFile);
            this.panelBusy.Controls.Add(this.taskInfoList);
            this.panelBusy.Controls.Add(this.btnCloseBusyPanel);
            this.panelBusy.Controls.Add(this.picState);
            this.panelBusy.Controls.Add(this.lblTask);
            this.panelBusy.Controls.Add(this.lblTaskLabel);
            this.panelBusy.Controls.Add(this.lblInfoLabel);
            this.panelBusy.Controls.Add(this.lblInfo);
            this.panelBusy.Controls.Add(this.progressBar);
            this.panelBusy.Controls.Add(this.lblProgressLabel);
            this.panelBusy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBusy.Location = new System.Drawing.Point(0, 62);
            this.panelBusy.Name = "panelBusy";
            this.panelBusy.Size = new System.Drawing.Size(684, 296);
            this.panelBusy.TabIndex = 11;
            this.panelBusy.Visible = false;
            // 
            // btnCloseBusyPanel
            // 
            this.btnCloseBusyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseBusyPanel.Location = new System.Drawing.Point(604, 267);
            this.btnCloseBusyPanel.Name = "btnCloseBusyPanel";
            this.btnCloseBusyPanel.Size = new System.Drawing.Size(68, 23);
            this.btnCloseBusyPanel.TabIndex = 11;
            this.btnCloseBusyPanel.Text = "Cl&ose";
            this.toolTip.SetToolTip(this.btnCloseBusyPanel, "Close the event list. (F4)");
            this.btnCloseBusyPanel.UseVisualStyleBackColor = true;
            this.btnCloseBusyPanel.Click += new System.EventHandler(this.btnCloseBusyPanel_Click);
            // 
            // btnOpenLogFile
            // 
            this.btnOpenLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenLogFile.Location = new System.Drawing.Point(530, 267);
            this.btnOpenLogFile.Name = "btnOpenLogFile";
            this.btnOpenLogFile.Size = new System.Drawing.Size(68, 23);
            this.btnOpenLogFile.TabIndex = 13;
            this.btnOpenLogFile.Text = "&Log File";
            this.toolTip.SetToolTip(this.btnOpenLogFile, "Open the last log file. (F12)");
            this.btnOpenLogFile.UseVisualStyleBackColor = true;
            this.btnOpenLogFile.Click += new System.EventHandler(this.ShowLastLogHandler);
            // 
            // taskInfoList
            // 
            this.taskInfoList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskInfoList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.taskInfoList.Location = new System.Drawing.Point(0, 76);
            this.taskInfoList.Margin = new System.Windows.Forms.Padding(0);
            this.taskInfoList.Name = "taskInfoList";
            this.taskInfoList.Size = new System.Drawing.Size(684, 186);
            this.taskInfoList.TabIndex = 12;
            // 
            // appList
            // 
            this.appList.Core = null;
            this.appList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appList.Location = new System.Drawing.Point(0, 62);
            this.appList.Name = "appList";
            this.appList.Size = new System.Drawing.Size(684, 296);
            this.appList.TabIndex = 10;
            // 
            // downloadList
            // 
            this.downloadList.AutoScroll = true;
            this.downloadList.BackColor = System.Drawing.SystemColors.Window;
            this.downloadList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.downloadList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.downloadList.Downloader = null;
            this.downloadList.Location = new System.Drawing.Point(0, 363);
            this.downloadList.Name = "downloadList";
            this.downloadList.Size = new System.Drawing.Size(684, 153);
            this.downloadList.TabIndex = 6;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmSetup,
            this.tsmEdit,
            this.tsmiColumns,
            this.tsmView});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(684, 24);
            this.menuStrip.TabIndex = 5;
            this.menuStrip.Text = "Menu";
            // 
            // tsmSetup
            // 
            this.tsmSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAuto,
            this.tsmiUpdateEnvironment,
            toolStripSeparator4,
            this.tsmiUpdateAppLibs,
            this.tsmiUpdateBench,
            this.tsmiUpgradeBench,
            this.tsmiExportClone,
            toolStripSeparator2,
            this.tsmiInstallAll,
            this.tsmiReinstallAll,
            this.tsmiUpgradeAll,
            this.tsmiUninstallAll,
            toolStripSeparator3,
            this.tsmiCleanUpObsoleteResources,
            this.tsmiDownloadAllResources,
            this.tsmiDownloadAllAppResources,
            this.tsmiDeleteAllResources,
            this.toolStripSeparator5,
            this.tsmiClose});
            this.tsmSetup.Name = "tsmSetup";
            this.tsmSetup.Size = new System.Drawing.Size(49, 20);
            this.tsmSetup.Text = "&Setup";
            // 
            // tsmiAuto
            // 
            this.tsmiAuto.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.do_16;
            this.tsmiAuto.Name = "tsmiAuto";
            this.tsmiAuto.Size = new System.Drawing.Size(237, 22);
            this.tsmiAuto.Text = "&Automatic Setup";
            this.tsmiAuto.ToolTipText = "Uninstalls incactive apps, downloades missing resources, installs active but not " +
    "installed apps.";
            this.tsmiAuto.Click += new System.EventHandler(this.AutoHandler);
            // 
            // tsmiUpdateEnvironment
            // 
            this.tsmiUpdateEnvironment.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.updateenv_16;
            this.tsmiUpdateEnvironment.Name = "tsmiUpdateEnvironment";
            this.tsmiUpdateEnvironment.Size = new System.Drawing.Size(237, 22);
            this.tsmiUpdateEnvironment.Text = "Update &Environment";
            this.tsmiUpdateEnvironment.ToolTipText = "Updates the Bench environment file(s) and launchers.";
            this.tsmiUpdateEnvironment.Click += new System.EventHandler(this.UpdateEnvironmentHandler);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(234, 6);
            // 
            // tsmiUpdateAppLibs
            // 
            this.tsmiUpdateAppLibs.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.update_apps_16;
            this.tsmiUpdateAppLibs.Name = "tsmiUpdateAppLibs";
            this.tsmiUpdateAppLibs.Size = new System.Drawing.Size(237, 22);
            this.tsmiUpdateAppLibs.Text = "Update App &Libraries";
            this.tsmiUpdateAppLibs.ToolTipText = "Clears the app library cache and re-loads all app libraries.";
            this.tsmiUpdateAppLibs.Click += new System.EventHandler(this.UpdateAppLibsHandler);
            // 
            // tsmiUpdateBench
            // 
            this.tsmiUpdateBench.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.update_apps_16;
            this.tsmiUpdateBench.Name = "tsmiUpdateBench";
            this.tsmiUpdateBench.Size = new System.Drawing.Size(237, 22);
            this.tsmiUpdateBench.Text = "&Update App Libraries and Apps";
            this.tsmiUpdateBench.ToolTipText = "Updates the libraries and upgrades all upgradable apps.";
            this.tsmiUpdateBench.Click += new System.EventHandler(this.UpdateBenchHandler);
            // 
            // tsmiUpgradeBench
            // 
            this.tsmiUpgradeBench.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.update_bench_16;
            this.tsmiUpgradeBench.Name = "tsmiUpgradeBench";
            this.tsmiUpgradeBench.Size = new System.Drawing.Size(237, 22);
            this.tsmiUpgradeBench.Text = "Upgrade &Bench";
            this.tsmiUpgradeBench.ToolTipText = "Upgrades the Bench system.";
            this.tsmiUpgradeBench.Click += new System.EventHandler(this.UpgradeBenchSystemHandler);
            // 
            // tsmiExportClone
            // 
            this.tsmiExportClone.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.export_clone_16;
            this.tsmiExportClone.Name = "tsmiExportClone";
            this.tsmiExportClone.Size = new System.Drawing.Size(237, 22);
            this.tsmiExportClone.Text = "E&xport or Clone Bench...";
            this.tsmiExportClone.ToolTipText = "Creates a compressed transfer package or clone this Bench environment to another " +
    "location.";
            this.tsmiExportClone.Click += new System.EventHandler(this.ExportCloneHandler);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(234, 6);
            // 
            // tsmiInstallAll
            // 
            this.tsmiInstallAll.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.install_16;
            this.tsmiInstallAll.Name = "tsmiInstallAll";
            this.tsmiInstallAll.Size = new System.Drawing.Size(237, 22);
            this.tsmiInstallAll.Text = "&Install Apps";
            this.tsmiInstallAll.ToolTipText = "Installes all active but not installed apps.";
            this.tsmiInstallAll.Click += new System.EventHandler(this.InstallAllHandler);
            // 
            // tsmiReinstallAll
            // 
            this.tsmiReinstallAll.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.reinstall_16;
            this.tsmiReinstallAll.Name = "tsmiReinstallAll";
            this.tsmiReinstallAll.Size = new System.Drawing.Size(237, 22);
            this.tsmiReinstallAll.Text = "&Reinstall Apps";
            this.tsmiReinstallAll.ToolTipText = "Uninstalles all installed apps and reinstalls all active apps.";
            this.tsmiReinstallAll.Click += new System.EventHandler(this.ReinstallAllHandler);
            // 
            // tsmiUpgradeAll
            // 
            this.tsmiUpgradeAll.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.upgrade_16;
            this.tsmiUpgradeAll.Name = "tsmiUpgradeAll";
            this.tsmiUpgradeAll.Size = new System.Drawing.Size(237, 22);
            this.tsmiUpgradeAll.Text = "Up&grade Apps";
            this.tsmiUpgradeAll.ToolTipText = "Upgrades all upgradable apps.";
            this.tsmiUpgradeAll.Click += new System.EventHandler(this.UpgradeAllHandler);
            // 
            // tsmiUninstallAll
            // 
            this.tsmiUninstallAll.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.uninstall_16;
            this.tsmiUninstallAll.Name = "tsmiUninstallAll";
            this.tsmiUninstallAll.Size = new System.Drawing.Size(237, 22);
            this.tsmiUninstallAll.Text = "U&ninstall Apps";
            this.tsmiUninstallAll.ToolTipText = "Uninstalls all apps.";
            this.tsmiUninstallAll.Click += new System.EventHandler(this.UninstallAllHandler);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(234, 6);
            // 
            // tsmiCleanUpObsoleteResources
            // 
            this.tsmiCleanUpObsoleteResources.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.cleanup_16;
            this.tsmiCleanUpObsoleteResources.Name = "tsmiCleanUpObsoleteResources";
            this.tsmiCleanUpObsoleteResources.Size = new System.Drawing.Size(237, 22);
            this.tsmiCleanUpObsoleteResources.Text = "&Clean-Up Obsolete Resources";
            this.tsmiCleanUpObsoleteResources.ToolTipText = "Deletes app resources, which are no longer referenced by any app.";
            this.tsmiCleanUpObsoleteResources.Click += new System.EventHandler(this.CleanUpResourcesHandler);
            // 
            // tsmiDownloadAllResources
            // 
            this.tsmiDownloadAllResources.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.download_16;
            this.tsmiDownloadAllResources.Name = "tsmiDownloadAllResources";
            this.tsmiDownloadAllResources.Size = new System.Drawing.Size(237, 22);
            this.tsmiDownloadAllResources.Text = "Do&wnload Active Resources";
            this.tsmiDownloadAllResources.ToolTipText = "Download missing app resources of all active apps.";
            this.tsmiDownloadAllResources.Click += new System.EventHandler(this.DownloadActiveHandler);
            // 
            // tsmiDownloadAllAppResources
            // 
            this.tsmiDownloadAllAppResources.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.downloadall_16;
            this.tsmiDownloadAllAppResources.Name = "tsmiDownloadAllAppResources";
            this.tsmiDownloadAllAppResources.Size = new System.Drawing.Size(237, 22);
            this.tsmiDownloadAllAppResources.Text = "D&ownload All Resources";
            this.tsmiDownloadAllAppResources.ToolTipText = "Downloads missing app resources of all apps.";
            this.tsmiDownloadAllAppResources.Click += new System.EventHandler(this.DownloadAllHandler);
            // 
            // tsmiDeleteAllResources
            // 
            this.tsmiDeleteAllResources.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.deletedownload_16;
            this.tsmiDeleteAllResources.Name = "tsmiDeleteAllResources";
            this.tsmiDeleteAllResources.Size = new System.Drawing.Size(237, 22);
            this.tsmiDeleteAllResources.Text = "&Delete All Resources";
            this.tsmiDeleteAllResources.ToolTipText = "Clears the app resource cache.";
            this.tsmiDeleteAllResources.Click += new System.EventHandler(this.DeleteAllResourcesHandler);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(234, 6);
            // 
            // tsmiClose
            // 
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.ShortcutKeyDisplayString = "Esc";
            this.tsmiClose.Size = new System.Drawing.Size(237, 22);
            this.tsmiClose.Text = "Clo&se";
            this.tsmiClose.Click += new System.EventHandler(this.CloseHandler);
            // 
            // tsmEdit
            // 
            this.tsmEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEditUserConfig,
            this.tsmiEditUserApps,
            this.tsmiEditActivationList,
            this.tsmiEditDeactivationList});
            this.tsmEdit.Name = "tsmEdit";
            this.tsmEdit.Size = new System.Drawing.Size(116, 20);
            this.tsmEdit.Text = "&Edit Configuration";
            // 
            // tsmiEditUserConfig
            // 
            this.tsmiEditUserConfig.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.userconfig_16;
            this.tsmiEditUserConfig.Name = "tsmiEditUserConfig";
            this.tsmiEditUserConfig.Size = new System.Drawing.Size(174, 22);
            this.tsmiEditUserConfig.Text = "User &Configuration";
            this.tsmiEditUserConfig.Click += new System.EventHandler(this.EditUserConfigHandler);
            // 
            // tsmiEditUserApps
            // 
            this.tsmiEditUserApps.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.apps;
            this.tsmiEditUserApps.Name = "tsmiEditUserApps";
            this.tsmiEditUserApps.Size = new System.Drawing.Size(174, 22);
            this.tsmiEditUserApps.Text = "&User Apps";
            this.tsmiEditUserApps.Click += new System.EventHandler(this.EditUserAppsHandler);
            // 
            // tsmiEditActivationList
            // 
            this.tsmiEditActivationList.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.include;
            this.tsmiEditActivationList.Name = "tsmiEditActivationList";
            this.tsmiEditActivationList.Size = new System.Drawing.Size(174, 22);
            this.tsmiEditActivationList.Text = "&Activated Apps";
            this.tsmiEditActivationList.Click += new System.EventHandler(this.ActivationListHandler);
            // 
            // tsmiEditDeactivationList
            // 
            this.tsmiEditDeactivationList.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.exclude;
            this.tsmiEditDeactivationList.Name = "tsmiEditDeactivationList";
            this.tsmiEditDeactivationList.Size = new System.Drawing.Size(174, 22);
            this.tsmiEditDeactivationList.Text = "&Deactivated Apps";
            this.tsmiEditDeactivationList.Click += new System.EventHandler(this.DeactivationListHandler);
            // 
            // tsmiColumns
            // 
            this.tsmiColumns.Name = "tsmiColumns";
            this.tsmiColumns.Size = new System.Drawing.Size(67, 20);
            this.tsmiColumns.Text = "&Columns";
            // 
            // tsmView
            // 
            this.tsmView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShowAppIndex,
            this.tsmiShowCustomAppIndex,
            toolStripSeparator1,
            this.tsmiAlwaysShowDownloads,
            this.tsmiConfigurationInfo,
            this.tsmiRefreshView});
            this.tsmView.Name = "tsmView";
            this.tsmView.Size = new System.Drawing.Size(44, 20);
            this.tsmView.Text = "&View";
            // 
            // tsmiShowAppIndex
            // 
            this.tsmiShowAppIndex.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.library_16;
            this.tsmiShowAppIndex.Name = "tsmiShowAppIndex";
            this.tsmiShowAppIndex.Size = new System.Drawing.Size(205, 22);
            this.tsmiShowAppIndex.Text = "Bench App &Libraries";
            // 
            // tsmiShowCustomAppIndex
            // 
            this.tsmiShowCustomAppIndex.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.userlibrary_16;
            this.tsmiShowCustomAppIndex.Name = "tsmiShowCustomAppIndex";
            this.tsmiShowCustomAppIndex.Size = new System.Drawing.Size(205, 22);
            this.tsmiShowCustomAppIndex.Text = "&User App Library";
            this.tsmiShowCustomAppIndex.Click += new System.EventHandler(this.ShowCustomAppIndexHandler);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(202, 6);
            // 
            // tsmiAlwaysShowDownloads
            // 
            this.tsmiAlwaysShowDownloads.CheckOnClick = true;
            this.tsmiAlwaysShowDownloads.Name = "tsmiAlwaysShowDownloads";
            this.tsmiAlwaysShowDownloads.Size = new System.Drawing.Size(205, 22);
            this.tsmiAlwaysShowDownloads.Text = "Always Show &Downloads";
            this.tsmiAlwaysShowDownloads.CheckedChanged += new System.EventHandler(this.AlwaysShowDownloadsCheckedChanged);
            // 
            // tsmiConfigurationInfo
            // 
            this.tsmiConfigurationInfo.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.info_16;
            this.tsmiConfigurationInfo.Name = "tsmiConfigurationInfo";
            this.tsmiConfigurationInfo.Size = new System.Drawing.Size(205, 22);
            this.tsmiConfigurationInfo.Text = "&Configuration Info";
            this.tsmiConfigurationInfo.Click += new System.EventHandler(this.ConfigurationInfoHandler);
            // 
            // tsmiRefreshView
            // 
            this.tsmiRefreshView.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.refresh_16;
            this.tsmiRefreshView.Name = "tsmiRefreshView";
            this.tsmiRefreshView.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.tsmiRefreshView.Size = new System.Drawing.Size(205, 22);
            this.tsmiRefreshView.Text = "&Refresh";
            this.tsmiRefreshView.Click += new System.EventHandler(this.RefreshViewHandler);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 521);
            this.Controls.Add(this.panelBusy);
            this.Controls.Add(this.appList);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.splitterBottom);
            this.Controls.Add(this.downloadList);
            this.Controls.Add(this.splitterConsole);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(640, 560);
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Bench - Setup";
            this.Activated += new System.EventHandler(this.SetupForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupForm_FormClosing);
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.Shown += new System.EventHandler(this.SetupForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownDownHandler);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picState)).EndInit();
            this.panelBusy.ResumeLayout(false);
            this.panelBusy.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Splitter splitterBottom;
        private ImmediateMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmSetup;
        private System.Windows.Forms.ToolStripMenuItem tsmiDownloadAllResources;
        private System.Windows.Forms.ToolStripMenuItem tsmiInstallAll;
        private System.Windows.Forms.ToolStripMenuItem tsmEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditUserConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditUserApps;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditActivationList;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditDeactivationList;
        private DownloadList downloadList;
        private System.Windows.Forms.ToolStripMenuItem tsmView;
        private System.Windows.Forms.ToolStripMenuItem tsmiAlwaysShowDownloads;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.Label lblTaskLabel;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblInfoLabel;
        private System.Windows.Forms.Label lblProgressLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteAllResources;
        private System.Windows.Forms.ToolStripMenuItem tsmiReinstallAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpgradeAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiUninstallAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshView;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateEnvironment;
        private System.Windows.Forms.Label lblPending;
        private System.Windows.Forms.Label lblPendlingLabel;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.ToolStripMenuItem tsmiAuto;
        private System.Windows.Forms.Splitter splitterConsole;
        private System.Windows.Forms.PictureBox picState;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowAppIndex;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowCustomAppIndex;
        private System.Windows.Forms.ToolStripMenuItem tsmiDownloadAllAppResources;
        private System.Windows.Forms.ToolStripMenuItem tsmiCleanUpObsoleteResources;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpgradeBench;
        private System.Windows.Forms.ToolStripMenuItem tsmiColumns;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateBench;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateAppLibs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportClone;
        private System.Windows.Forms.ToolStripMenuItem tsmiConfigurationInfo;
        private AppList appList;
        private System.Windows.Forms.Panel panelBusy;
        private System.Windows.Forms.Button btnCloseBusyPanel;
        private TaskInfoList taskInfoList;
        private System.Windows.Forms.Button btnOpenLogFile;
    }
}