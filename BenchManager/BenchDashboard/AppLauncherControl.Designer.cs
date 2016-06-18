namespace Mastersign.Bench.Dashboard
{
    partial class AppLauncherControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listView = new System.Windows.Forms.ListView();
            this.icons32 = new System.Windows.Forms.ImageList(this.components);
            this.icons16 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.LargeImageList = this.icons32;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(190, 135);
            this.listView.SmallImageList = this.icons16;
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 0;
            this.listView.TileSize = new System.Drawing.Size(180, 36);
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Tile;
            this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
            this.listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseClick);
            // 
            // icons32
            // 
            this.icons32.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.icons32.ImageSize = new System.Drawing.Size(32, 32);
            this.icons32.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // icons16
            // 
            this.icons16.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.icons16.ImageSize = new System.Drawing.Size(16, 16);
            this.icons16.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // AppLauncherControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView);
            this.Name = "AppLauncherControl";
            this.Size = new System.Drawing.Size(190, 135);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ImageList icons32;
        private System.Windows.Forms.ImageList icons16;
    }
}
