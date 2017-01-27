namespace Mastersign.Bench.UI
{
    partial class AppSelectionStepControl
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
            this.clstAppSelection = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // clstAppSelection
            // 
            this.clstAppSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clstAppSelection.CheckOnClick = true;
            this.clstAppSelection.FormattingEnabled = true;
            this.clstAppSelection.Location = new System.Drawing.Point(15, 15);
            this.clstAppSelection.Name = "clstAppSelection";
            this.clstAppSelection.Size = new System.Drawing.Size(434, 139);
            this.clstAppSelection.TabIndex = 0;
            // 
            // AppSelectionStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clstAppSelection);
            this.Name = "AppSelectionStepControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clstAppSelection;
    }
}
