namespace Mastersign.Bench.Dashboard
{
    partial class MarkdownViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarkdownViewer));
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitter = new System.Windows.Forms.Splitter();
            this.markdownControl = new Mastersign.Bench.Dashboard.MarkdownControl();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(200, 441);
            this.treeView.TabIndex = 1;
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(200, 0);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(6, 441);
            this.splitter.TabIndex = 2;
            this.splitter.TabStop = false;
            // 
            // markdownControl
            // 
            this.markdownControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.markdownControl.Location = new System.Drawing.Point(206, 0);
            this.markdownControl.Name = "markdownControl";
            this.markdownControl.Size = new System.Drawing.Size(418, 441);
            this.markdownControl.TabIndex = 3;
            // 
            // MarkdownViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.markdownControl);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.treeView);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MarkdownViewer";
            this.Text = "Bench Markdown Viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MarkdownViewer_FormClosed);
            this.Load += new System.EventHandler(this.MarkdownViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Splitter splitter;
        private MarkdownControl markdownControl;
    }
}