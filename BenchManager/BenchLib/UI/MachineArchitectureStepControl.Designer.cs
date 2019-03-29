namespace Mastersign.Bench.UI
{
    partial class MachineArchitectureStepControl
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
            System.Windows.Forms.Label lblAllow64BitHint;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MachineArchitectureStepControl));
            this.chkAllow64Bit = new System.Windows.Forms.CheckBox();
            lblAllow64BitHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAllow64BitHint
            // 
            lblAllow64BitHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            lblAllow64BitHint.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            lblAllow64BitHint.Location = new System.Drawing.Point(31, 35);
            lblAllow64BitHint.Name = "lblAllow64BitHint";
            lblAllow64BitHint.Size = new System.Drawing.Size(418, 58);
            lblAllow64BitHint.TabIndex = 6;
            lblAllow64BitHint.Text = resources.GetString("lblAllow64BitHint.Text");
            // 
            // chkAllow64Bit
            // 
            this.chkAllow64Bit.AutoSize = true;
            this.chkAllow64Bit.Location = new System.Drawing.Point(15, 15);
            this.chkAllow64Bit.Name = "chkAllow64Bit";
            this.chkAllow64Bit.Size = new System.Drawing.Size(182, 17);
            this.chkAllow64Bit.TabIndex = 5;
            this.chkAllow64Bit.Text = "Allow the usage of &64 Bit binaries";
            this.chkAllow64Bit.UseVisualStyleBackColor = true;
            // 
            // MachineArchitectureStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(lblAllow64BitHint);
            this.Controls.Add(this.chkAllow64Bit);
            this.Name = "MachineArchitectureStepControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkAllow64Bit;
    }
}
