﻿namespace Mastersign.Bench.UI
{
    partial class AdvancedStepControl
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
            this.chkStartAutoSetup = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkStartAutoSetup
            // 
            this.chkStartAutoSetup.AutoSize = true;
            this.chkStartAutoSetup.Location = new System.Drawing.Point(15, 15);
            this.chkStartAutoSetup.Name = "chkStartAutoSetup";
            this.chkStartAutoSetup.Size = new System.Drawing.Size(134, 17);
            this.chkStartAutoSetup.TabIndex = 1;
            this.chkStartAutoSetup.Text = "&Start setup immediately";
            this.chkStartAutoSetup.UseVisualStyleBackColor = true;
            // 
            // AdvancedStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkStartAutoSetup);
            this.Name = "AdvancedStepControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkStartAutoSetup;
    }
}
