namespace Mastersign.Bench.Dashboard
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.lblLicenses = new System.Windows.Forms.Label();
            this.lblAckLabel = new System.Windows.Forms.Label();
            this.panelHead = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblAcks = new System.Windows.Forms.Label();
            this.txtLicenses = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.linkAuthor = new System.Windows.Forms.LinkLabel();
            this.table.SuspendLayout();
            this.panelHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Controls.Add(this.lblLicenses, 0, 2);
            this.table.Controls.Add(this.lblAckLabel, 0, 1);
            this.table.Controls.Add(this.panelHead, 0, 0);
            this.table.Controls.Add(this.panelFooter, 0, 3);
            this.table.Controls.Add(this.lblAcks, 1, 1);
            this.table.Controls.Add(this.txtLicenses, 1, 2);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 4;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.table.Size = new System.Drawing.Size(624, 481);
            this.table.TabIndex = 0;
            // 
            // lblLicenses
            // 
            this.lblLicenses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLicenses.AutoSize = true;
            this.lblLicenses.Location = new System.Drawing.Point(65, 125);
            this.lblLicenses.Margin = new System.Windows.Forms.Padding(12, 8, 0, 8);
            this.lblLicenses.Name = "lblLicenses";
            this.lblLicenses.Size = new System.Drawing.Size(49, 13);
            this.lblLicenses.TabIndex = 4;
            this.lblLicenses.Text = "Licenses";
            // 
            // lblAckLabel
            // 
            this.lblAckLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAckLabel.AutoSize = true;
            this.lblAckLabel.Location = new System.Drawing.Point(12, 96);
            this.lblAckLabel.Margin = new System.Windows.Forms.Padding(12, 8, 0, 8);
            this.lblAckLabel.Name = "lblAckLabel";
            this.lblAckLabel.Size = new System.Drawing.Size(102, 13);
            this.lblAckLabel.TabIndex = 0;
            this.lblAckLabel.Text = "Acknowledgments";
            // 
            // panelHead
            // 
            this.panelHead.BackColor = System.Drawing.SystemColors.Control;
            this.table.SetColumnSpan(this.panelHead, 2);
            this.panelHead.Controls.Add(this.lblVersion);
            this.panelHead.Controls.Add(this.picLogo);
            this.panelHead.Controls.Add(this.lblSubtitle);
            this.panelHead.Controls.Add(this.lblTitle);
            this.panelHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHead.Location = new System.Drawing.Point(0, 0);
            this.panelHead.Margin = new System.Windows.Forms.Padding(0);
            this.panelHead.Name = "panelHead";
            this.panelHead.Size = new System.Drawing.Size(624, 88);
            this.panelHead.TabIndex = 1;
            // 
            // picLogo
            // 
            this.picLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picLogo.Image = global::Mastersign.Bench.Dashboard.Properties.Resources.logo_64;
            this.picLogo.Location = new System.Drawing.Point(548, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(64, 64);
            this.picLogo.TabIndex = 3;
            this.picLogo.TabStop = false;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.Location = new System.Drawing.Point(30, 52);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(427, 21);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Portable Environment for Software Development on Windows";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(93, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Bench";
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.SystemColors.Control;
            this.table.SetColumnSpan(this.panelFooter, 2);
            this.panelFooter.Controls.Add(this.linkAuthor);
            this.panelFooter.Controls.Add(this.btnClose);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 421);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(624, 60);
            this.panelFooter.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(528, 16);
            this.btnClose.Margin = new System.Windows.Forms.Padding(16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblAcks
            // 
            this.lblAcks.AutoSize = true;
            this.lblAcks.Location = new System.Drawing.Point(122, 96);
            this.lblAcks.Margin = new System.Windows.Forms.Padding(8);
            this.lblAcks.Name = "lblAcks";
            this.lblAcks.Size = new System.Drawing.Size(118, 13);
            this.lblAcks.TabIndex = 3;
            this.lblAcks.Text = "<Acknowledgments>";
            // 
            // txtLicenses
            // 
            this.txtLicenses.BackColor = System.Drawing.SystemColors.Window;
            this.txtLicenses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLicenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLicenses.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLicenses.Location = new System.Drawing.Point(122, 125);
            this.txtLicenses.Margin = new System.Windows.Forms.Padding(8, 8, 0, 8);
            this.txtLicenses.Multiline = true;
            this.txtLicenses.Name = "txtLicenses";
            this.txtLicenses.ReadOnly = true;
            this.txtLicenses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLicenses.Size = new System.Drawing.Size(502, 288);
            this.txtLicenses.TabIndex = 5;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.Location = new System.Drawing.Point(440, 16);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(100, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "v0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linkAuthor
            // 
            this.linkAuthor.AutoSize = true;
            this.linkAuthor.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkAuthor.Location = new System.Drawing.Point(12, 24);
            this.linkAuthor.Name = "linkAuthor";
            this.linkAuthor.Size = new System.Drawing.Size(93, 13);
            this.linkAuthor.TabIndex = 4;
            this.linkAuthor.TabStop = true;
            this.linkAuthor.Text = "Tobias Kiertscher";
            this.linkAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAuthor_LinkClicked);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(624, 481);
            this.Controls.Add(this.table);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.Text = "About Bench";
            this.table.ResumeLayout(false);
            this.table.PerformLayout();
            this.panelHead.ResumeLayout(false);
            this.panelHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.Label lblAckLabel;
        private System.Windows.Forms.Panel panelHead;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAcks;
        private System.Windows.Forms.Label lblLicenses;
        private System.Windows.Forms.TextBox txtLicenses;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel linkAuthor;
    }
}