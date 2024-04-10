namespace Mastersign.Bench.Dashboard
{
    partial class ExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            this.panelContent = new System.Windows.Forms.Panel();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.grpContentSelection = new System.Windows.Forms.GroupBox();
            this.lblAppsHint = new System.Windows.Forms.Label();
            this.lblCacheHint = new System.Windows.Forms.Label();
            this.chkApps = new System.Windows.Forms.CheckBox();
            this.chkCache = new System.Windows.Forms.CheckBox();
            this.chkRequiredApps = new System.Windows.Forms.CheckBox();
            this.chkRequiredCache = new System.Windows.Forms.CheckBox();
            this.chkAppLibraries = new System.Windows.Forms.CheckBox();
            this.chkProjects = new System.Windows.Forms.CheckBox();
            this.chkHomeDirectory = new System.Windows.Forms.CheckBox();
            this.chkUserConfiguration = new System.Windows.Forms.CheckBox();
            this.radClone = new System.Windows.Forms.RadioButton();
            this.radExport = new System.Windows.Forms.RadioButton();
            this.panelHead = new System.Windows.Forms.Panel();
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelContent.SuspendLayout();
            this.grpContentSelection.SuspendLayout();
            this.panelHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.SystemColors.Window;
            this.panelContent.Controls.Add(this.btnBrowse);
            this.panelContent.Controls.Add(this.txtTarget);
            this.panelContent.Controls.Add(this.lblTarget);
            this.panelContent.Controls.Add(this.grpContentSelection);
            this.panelContent.Controls.Add(this.radClone);
            this.panelContent.Controls.Add(this.radExport);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 70);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(8);
            this.panelContent.Size = new System.Drawing.Size(624, 323);
            this.panelContent.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.browse_file_16;
            this.btnBrowse.Location = new System.Drawing.Point(578, 63);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(34, 24);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BrowseTargetClickHandler);
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.Location = new System.Drawing.Point(92, 64);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(481, 22);
            this.txtTarget.TabIndex = 3;
            this.txtTarget.TextChanged += new System.EventHandler(this.TargetTextChangedHandler);
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(19, 67);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(68, 13);
            this.lblTarget.TabIndex = 2;
            this.lblTarget.Text = "Target Path:";
            // 
            // grpContentSelection
            // 
            this.grpContentSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpContentSelection.Controls.Add(this.lblAppsHint);
            this.grpContentSelection.Controls.Add(this.lblCacheHint);
            this.grpContentSelection.Controls.Add(this.chkApps);
            this.grpContentSelection.Controls.Add(this.chkCache);
            this.grpContentSelection.Controls.Add(this.chkRequiredApps);
            this.grpContentSelection.Controls.Add(this.chkRequiredCache);
            this.grpContentSelection.Controls.Add(this.chkAppLibraries);
            this.grpContentSelection.Controls.Add(this.chkProjects);
            this.grpContentSelection.Controls.Add(this.chkHomeDirectory);
            this.grpContentSelection.Controls.Add(this.chkUserConfiguration);
            this.grpContentSelection.Location = new System.Drawing.Point(12, 96);
            this.grpContentSelection.Name = "grpContentSelection";
            this.grpContentSelection.Size = new System.Drawing.Size(600, 216);
            this.grpContentSelection.TabIndex = 5;
            this.grpContentSelection.TabStop = false;
            this.grpContentSelection.Text = "Include";
            // 
            // lblAppsHint
            // 
            this.lblAppsHint.AutoSize = true;
            this.lblAppsHint.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblAppsHint.Location = new System.Drawing.Point(148, 187);
            this.lblAppsHint.Name = "lblAppsHint";
            this.lblAppsHint.Size = new System.Drawing.Size(376, 13);
            this.lblAppsHint.TabIndex = 8;
            this.lblAppsHint.Text = "Do not use this option for the *.exe format. It must be smaller then 2GB.";
            // 
            // lblCacheHint
            // 
            this.lblCacheHint.AutoSize = true;
            this.lblCacheHint.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCacheHint.Location = new System.Drawing.Point(193, 141);
            this.lblCacheHint.Name = "lblCacheHint";
            this.lblCacheHint.Size = new System.Drawing.Size(376, 13);
            this.lblCacheHint.TabIndex = 8;
            this.lblCacheHint.Text = "Do not use this option for the *.exe format. It must be smaller then 2GB.";
            // 
            // chkApps
            // 
            this.chkApps.AutoSize = true;
            this.chkApps.Location = new System.Drawing.Point(14, 186);
            this.chkApps.Name = "chkApps";
            this.chkApps.Size = new System.Drawing.Size(128, 17);
            this.chkApps.TabIndex = 7;
            this.chkApps.Text = "All apps as installed";
            this.chkApps.UseVisualStyleBackColor = true;
            // 
            // chkCache
            // 
            this.chkCache.AutoSize = true;
            this.chkCache.Location = new System.Drawing.Point(14, 140);
            this.chkCache.Name = "chkCache";
            this.chkCache.Size = new System.Drawing.Size(173, 17);
            this.chkCache.TabIndex = 5;
            this.chkCache.Text = "Cached resources of all apps";
            this.chkCache.UseVisualStyleBackColor = true;
            // 
            // chkRequiredApps
            // 
            this.chkRequiredApps.AutoSize = true;
            this.chkRequiredApps.Location = new System.Drawing.Point(14, 163);
            this.chkRequiredApps.Name = "chkRequiredApps";
            this.chkRequiredApps.Size = new System.Drawing.Size(162, 17);
            this.chkRequiredApps.TabIndex = 6;
            this.chkRequiredApps.Text = "Required apps as installed";
            this.chkRequiredApps.UseVisualStyleBackColor = true;
            // 
            // chkRequiredCache
            // 
            this.chkRequiredCache.AutoSize = true;
            this.chkRequiredCache.Location = new System.Drawing.Point(14, 117);
            this.chkRequiredCache.Name = "chkRequiredCache";
            this.chkRequiredCache.Size = new System.Drawing.Size(205, 17);
            this.chkRequiredCache.TabIndex = 4;
            this.chkRequiredCache.Text = "Cached resources of required apps";
            this.chkRequiredCache.UseVisualStyleBackColor = true;
            // 
            // chkAppLibraries
            // 
            this.chkAppLibraries.AutoSize = true;
            this.chkAppLibraries.Location = new System.Drawing.Point(14, 94);
            this.chkAppLibraries.Name = "chkAppLibraries";
            this.chkAppLibraries.Size = new System.Drawing.Size(131, 17);
            this.chkAppLibraries.TabIndex = 3;
            this.chkAppLibraries.Text = "Loaded app libraries";
            this.chkAppLibraries.UseVisualStyleBackColor = true;
            // 
            // chkProjects
            // 
            this.chkProjects.AutoSize = true;
            this.chkProjects.Location = new System.Drawing.Point(14, 71);
            this.chkProjects.Name = "chkProjects";
            this.chkProjects.Size = new System.Drawing.Size(114, 17);
            this.chkProjects.TabIndex = 2;
            this.chkProjects.Text = "Projects directory";
            this.chkProjects.UseVisualStyleBackColor = true;
            // 
            // chkHomeDirectory
            // 
            this.chkHomeDirectory.AutoSize = true;
            this.chkHomeDirectory.Location = new System.Drawing.Point(14, 48);
            this.chkHomeDirectory.Name = "chkHomeDirectory";
            this.chkHomeDirectory.Size = new System.Drawing.Size(178, 17);
            this.chkHomeDirectory.TabIndex = 1;
            this.chkHomeDirectory.Text = "Home directory with AppData";
            this.chkHomeDirectory.UseVisualStyleBackColor = true;
            // 
            // chkUserConfiguration
            // 
            this.chkUserConfiguration.AutoSize = true;
            this.chkUserConfiguration.Location = new System.Drawing.Point(14, 25);
            this.chkUserConfiguration.Name = "chkUserConfiguration";
            this.chkUserConfiguration.Size = new System.Drawing.Size(229, 17);
            this.chkUserConfiguration.TabIndex = 0;
            this.chkUserConfiguration.Text = "User configuration and user app library";
            this.chkUserConfiguration.UseVisualStyleBackColor = true;
            // 
            // radClone
            // 
            this.radClone.AutoSize = true;
            this.radClone.Location = new System.Drawing.Point(26, 34);
            this.radClone.Name = "radClone";
            this.radClone.Size = new System.Drawing.Size(382, 17);
            this.radClone.TabIndex = 1;
            this.radClone.Text = "Clone the Bench environment to a different location in the file system\r\n";
            this.radClone.UseVisualStyleBackColor = true;
            this.radClone.CheckedChanged += new System.EventHandler(this.ModeSelectionChangedHandler);
            // 
            // radExport
            // 
            this.radExport.AutoSize = true;
            this.radExport.Checked = true;
            this.radExport.Location = new System.Drawing.Point(26, 11);
            this.radExport.Name = "radExport";
            this.radExport.Size = new System.Drawing.Size(385, 17);
            this.radExport.TabIndex = 0;
            this.radExport.TabStop = true;
            this.radExport.Text = "Export the Bench environment as compressed archive (*.zip, *.7z, *.exe)";
            this.radExport.UseVisualStyleBackColor = true;
            this.radExport.CheckedChanged += new System.EventHandler(this.ModeSelectionChangedHandler);
            // 
            // panelHead
            // 
            this.panelHead.Controls.Add(this.lblCurrentStep);
            this.panelHead.Controls.Add(this.lblTitle);
            this.panelHead.Controls.Add(this.picIcon);
            this.panelHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHead.Location = new System.Drawing.Point(0, 0);
            this.panelHead.Name = "panelHead";
            this.panelHead.Size = new System.Drawing.Size(624, 70);
            this.panelHead.TabIndex = 0;
            // 
            // lblCurrentStep
            // 
            this.lblCurrentStep.AutoSize = true;
            this.lblCurrentStep.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentStep.Location = new System.Drawing.Point(14, 42);
            this.lblCurrentStep.Name = "lblCurrentStep";
            this.lblCurrentStep.Size = new System.Drawing.Size(245, 17);
            this.lblCurrentStep.TabIndex = 1;
            this.lblCurrentStep.Text = "Select mode and content for the transfer";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(406, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Exporting or Cloning the Bench Environment";
            // 
            // picIcon
            // 
            this.picIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picIcon.Location = new System.Drawing.Point(564, 11);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(48, 48);
            this.picIcon.TabIndex = 0;
            this.picIcon.TabStop = false;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.btnCancel);
            this.panelFooter.Controls.Add(this.btnOK);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 393);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(624, 48);
            this.panelFooter.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(456, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.CancelHandler);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(537, 13);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OkHandler);
            // 
            // ExportForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHead);
            this.Controls.Add(this.panelFooter);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportForm";
            this.Text = "Bench - Export and Cloning";
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.grpContentSelection.ResumeLayout(false);
            this.grpContentSelection.PerformLayout();
            this.panelHead.ResumeLayout(false);
            this.panelHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.GroupBox grpContentSelection;
        private System.Windows.Forms.CheckBox chkApps;
        private System.Windows.Forms.CheckBox chkCache;
        private System.Windows.Forms.CheckBox chkRequiredApps;
        private System.Windows.Forms.CheckBox chkRequiredCache;
        private System.Windows.Forms.CheckBox chkAppLibraries;
        private System.Windows.Forms.CheckBox chkProjects;
        private System.Windows.Forms.CheckBox chkHomeDirectory;
        private System.Windows.Forms.CheckBox chkUserConfiguration;
        private System.Windows.Forms.RadioButton radClone;
        private System.Windows.Forms.RadioButton radExport;
        private System.Windows.Forms.Panel panelHead;
        private System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblCacheHint;
        private System.Windows.Forms.Label lblAppsHint;
    }
}