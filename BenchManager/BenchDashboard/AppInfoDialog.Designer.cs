namespace Mastersign.Bench.Dashboard
{
    partial class AppInfoDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppInfoDialog));
            this.lblAppId = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabResolved = new System.Windows.Forms.TabPage();
            this.gridResolved = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabRaw = new System.Windows.Forms.TabPage();
            this.gridRaw = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl.SuspendLayout();
            this.tabResolved.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResolved)).BeginInit();
            this.tabRaw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRaw)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAppId
            // 
            this.lblAppId.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAppId.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppId.Location = new System.Drawing.Point(0, 0);
            this.lblAppId.Name = "lblAppId";
            this.lblAppId.Padding = new System.Windows.Forms.Padding(4);
            this.lblAppId.Size = new System.Drawing.Size(433, 48);
            this.lblAppId.TabIndex = 1;
            this.lblAppId.Text = "<ID>";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabResolved);
            this.tabControl.Controls.Add(this.tabRaw);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 48);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(433, 465);
            this.tabControl.TabIndex = 2;
            // 
            // tabResolved
            // 
            this.tabResolved.Controls.Add(this.gridResolved);
            this.tabResolved.Location = new System.Drawing.Point(4, 22);
            this.tabResolved.Name = "tabResolved";
            this.tabResolved.Padding = new System.Windows.Forms.Padding(3);
            this.tabResolved.Size = new System.Drawing.Size(425, 439);
            this.tabResolved.TabIndex = 0;
            this.tabResolved.Text = "Effective";
            this.tabResolved.UseVisualStyleBackColor = true;
            // 
            // gridResolved
            // 
            this.gridResolved.AllowUserToAddRows = false;
            this.gridResolved.AllowUserToDeleteRows = false;
            this.gridResolved.AllowUserToResizeRows = false;
            this.gridResolved.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.gridResolved.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridResolved.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResolved.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colValue});
            this.gridResolved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResolved.Location = new System.Drawing.Point(3, 3);
            this.gridResolved.Name = "gridResolved";
            this.gridResolved.ReadOnly = true;
            this.gridResolved.RowHeadersVisible = false;
            this.gridResolved.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridResolved.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResolved.Size = new System.Drawing.Size(419, 433);
            this.gridResolved.TabIndex = 1;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colName.Frozen = true;
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 61;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // tabRaw
            // 
            this.tabRaw.Controls.Add(this.gridRaw);
            this.tabRaw.Location = new System.Drawing.Point(4, 22);
            this.tabRaw.Name = "tabRaw";
            this.tabRaw.Padding = new System.Windows.Forms.Padding(3);
            this.tabRaw.Size = new System.Drawing.Size(425, 439);
            this.tabRaw.TabIndex = 1;
            this.tabRaw.Text = "Raw";
            this.tabRaw.UseVisualStyleBackColor = true;
            // 
            // gridRaw
            // 
            this.gridRaw.AllowUserToAddRows = false;
            this.gridRaw.AllowUserToDeleteRows = false;
            this.gridRaw.AllowUserToResizeRows = false;
            this.gridRaw.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.gridRaw.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridRaw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRaw.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.gridRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRaw.Location = new System.Drawing.Point(3, 3);
            this.gridRaw.Name = "gridRaw";
            this.gridRaw.ReadOnly = true;
            this.gridRaw.RowHeadersVisible = false;
            this.gridRaw.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridRaw.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRaw.Size = new System.Drawing.Size(419, 433);
            this.gridRaw.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 61;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // AppInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 513);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.lblAppId);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AppInfoDialog";
            this.Text = "App Info";
            this.tabControl.ResumeLayout(false);
            this.tabResolved.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridResolved)).EndInit();
            this.tabRaw.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRaw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblAppId;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabResolved;
        private System.Windows.Forms.DataGridView gridResolved;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.TabPage tabRaw;
        private System.Windows.Forms.DataGridView gridRaw;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}