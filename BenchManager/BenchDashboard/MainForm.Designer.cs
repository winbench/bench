namespace Mastersign.Bench.Dashboard
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripStatusLabel tsslRootPathLabel;
            System.Windows.Forms.ToolStripStatusLabel tsslAppCountLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslRootPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAppCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnAutoSetup = new System.Windows.Forms.Button();
            this.btnDocs = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnShellBash = new System.Windows.Forms.Button();
            this.btnShellPowerShell = new System.Windows.Forms.Button();
            this.btnShellCmd = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.appLauncherList = new Mastersign.Bench.Dashboard.AppLauncherControl();
            this.tsslSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslVersionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslVersionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            tsslRootPathLabel = new System.Windows.Forms.ToolStripStatusLabel();
            tsslAppCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsslRootPathLabel,
            this.tsslRootPath,
            tsslAppCountLabel,
            this.tsslAppCount,
            this.tsslSpacer,
            this.tsslVersionLabel,
            this.tsslVersion,
            this.tsslVersionStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 337);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(564, 24);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tsslRootPathLabel
            // 
            tsslRootPathLabel.Name = "tsslRootPathLabel";
            tsslRootPathLabel.Size = new System.Drawing.Size(62, 19);
            tsslRootPathLabel.Text = "Root Path:";
            // 
            // tsslRootPath
            // 
            this.tsslRootPath.IsLink = true;
            this.tsslRootPath.Name = "tsslRootPath";
            this.tsslRootPath.Size = new System.Drawing.Size(47, 19);
            this.tsslRootPath.Text = "<Path>";
            this.tsslRootPath.Click += new System.EventHandler(this.RootPathClickHandler);
            // 
            // tsslAppCountLabel
            // 
            tsslAppCountLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            tsslAppCountLabel.Margin = new System.Windows.Forms.Padding(2, 3, 0, 2);
            tsslAppCountLabel.Name = "tsslAppCountLabel";
            tsslAppCountLabel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            tsslAppCountLabel.Size = new System.Drawing.Size(81, 19);
            tsslAppCountLabel.Text = "Active Apps:";
            // 
            // tsslAppCount
            // 
            this.tsslAppCount.Name = "tsslAppCount";
            this.tsslAppCount.Size = new System.Drawing.Size(56, 19);
            this.tsslAppCount.Text = "<Count>";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnAutoSetup);
            this.panelTop.Controls.Add(this.btnDocs);
            this.panelTop.Controls.Add(this.btnAbout);
            this.panelTop.Controls.Add(this.btnShellBash);
            this.panelTop.Controls.Add(this.btnShellPowerShell);
            this.panelTop.Controls.Add(this.btnShellCmd);
            this.panelTop.Controls.Add(this.btnSetup);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(564, 29);
            this.panelTop.TabIndex = 0;
            // 
            // btnAutoSetup
            // 
            this.btnAutoSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoSetup.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.do_16;
            this.btnAutoSetup.Location = new System.Drawing.Point(466, 2);
            this.btnAutoSetup.Margin = new System.Windows.Forms.Padding(2);
            this.btnAutoSetup.Name = "btnAutoSetup";
            this.btnAutoSetup.Size = new System.Drawing.Size(29, 25);
            this.btnAutoSetup.TabIndex = 6;
            this.toolTip.SetToolTip(this.btnAutoSetup, "Bench Auto Setup");
            this.btnAutoSetup.Click += new System.EventHandler(this.AutoSetupHandler);
            // 
            // btnDocs
            // 
            this.btnDocs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDocs.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.docs_16;
            this.btnDocs.Location = new System.Drawing.Point(433, 2);
            this.btnDocs.Margin = new System.Windows.Forms.Padding(2);
            this.btnDocs.Name = "btnDocs";
            this.btnDocs.Size = new System.Drawing.Size(29, 25);
            this.btnDocs.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnDocs, "Documentation and Online Resources");
            this.btnDocs.Click += new System.EventHandler(this.DocsHandler);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbout.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.info_16;
            this.btnAbout.Location = new System.Drawing.Point(532, 2);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(2);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(29, 25);
            this.btnAbout.TabIndex = 5;
            this.toolTip.SetToolTip(this.btnAbout, "Bench Info");
            this.btnAbout.Click += new System.EventHandler(this.AboutHandler);
            // 
            // btnShellBash
            // 
            this.btnShellBash.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.missing_app_16;
            this.btnShellBash.Location = new System.Drawing.Point(64, 2);
            this.btnShellBash.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.btnShellBash.Name = "btnShellBash";
            this.btnShellBash.Size = new System.Drawing.Size(29, 25);
            this.btnShellBash.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnShellBash, "Git Bash");
            this.btnShellBash.Click += new System.EventHandler(this.ShellBashHandler);
            // 
            // btnShellPowerShell
            // 
            this.btnShellPowerShell.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.missing_app_16;
            this.btnShellPowerShell.Location = new System.Drawing.Point(33, 2);
            this.btnShellPowerShell.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.btnShellPowerShell.Name = "btnShellPowerShell";
            this.btnShellPowerShell.Size = new System.Drawing.Size(29, 25);
            this.btnShellPowerShell.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnShellPowerShell, "Windows PowerShell");
            this.btnShellPowerShell.Click += new System.EventHandler(this.ShellPowerShellHandler);
            // 
            // btnShellCmd
            // 
            this.btnShellCmd.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.missing_app_16;
            this.btnShellCmd.Location = new System.Drawing.Point(2, 2);
            this.btnShellCmd.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.btnShellCmd.Name = "btnShellCmd";
            this.btnShellCmd.Size = new System.Drawing.Size(29, 25);
            this.btnShellCmd.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnShellCmd, "Windows Command Prompt");
            this.btnShellCmd.Click += new System.EventHandler(this.ShellCmdHandler);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.setup_16;
            this.btnSetup.Location = new System.Drawing.Point(499, 2);
            this.btnSetup.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(29, 25);
            this.btnSetup.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnSetup, "Bench Setup and Configuration");
            this.btnSetup.Click += new System.EventHandler(this.SetupHandler);
            // 
            // appLauncherList
            // 
            this.appLauncherList.AppIndex = null;
            this.appLauncherList.Core = null;
            this.appLauncherList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appLauncherList.Location = new System.Drawing.Point(0, 29);
            this.appLauncherList.Name = "appLauncherList";
            this.appLauncherList.Size = new System.Drawing.Size(564, 310);
            this.appLauncherList.TabIndex = 1;
            // 
            // tsslSpacer
            // 
            this.tsslSpacer.Name = "tsslSpacer";
            this.tsslSpacer.Size = new System.Drawing.Size(160, 19);
            this.tsslSpacer.Spring = true;
            // 
            // tsslVersionLabel
            // 
            this.tsslVersionLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tsslVersionLabel.Margin = new System.Windows.Forms.Padding(2, 3, 0, 2);
            this.tsslVersionLabel.Name = "tsslVersionLabel";
            this.tsslVersionLabel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.tsslVersionLabel.Size = new System.Drawing.Size(56, 19);
            this.tsslVersionLabel.Text = "Version:";
            // 
            // tsslVersion
            // 
            this.tsslVersion.Name = "tsslVersion";
            this.tsslVersion.Size = new System.Drawing.Size(31, 19);
            this.tsslVersion.Text = "0.0.0";
            // 
            // tsslVersionStatus
            // 
            this.tsslVersionStatus.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.ok_16;
            this.tsslVersionStatus.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.tsslVersionStatus.Name = "tsslVersionStatus";
            this.tsslVersionStatus.Size = new System.Drawing.Size(16, 19);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 361);
            this.Controls.Add(this.appLauncherList);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(260, 128);
            this.Name = "MainForm";
            this.Text = "Bench";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownDownHandler);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AppLauncherControl appLauncherList;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslRootPath;
        private System.Windows.Forms.ToolStripStatusLabel tsslAppCount;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnShellCmd;
        private System.Windows.Forms.Button btnShellPowerShell;
        private System.Windows.Forms.Button btnShellBash;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnDocs;
        private System.Windows.Forms.Button btnAutoSetup;
        private System.Windows.Forms.ToolStripStatusLabel tsslSpacer;
        private System.Windows.Forms.ToolStripStatusLabel tsslVersionLabel;
        private System.Windows.Forms.ToolStripStatusLabel tsslVersion;
        private System.Windows.Forms.ToolStripStatusLabel tsslVersionStatus;
    }
}

