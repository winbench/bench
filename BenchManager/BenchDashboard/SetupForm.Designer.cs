﻿namespace Mastersign.Bench.Dashboard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.tsSeparatorDownloads = new System.Windows.Forms.ToolStripSeparator();
            this.splitterBottom = new System.Windows.Forms.Splitter();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.picState = new System.Windows.Forms.PictureBox();
            this.btnAuto = new System.Windows.Forms.Button();
            this.lblPending = new System.Windows.Forms.Label();
            this.lblPendlingLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressLabel = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblInfoLabel = new System.Windows.Forms.Label();
            this.lblTask = new System.Windows.Forms.Label();
            this.lblTaskLabel = new System.Windows.Forms.Label();
            this.gridApps = new System.Windows.Forms.DataGridView();
            this.colIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLibrary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActivated = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colExcluded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLicense = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctxmAppActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAppInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.miWebsite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparatorWebsite = new System.Windows.Forms.ToolStripSeparator();
            this.miInstall = new System.Windows.Forms.ToolStripMenuItem();
            this.miReinstall = new System.Windows.Forms.ToolStripMenuItem();
            this.miPackageUpgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.miUpgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.miUninstall = new System.Windows.Forms.ToolStripMenuItem();
            this.miDownloadResource = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteResource = new System.Windows.Forms.ToolStripMenuItem();
            this.splitterConsole = new System.Windows.Forms.Splitter();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
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
            this.tsmiRefreshView = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridApps)).BeginInit();
            this.ctxmAppActions.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(234, 6);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(234, 6);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(234, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(202, 6);
            // 
            // tsSeparatorDownloads
            // 
            this.tsSeparatorDownloads.Name = "tsSeparatorDownloads";
            this.tsSeparatorDownloads.Size = new System.Drawing.Size(176, 6);
            // 
            // splitterBottom
            // 
            this.splitterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterBottom.Location = new System.Drawing.Point(0, 298);
            this.splitterBottom.Name = "splitterBottom";
            this.splitterBottom.Size = new System.Drawing.Size(684, 5);
            this.splitterBottom.TabIndex = 0;
            this.splitterBottom.TabStop = false;
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.SystemColors.Control;
            this.panelStatus.Controls.Add(this.picState);
            this.panelStatus.Controls.Add(this.btnAuto);
            this.panelStatus.Controls.Add(this.lblPending);
            this.panelStatus.Controls.Add(this.lblPendlingLabel);
            this.panelStatus.Controls.Add(this.progressBar);
            this.panelStatus.Controls.Add(this.lblProgressLabel);
            this.panelStatus.Controls.Add(this.lblInfo);
            this.panelStatus.Controls.Add(this.lblInfoLabel);
            this.panelStatus.Controls.Add(this.lblTask);
            this.panelStatus.Controls.Add(this.lblTaskLabel);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatus.Location = new System.Drawing.Point(0, 24);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(684, 109);
            this.panelStatus.TabIndex = 7;
            // 
            // picState
            // 
            this.picState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picState.Location = new System.Drawing.Point(623, 12);
            this.picState.Name = "picState";
            this.picState.Size = new System.Drawing.Size(48, 48);
            this.picState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picState.TabIndex = 9;
            this.picState.TabStop = false;
            // 
            // btnAuto
            // 
            this.btnAuto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAuto.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.do_32;
            this.btnAuto.Location = new System.Drawing.Point(623, 69);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(49, 32);
            this.btnAuto.TabIndex = 8;
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.AutoHandler);
            // 
            // lblPending
            // 
            this.lblPending.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPending.Location = new System.Drawing.Point(69, 79);
            this.lblPending.Name = "lblPending";
            this.lblPending.Size = new System.Drawing.Size(546, 15);
            this.lblPending.TabIndex = 7;
            this.lblPending.Text = "Nothing";
            // 
            // lblPendlingLabel
            // 
            this.lblPendlingLabel.AutoSize = true;
            this.lblPendlingLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPendlingLabel.Location = new System.Drawing.Point(12, 79);
            this.lblPendlingLabel.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.lblPendlingLabel.Name = "lblPendlingLabel";
            this.lblPendlingLabel.Size = new System.Drawing.Size(53, 13);
            this.lblPendlingLabel.TabIndex = 6;
            this.lblPendlingLabel.Text = "Pending:";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(72, 50);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(541, 13);
            this.progressBar.TabIndex = 5;
            // 
            // lblProgressLabel
            // 
            this.lblProgressLabel.AutoSize = true;
            this.lblProgressLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblProgressLabel.Location = new System.Drawing.Point(12, 50);
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
            this.lblInfo.Location = new System.Drawing.Point(69, 29);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(546, 13);
            this.lblInfo.TabIndex = 3;
            // 
            // lblInfoLabel
            // 
            this.lblInfoLabel.AutoSize = true;
            this.lblInfoLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblInfoLabel.Location = new System.Drawing.Point(12, 29);
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
            this.lblTask.Location = new System.Drawing.Point(69, 8);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(546, 13);
            this.lblTask.TabIndex = 1;
            this.lblTask.Text = "none";
            // 
            // lblTaskLabel
            // 
            this.lblTaskLabel.AutoSize = true;
            this.lblTaskLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTaskLabel.Location = new System.Drawing.Point(12, 8);
            this.lblTaskLabel.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.lblTaskLabel.Name = "lblTaskLabel";
            this.lblTaskLabel.Size = new System.Drawing.Size(31, 13);
            this.lblTaskLabel.TabIndex = 0;
            this.lblTaskLabel.Text = "Task:";
            // 
            // gridApps
            // 
            this.gridApps.AllowUserToAddRows = false;
            this.gridApps.AllowUserToDeleteRows = false;
            this.gridApps.AllowUserToResizeRows = false;
            this.gridApps.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridApps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridApps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridApps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIcon,
            this.colIndex,
            this.colLibrary,
            this.colID,
            this.colCategory,
            this.colLabel,
            this.colVersion,
            this.colActivated,
            this.colExcluded,
            this.colStatus,
            this.colTyp,
            this.colLicense,
            this.colComment});
            this.gridApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridApps.Location = new System.Drawing.Point(0, 133);
            this.gridApps.MultiSelect = false;
            this.gridApps.Name = "gridApps";
            this.gridApps.RowHeadersVisible = false;
            this.gridApps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridApps.ShowEditingIcon = false;
            this.gridApps.Size = new System.Drawing.Size(684, 165);
            this.gridApps.TabIndex = 8;
            this.gridApps.VirtualMode = true;
            this.gridApps.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridApps_CellContentClick);
            this.gridApps.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridApps_CellDoubleClick);
            this.gridApps.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridApps_ColumnHeaderMouseClick);
            this.gridApps.RowContextMenuStripNeeded += new System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventHandler(this.gridApps_RowContextMenuStripNeeded);
            // 
            // colIcon
            // 
            this.colIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colIcon.DataPropertyName = "StatusIcon";
            this.colIcon.Frozen = true;
            this.colIcon.HeaderText = "";
            this.colIcon.Name = "colIcon";
            this.colIcon.ReadOnly = true;
            this.colIcon.Width = 32;
            // 
            // colIndex
            // 
            this.colIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colIndex.DataPropertyName = "Index";
            this.colIndex.Frozen = true;
            this.colIndex.HeaderText = "Order";
            this.colIndex.Name = "colIndex";
            this.colIndex.ReadOnly = true;
            this.colIndex.ToolTipText = "The index number from the app registry.";
            this.colIndex.Width = 62;
            // 
            // colLibrary
            // 
            this.colLibrary.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLibrary.DataPropertyName = "AppLibrary";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colLibrary.DefaultCellStyle = dataGridViewCellStyle1;
            this.colLibrary.Frozen = true;
            this.colLibrary.HeaderText = "Library";
            this.colLibrary.Name = "colLibrary";
            this.colLibrary.ReadOnly = true;
            this.colLibrary.ToolTipText = "The ID of the library, this app is defined in.";
            this.colLibrary.Width = 66;
            // 
            // colID
            // 
            this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colID.DataPropertyName = "ID";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colID.DefaultCellStyle = dataGridViewCellStyle2;
            this.colID.Frozen = true;
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.ToolTipText = "The full ID of the app including the namespace.";
            this.colID.Width = 43;
            // 
            // colCategory
            // 
            this.colCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCategory.DataPropertyName = "Category";
            this.colCategory.Frozen = true;
            this.colCategory.HeaderText = "Category";
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            this.colCategory.ToolTipText = "The category of the app.";
            this.colCategory.Width = 78;
            // 
            // colLabel
            // 
            this.colLabel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLabel.DataPropertyName = "Label";
            this.colLabel.Frozen = true;
            this.colLabel.HeaderText = "Label";
            this.colLabel.Name = "colLabel";
            this.colLabel.ReadOnly = true;
            this.colLabel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colLabel.ToolTipText = "The user friendly name of the app.";
            this.colLabel.Width = 59;
            // 
            // colVersion
            // 
            this.colVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colVersion.DataPropertyName = "Version";
            this.colVersion.Frozen = true;
            this.colVersion.HeaderText = "Version";
            this.colVersion.Name = "colVersion";
            this.colVersion.ReadOnly = true;
            this.colVersion.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colVersion.ToolTipText = "The version number of the app.";
            this.colVersion.Width = 70;
            // 
            // colActivated
            // 
            this.colActivated.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colActivated.DataPropertyName = "IsActive";
            this.colActivated.FalseValue = "inactive";
            this.colActivated.Frozen = true;
            this.colActivated.HeaderText = "Active";
            this.colActivated.IndeterminateValue = "implicit";
            this.colActivated.Name = "colActivated";
            this.colActivated.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colActivated.ThreeState = true;
            this.colActivated.ToolTipText = "States whether the app is activated by the user or not.";
            this.colActivated.TrueValue = "active";
            this.colActivated.Width = 62;
            // 
            // colExcluded
            // 
            this.colExcluded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colExcluded.DataPropertyName = "IsDeactivated";
            this.colExcluded.Frozen = true;
            this.colExcluded.HeaderText = "Deactivated";
            this.colExcluded.Name = "colExcluded";
            this.colExcluded.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colExcluded.ToolTipText = "States whether the app is deactivated by the user.";
            this.colExcluded.Width = 92;
            // 
            // colStatus
            // 
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colStatus.DataPropertyName = "ShortStatus";
            this.colStatus.Frozen = true;
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.ToolTipText = "A brief description of the apps status.";
            this.colStatus.Width = 64;
            // 
            // colTyp
            // 
            this.colTyp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTyp.DataPropertyName = "Typ";
            this.colTyp.Frozen = true;
            this.colTyp.HeaderText = "Typ";
            this.colTyp.Name = "colTyp";
            this.colTyp.ReadOnly = true;
            this.colTyp.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTyp.ToolTipText = "The typ of the app.";
            this.colTyp.Width = 48;
            // 
            // colLicense
            // 
            this.colLicense.DataPropertyName = "License";
            this.colLicense.Frozen = true;
            this.colLicense.HeaderText = "License";
            this.colLicense.Name = "colLicense";
            this.colLicense.ReadOnly = true;
            this.colLicense.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colComment
            // 
            this.colComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colComment.DataPropertyName = "LongStatus";
            this.colComment.HeaderText = "Comment";
            this.colComment.MinimumWidth = 100;
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            this.colComment.ToolTipText = "A more detailed description of the apps status.";
            // 
            // ctxmAppActions
            // 
            this.ctxmAppActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAppInfo,
            this.miWebsite,
            this.tsSeparatorWebsite,
            this.miInstall,
            this.miReinstall,
            this.miPackageUpgrade,
            this.miUpgrade,
            this.miUninstall,
            this.tsSeparatorDownloads,
            this.miDownloadResource,
            this.miDeleteResource});
            this.ctxmAppActions.Name = "ctxMenuAppActions";
            this.ctxmAppActions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ctxmAppActions.Size = new System.Drawing.Size(180, 214);
            // 
            // miAppInfo
            // 
            this.miAppInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.miAppInfo.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.properties_16;
            this.miAppInfo.Name = "miAppInfo";
            this.miAppInfo.Size = new System.Drawing.Size(179, 22);
            this.miAppInfo.Text = "&Property Details";
            this.miAppInfo.Click += new System.EventHandler(this.AppInfoHandler);
            // 
            // miWebsite
            // 
            this.miWebsite.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.website_16;
            this.miWebsite.Name = "miWebsite";
            this.miWebsite.Size = new System.Drawing.Size(179, 22);
            this.miWebsite.Text = "Open &Website";
            this.miWebsite.Click += new System.EventHandler(this.OpenWebsiteHandler);
            // 
            // tsSeparatorWebsite
            // 
            this.tsSeparatorWebsite.Name = "tsSeparatorWebsite";
            this.tsSeparatorWebsite.Size = new System.Drawing.Size(176, 6);
            // 
            // miInstall
            // 
            this.miInstall.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.install_16;
            this.miInstall.Name = "miInstall";
            this.miInstall.Size = new System.Drawing.Size(179, 22);
            this.miInstall.Text = "&Install";
            this.miInstall.Click += new System.EventHandler(this.InstallAppHandler);
            // 
            // miReinstall
            // 
            this.miReinstall.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.reinstall_16;
            this.miReinstall.Name = "miReinstall";
            this.miReinstall.Size = new System.Drawing.Size(179, 22);
            this.miReinstall.Text = "R&einstall";
            this.miReinstall.Click += new System.EventHandler(this.ReinstallAppHandler);
            // 
            // miPackageUpgrade
            // 
            this.miPackageUpgrade.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.upgrade_16;
            this.miPackageUpgrade.Name = "miPackageUpgrade";
            this.miPackageUpgrade.Size = new System.Drawing.Size(179, 22);
            this.miPackageUpgrade.Text = "&Upgrade Package";
            this.miPackageUpgrade.Click += new System.EventHandler(this.UpgradePackageHandler);
            // 
            // miUpgrade
            // 
            this.miUpgrade.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.upgrade_16;
            this.miUpgrade.Name = "miUpgrade";
            this.miUpgrade.Size = new System.Drawing.Size(179, 22);
            this.miUpgrade.Text = "&Upgrade";
            this.miUpgrade.Click += new System.EventHandler(this.UpgradeAppHandler);
            // 
            // miUninstall
            // 
            this.miUninstall.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.uninstall_16;
            this.miUninstall.Name = "miUninstall";
            this.miUninstall.Size = new System.Drawing.Size(179, 22);
            this.miUninstall.Text = "U&ninstall";
            this.miUninstall.Click += new System.EventHandler(this.UninstallAppHandler);
            // 
            // miDownloadResource
            // 
            this.miDownloadResource.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.download_16;
            this.miDownloadResource.Name = "miDownloadResource";
            this.miDownloadResource.Size = new System.Drawing.Size(179, 22);
            this.miDownloadResource.Text = "D&ownload Resource";
            this.miDownloadResource.Click += new System.EventHandler(this.DownloadAppResourceHandler);
            // 
            // miDeleteResource
            // 
            this.miDeleteResource.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.deletedownload_16;
            this.miDeleteResource.Name = "miDeleteResource";
            this.miDeleteResource.Size = new System.Drawing.Size(179, 22);
            this.miDeleteResource.Text = "&Delete Resource";
            this.miDeleteResource.Click += new System.EventHandler(this.DeleteAppResourceHandler);
            // 
            // splitterConsole
            // 
            this.splitterConsole.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterConsole.Location = new System.Drawing.Point(0, 456);
            this.splitterConsole.Name = "splitterConsole";
            this.splitterConsole.Size = new System.Drawing.Size(684, 5);
            this.splitterConsole.TabIndex = 9;
            this.splitterConsole.TabStop = false;
            // 
            // downloadList
            // 
            this.downloadList.AutoScroll = true;
            this.downloadList.BackColor = System.Drawing.SystemColors.Window;
            this.downloadList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.downloadList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.downloadList.Downloader = null;
            this.downloadList.Location = new System.Drawing.Point(0, 303);
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
            // tsmiAlwaysShowDownloads
            // 
            this.tsmiAlwaysShowDownloads.CheckOnClick = true;
            this.tsmiAlwaysShowDownloads.Name = "tsmiAlwaysShowDownloads";
            this.tsmiAlwaysShowDownloads.Size = new System.Drawing.Size(205, 22);
            this.tsmiAlwaysShowDownloads.Text = "Always Show &Downloads";
            this.tsmiAlwaysShowDownloads.CheckedChanged += new System.EventHandler(this.AlwaysShowDownloadsCheckedChanged);
            // 
            // tsmiRefreshView
            // 
            this.tsmiRefreshView.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.refresh_16;
            this.tsmiRefreshView.Name = "tsmiRefreshView";
            this.tsmiRefreshView.Size = new System.Drawing.Size(205, 22);
            this.tsmiRefreshView.Text = "&Refresh";
            this.tsmiRefreshView.Click += new System.EventHandler(this.RefreshViewHandler);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.gridApps);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.splitterBottom);
            this.Controls.Add(this.downloadList);
            this.Controls.Add(this.splitterConsole);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Bench - Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupForm_FormClosing);
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridApps)).EndInit();
            this.ctxmAppActions.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.Label lblTaskLabel;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblInfoLabel;
        private System.Windows.Forms.Label lblProgressLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteAllResources;
        private System.Windows.Forms.DataGridView gridApps;
        private System.Windows.Forms.ContextMenuStrip ctxmAppActions;
        private System.Windows.Forms.ToolStripMenuItem miInstall;
        private System.Windows.Forms.ToolStripMenuItem miReinstall;
        private System.Windows.Forms.ToolStripMenuItem miUpgrade;
        private System.Windows.Forms.ToolStripMenuItem miDownloadResource;
        private System.Windows.Forms.ToolStripMenuItem miDeleteResource;
        private System.Windows.Forms.ToolStripMenuItem miUninstall;
        private System.Windows.Forms.ToolStripMenuItem tsmiReinstallAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpgradeAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiUninstallAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshView;
        private System.Windows.Forms.ToolStripMenuItem miPackageUpgrade;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateEnvironment;
        private System.Windows.Forms.Label lblPending;
        private System.Windows.Forms.Label lblPendlingLabel;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.ToolStripMenuItem tsmiAuto;
        private System.Windows.Forms.ToolStripSeparator tsSeparatorDownloads;
        private System.Windows.Forms.Splitter splitterConsole;
        private System.Windows.Forms.PictureBox picState;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowAppIndex;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowCustomAppIndex;
        private System.Windows.Forms.ToolStripMenuItem tsmiDownloadAllAppResources;
        private System.Windows.Forms.ToolStripMenuItem tsmiCleanUpObsoleteResources;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem miWebsite;
        private System.Windows.Forms.ToolStripSeparator tsSeparatorWebsite;
        private System.Windows.Forms.ToolStripMenuItem miAppInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpgradeBench;
        private System.Windows.Forms.ToolStripMenuItem tsmiColumns;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateBench;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateAppLibs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.DataGridViewImageColumn colIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLibrary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colActivated;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colExcluded;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTyp;
        private System.Windows.Forms.DataGridViewLinkColumn colLicense;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportClone;
    }
}