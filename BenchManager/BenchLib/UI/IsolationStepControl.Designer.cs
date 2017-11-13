namespace Mastersign.Bench.UI
{
    partial class IsolationStepControl
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
            System.Windows.Forms.Label lblFullyIsolatedHint;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IsolationStepControl));
            System.Windows.Forms.Label lblFullyIntegrated;
            this.radIsolated = new System.Windows.Forms.RadioButton();
            this.radIntegrated = new System.Windows.Forms.RadioButton();
            lblFullyIsolatedHint = new System.Windows.Forms.Label();
            lblFullyIntegrated = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFullyIsolatedHint
            // 
            lblFullyIsolatedHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            lblFullyIsolatedHint.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            lblFullyIsolatedHint.Location = new System.Drawing.Point(31, 35);
            lblFullyIsolatedHint.Name = "lblFullyIsolatedHint";
            lblFullyIsolatedHint.Size = new System.Drawing.Size(418, 74);
            lblFullyIsolatedHint.TabIndex = 7;
            lblFullyIsolatedHint.Text = resources.GetString("lblFullyIsolatedHint.Text");
            // 
            // lblFullyIntegrated
            // 
            lblFullyIntegrated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            lblFullyIntegrated.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            lblFullyIntegrated.Location = new System.Drawing.Point(31, 136);
            lblFullyIntegrated.Name = "lblFullyIntegrated";
            lblFullyIntegrated.Size = new System.Drawing.Size(418, 74);
            lblFullyIntegrated.TabIndex = 7;
            lblFullyIntegrated.Text = resources.GetString("lblFullyIntegrated.Text");
            // 
            // radIsolated
            // 
            this.radIsolated.AutoSize = true;
            this.radIsolated.Checked = true;
            this.radIsolated.Location = new System.Drawing.Point(15, 15);
            this.radIsolated.Name = "radIsolated";
            this.radIsolated.Size = new System.Drawing.Size(155, 17);
            this.radIsolated.TabIndex = 0;
            this.radIsolated.Text = "&Portable / Isolated (Default)";
            this.radIsolated.UseVisualStyleBackColor = true;
            // 
            // radIntegrated
            // 
            this.radIntegrated.AutoSize = true;
            this.radIntegrated.Location = new System.Drawing.Point(15, 116);
            this.radIntegrated.Name = "radIntegrated";
            this.radIntegrated.Size = new System.Drawing.Size(73, 17);
            this.radIntegrated.TabIndex = 0;
            this.radIntegrated.Text = "&Integrated";
            this.radIntegrated.UseVisualStyleBackColor = true;
            // 
            // IsolationStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(lblFullyIntegrated);
            this.Controls.Add(lblFullyIsolatedHint);
            this.Controls.Add(this.radIntegrated);
            this.Controls.Add(this.radIsolated);
            this.Name = "IsolationStepControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radIsolated;
        private System.Windows.Forms.RadioButton radIntegrated;
    }
}
