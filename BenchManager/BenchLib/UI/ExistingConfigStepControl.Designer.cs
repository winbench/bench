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
            this.radExistingConfigInGitRepo.Location = new System.Drawing.Point(15, 38);
            this.radExistingConfigInGitRepo.Name = "radExistingConfigInGitRepo";
            this.radExistingConfigInGitRepo.Size = new System.Drawing.Size(209, 17);
            this.radExistingConfigInGitRepo.TabIndex = 1;
            this.radExistingConfigInGitRepo.Text = "Existing configuration in a Git repository";
            this.radExistingConfigInGitRepo.UseVisualStyleBackColor = true;
            this.radExistingConfigInGitRepo.CheckedChanged += new System.EventHandler(this.radExistingConfigInGitRepo_CheckedChanged);
            // 
            // lblConfigGitRepo
            // 
            this.lblConfigGitRepo.AutoSize = true;
            this.lblConfigGitRepo.Location = new System.Drawing.Point(32, 64);
            this.lblConfigGitRepo.Name = "lblConfigGitRepo";
            this.lblConfigGitRepo.Size = new System.Drawing.Size(58, 13);
            this.lblConfigGitRepo.TabIndex = 2;
            this.lblConfigGitRepo.Text = "Repo URL";
            // 
            // txtConfigGitRepo
            // 
            this.txtConfigGitRepo.Location = new System.Drawing.Point(96, 61);
            this.txtConfigGitRepo.Name = "txtConfigGitRepo";
            this.txtConfigGitRepo.Size = new System.Drawing.Size(353, 20);
            this.txtConfigGitRepo.TabIndex = 3;
            // 
            // ExistingConfigStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
    }
}
