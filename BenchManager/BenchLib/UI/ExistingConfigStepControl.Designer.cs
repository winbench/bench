namespace Mastersign.Bench.UI
{
    partial class ExistingConfigStepControl
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
            this.radNoExistingConfig = new System.Windows.Forms.RadioButton();
            this.radExistingConfigInGitRepo = new System.Windows.Forms.RadioButton();
            this.lblConfigGitRepo = new System.Windows.Forms.Label();
            this.txtConfigGitRepo = new System.Windows.Forms.TextBox();
            this.radExistingConfigInDirectory = new System.Windows.Forms.RadioButton();
            this.txtConfigTemplateDir = new System.Windows.Forms.TextBox();
            this.lblConfigTemplateDir = new System.Windows.Forms.Label();
            this.btnConfigTemplateDirBrowse = new System.Windows.Forms.Button();
            this.btnConfigTemplateZipFileBrowse = new System.Windows.Forms.Button();
            this.txtConfigTemplateZipFile = new System.Windows.Forms.TextBox();
            this.lblConfigTemplateZipFile = new System.Windows.Forms.Label();
            this.radExistingConfigInZipFile = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // radNoExistingConfig
            // 
            this.radNoExistingConfig.AutoSize = true;
            this.radNoExistingConfig.Checked = true;
            this.radNoExistingConfig.Location = new System.Drawing.Point(15, 15);
            this.radNoExistingConfig.Name = "radNoExistingConfig";
            this.radNoExistingConfig.Size = new System.Drawing.Size(245, 17);
            this.radNoExistingConfig.TabIndex = 0;
            this.radNoExistingConfig.TabStop = true;
            this.radNoExistingConfig.Text = "No existing configuration, initialize with defaults";
            this.radNoExistingConfig.UseVisualStyleBackColor = true;
            // 
            // radExistingConfigInGitRepo
            // 
            this.radExistingConfigInGitRepo.AutoSize = true;
            this.radExistingConfigInGitRepo.Location = new System.Drawing.Point(15, 130);
            this.radExistingConfigInGitRepo.Name = "radExistingConfigInGitRepo";
            this.radExistingConfigInGitRepo.Size = new System.Drawing.Size(209, 17);
            this.radExistingConfigInGitRepo.TabIndex = 1;
            this.radExistingConfigInGitRepo.Text = "Existing configuration in a Git repository";
            this.radExistingConfigInGitRepo.UseVisualStyleBackColor = true;
            this.radExistingConfigInGitRepo.CheckedChanged += new System.EventHandler(this.ConfigSourceChangedHandler);
            // 
            // lblConfigGitRepo
            // 
            this.lblConfigGitRepo.AutoSize = true;
            this.lblConfigGitRepo.Location = new System.Drawing.Point(32, 156);
            this.lblConfigGitRepo.Name = "lblConfigGitRepo";
            this.lblConfigGitRepo.Size = new System.Drawing.Size(58, 13);
            this.lblConfigGitRepo.TabIndex = 2;
            this.lblConfigGitRepo.Text = "Repo URL";
            // 
            // txtConfigGitRepo
            // 
            this.txtConfigGitRepo.Location = new System.Drawing.Point(96, 153);
            this.txtConfigGitRepo.Name = "txtConfigGitRepo";
            this.txtConfigGitRepo.Size = new System.Drawing.Size(353, 20);
            this.txtConfigGitRepo.TabIndex = 3;
            // 
            // radExistingConfigInDirectory
            // 
            this.radExistingConfigInDirectory.AutoSize = true;
            this.radExistingConfigInDirectory.Location = new System.Drawing.Point(15, 38);
            this.radExistingConfigInDirectory.Name = "radExistingConfigInDirectory";
            this.radExistingConfigInDirectory.Size = new System.Drawing.Size(364, 17);
            this.radExistingConfigInDirectory.TabIndex = 4;
            this.radExistingConfigInDirectory.Text = "Existing configuration in a directory file (e.g. existing Bench environment)";
            this.radExistingConfigInDirectory.UseVisualStyleBackColor = true;
            this.radExistingConfigInDirectory.CheckedChanged += new System.EventHandler(this.ConfigSourceChangedHandler);
            // 
            // txtConfigTemplateDir
            // 
            this.txtConfigTemplateDir.Location = new System.Drawing.Point(96, 58);
            this.txtConfigTemplateDir.Name = "txtConfigTemplateDir";
            this.txtConfigTemplateDir.Size = new System.Drawing.Size(316, 20);
            this.txtConfigTemplateDir.TabIndex = 6;
            // 
            // lblConfigTemplateDir
            // 
            this.lblConfigTemplateDir.AutoSize = true;
            this.lblConfigTemplateDir.Location = new System.Drawing.Point(32, 61);
            this.lblConfigTemplateDir.Name = "lblConfigTemplateDir";
            this.lblConfigTemplateDir.Size = new System.Drawing.Size(49, 13);
            this.lblConfigTemplateDir.TabIndex = 5;
            this.lblConfigTemplateDir.Text = "Directory";
            // 
            // btnConfigTemplateDirBrowse
            // 
            this.btnConfigTemplateDirBrowse.Location = new System.Drawing.Point(418, 56);
            this.btnConfigTemplateDirBrowse.Name = "btnConfigTemplateDirBrowse";
            this.btnConfigTemplateDirBrowse.Size = new System.Drawing.Size(31, 23);
            this.btnConfigTemplateDirBrowse.TabIndex = 7;
            this.btnConfigTemplateDirBrowse.Text = "...";
            this.btnConfigTemplateDirBrowse.UseVisualStyleBackColor = true;
            this.btnConfigTemplateDirBrowse.Click += new System.EventHandler(this.BrowseTemplateDirHandler);
            // 
            // btnConfigTemplateZipFileBrowse
            // 
            this.btnConfigTemplateZipFileBrowse.Location = new System.Drawing.Point(418, 102);
            this.btnConfigTemplateZipFileBrowse.Name = "btnConfigTemplateZipFileBrowse";
            this.btnConfigTemplateZipFileBrowse.Size = new System.Drawing.Size(31, 23);
            this.btnConfigTemplateZipFileBrowse.TabIndex = 11;
            this.btnConfigTemplateZipFileBrowse.Text = "...";
            this.btnConfigTemplateZipFileBrowse.UseVisualStyleBackColor = true;
            this.btnConfigTemplateZipFileBrowse.Click += new System.EventHandler(this.BrowseTemplateZipFileHandler);
            // 
            // txtConfigTemplateZipFile
            // 
            this.txtConfigTemplateZipFile.Location = new System.Drawing.Point(96, 104);
            this.txtConfigTemplateZipFile.Name = "txtConfigTemplateZipFile";
            this.txtConfigTemplateZipFile.Size = new System.Drawing.Size(316, 20);
            this.txtConfigTemplateZipFile.TabIndex = 10;
            // 
            // lblConfigTemplateZipFile
            // 
            this.lblConfigTemplateZipFile.AutoSize = true;
            this.lblConfigTemplateZipFile.Location = new System.Drawing.Point(32, 107);
            this.lblConfigTemplateZipFile.Name = "lblConfigTemplateZipFile";
            this.lblConfigTemplateZipFile.Size = new System.Drawing.Size(40, 13);
            this.lblConfigTemplateZipFile.TabIndex = 9;
            this.lblConfigTemplateZipFile.Text = "ZIP file";
            // 
            // radExistingConfigInZipFile
            // 
            this.radExistingConfigInZipFile.AutoSize = true;
            this.radExistingConfigInZipFile.Location = new System.Drawing.Point(15, 84);
            this.radExistingConfigInZipFile.Name = "radExistingConfigInZipFile";
            this.radExistingConfigInZipFile.Size = new System.Drawing.Size(181, 17);
            this.radExistingConfigInZipFile.TabIndex = 8;
            this.radExistingConfigInZipFile.Text = "Existing configuration in a ZIP file";
            this.radExistingConfigInZipFile.UseVisualStyleBackColor = true;
            this.radExistingConfigInZipFile.CheckedChanged += new System.EventHandler(this.ConfigSourceChangedHandler);
            // 
            // ExistingConfigStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnConfigTemplateZipFileBrowse);
            this.Controls.Add(this.txtConfigTemplateZipFile);
            this.Controls.Add(this.lblConfigTemplateZipFile);
            this.Controls.Add(this.radExistingConfigInZipFile);
            this.Controls.Add(this.btnConfigTemplateDirBrowse);
            this.Controls.Add(this.txtConfigTemplateDir);
            this.Controls.Add(this.lblConfigTemplateDir);
            this.Controls.Add(this.radExistingConfigInDirectory);
            this.Controls.Add(this.txtConfigGitRepo);
            this.Controls.Add(this.lblConfigGitRepo);
            this.Controls.Add(this.radExistingConfigInGitRepo);
            this.Controls.Add(this.radNoExistingConfig);
            this.Name = "ExistingConfigStepControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radNoExistingConfig;
        private System.Windows.Forms.RadioButton radExistingConfigInGitRepo;
        private System.Windows.Forms.TextBox txtConfigGitRepo;
        private System.Windows.Forms.Label lblConfigGitRepo;
        private System.Windows.Forms.RadioButton radExistingConfigInDirectory;
        private System.Windows.Forms.TextBox txtConfigTemplateDir;
        private System.Windows.Forms.Label lblConfigTemplateDir;
        private System.Windows.Forms.Button btnConfigTemplateDirBrowse;
        private System.Windows.Forms.Button btnConfigTemplateZipFileBrowse;
        private System.Windows.Forms.TextBox txtConfigTemplateZipFile;
        private System.Windows.Forms.Label lblConfigTemplateZipFile;
        private System.Windows.Forms.RadioButton radExistingConfigInZipFile;
    }
}
