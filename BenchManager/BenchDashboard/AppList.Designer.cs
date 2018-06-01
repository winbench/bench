namespace Mastersign.Bench.Dashboard
{
    partial class AppList
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.tsSeparatorDownloads = new System.Windows.Forms.ToolStripSeparator();
            this.miDownloadResource = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteResource = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridApps)).BeginInit();
            this.ctxmAppActions.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
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
            this.gridApps.Location = new System.Drawing.Point(0, 32);
            this.gridApps.MultiSelect = false;
            this.gridApps.Name = "gridApps";
            this.gridApps.RowHeadersVisible = false;
            this.gridApps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridApps.ShowEditingIcon = false;
            this.gridApps.Size = new System.Drawing.Size(1044, 313);
            this.gridApps.TabIndex = 1;
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
            this.colIndex.Width = 58;
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
            this.colLibrary.Width = 63;
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
            this.colCategory.Width = 74;
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
            this.colLabel.Width = 58;
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
            this.colVersion.Width = 67;
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
            this.colActivated.TrueValue = "activated";
            this.colActivated.Width = 62;
            // 
            // colExcluded
            // 
            this.colExcluded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colExcluded.DataPropertyName = "IsSuppressed";
            this.colExcluded.FalseValue = "supported";
            this.colExcluded.Frozen = true;
            this.colExcluded.HeaderText = "Deactivated";
            this.colExcluded.IndeterminateValue = "implicit";
            this.colExcluded.Name = "colExcluded";
            this.colExcluded.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colExcluded.ThreeState = true;
            this.colExcluded.ToolTipText = "States whether the app is deactivated by the user.";
            this.colExcluded.TrueValue = "deactivated";
            this.colExcluded.Width = 90;
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
            this.colStatus.Width = 62;
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
            this.colTyp.Width = 50;
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
            // tsSeparatorDownloads
            // 
            this.tsSeparatorDownloads.Name = "tsSeparatorDownloads";
            this.tsSeparatorDownloads.Size = new System.Drawing.Size(176, 6);
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
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnClearSearch);
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1044, 32);
            this.panelTop.TabIndex = 0;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSearch.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.remove_16;
            this.btnClearSearch.Location = new System.Drawing.Point(1015, 4);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(24, 24);
            this.btnClearSearch.TabIndex = 1;
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(5, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(1010, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.WordWrap = false;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // AppList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridApps);
            this.Controls.Add(this.panelTop);
            this.Name = "AppList";
            this.Size = new System.Drawing.Size(1044, 345);
            ((System.ComponentModel.ISupportInitialize)(this.gridApps)).EndInit();
            this.ctxmAppActions.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridApps;
        private System.Windows.Forms.ContextMenuStrip ctxmAppActions;
        private System.Windows.Forms.ToolStripMenuItem miAppInfo;
        private System.Windows.Forms.ToolStripMenuItem miWebsite;
        private System.Windows.Forms.ToolStripSeparator tsSeparatorWebsite;
        private System.Windows.Forms.ToolStripMenuItem miInstall;
        private System.Windows.Forms.ToolStripMenuItem miReinstall;
        private System.Windows.Forms.ToolStripMenuItem miPackageUpgrade;
        private System.Windows.Forms.ToolStripMenuItem miUpgrade;
        private System.Windows.Forms.ToolStripMenuItem miUninstall;
        private System.Windows.Forms.ToolStripSeparator tsSeparatorDownloads;
        private System.Windows.Forms.ToolStripMenuItem miDownloadResource;
        private System.Windows.Forms.ToolStripMenuItem miDeleteResource;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.TextBox txtSearch;
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
    }
}
