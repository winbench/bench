namespace Mastersign.Bench.Dashboard
{
    partial class TaskInfoForm
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
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblContext = new System.Windows.Forms.Label();
            this.lblOutputLabel = new System.Windows.Forms.Label();
            this.lblDetailsLabel = new System.Windows.Forms.Label();
            this.lblMessageLabel = new System.Windows.Forms.Label();
            this.lblContextLabel = new System.Windows.Forms.Label();
            this.lblTimestampLabel = new System.Windows.Forms.Label();
            this.lblTimestamp = new System.Windows.Forms.Label();
            this.tableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Controls.Add(this.txtOutput, 1, 4);
            this.tableLayout.Controls.Add(this.lblDetails, 1, 3);
            this.tableLayout.Controls.Add(this.lblMessage, 1, 2);
            this.tableLayout.Controls.Add(this.lblContext, 1, 1);
            this.tableLayout.Controls.Add(this.lblOutputLabel, 0, 4);
            this.tableLayout.Controls.Add(this.lblDetailsLabel, 0, 3);
            this.tableLayout.Controls.Add(this.lblMessageLabel, 0, 2);
            this.tableLayout.Controls.Add(this.lblContextLabel, 0, 1);
            this.tableLayout.Controls.Add(this.lblTimestampLabel, 0, 0);
            this.tableLayout.Controls.Add(this.lblTimestamp, 1, 0);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(0, 0);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 5;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout.Size = new System.Drawing.Size(628, 304);
            this.tableLayout.TabIndex = 0;
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.SystemColors.Window;
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(80, 116);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(3, 6, 10, 10);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(538, 178);
            this.txtOutput.TabIndex = 11;
            this.txtOutput.Text = "Console Output";
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetails.Location = new System.Drawing.Point(80, 85);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Padding = new System.Windows.Forms.Padding(0, 6, 10, 6);
            this.lblDetails.Size = new System.Drawing.Size(545, 25);
            this.lblDetails.TabIndex = 9;
            this.lblDetails.Text = "More infos";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(80, 60);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(0, 6, 10, 6);
            this.lblMessage.Size = new System.Drawing.Size(545, 25);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "Description of the action";
            // 
            // lblContext
            // 
            this.lblContext.AutoSize = true;
            this.lblContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContext.Location = new System.Drawing.Point(80, 35);
            this.lblContext.Name = "lblContext";
            this.lblContext.Padding = new System.Windows.Forms.Padding(0, 6, 10, 6);
            this.lblContext.Size = new System.Drawing.Size(545, 25);
            this.lblContext.TabIndex = 7;
            this.lblContext.Text = "global";
            // 
            // lblOutputLabel
            // 
            this.lblOutputLabel.AutoSize = true;
            this.lblOutputLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutputLabel.Location = new System.Drawing.Point(3, 110);
            this.lblOutputLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblOutputLabel.Name = "lblOutputLabel";
            this.lblOutputLabel.Padding = new System.Windows.Forms.Padding(10, 6, 0, 6);
            this.lblOutputLabel.Size = new System.Drawing.Size(71, 184);
            this.lblOutputLabel.TabIndex = 5;
            this.lblOutputLabel.Text = "Output:";
            this.lblOutputLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDetailsLabel
            // 
            this.lblDetailsLabel.AutoSize = true;
            this.lblDetailsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailsLabel.Location = new System.Drawing.Point(3, 85);
            this.lblDetailsLabel.Name = "lblDetailsLabel";
            this.lblDetailsLabel.Padding = new System.Windows.Forms.Padding(10, 6, 0, 6);
            this.lblDetailsLabel.Size = new System.Drawing.Size(71, 25);
            this.lblDetailsLabel.TabIndex = 3;
            this.lblDetailsLabel.Text = "Details:";
            this.lblDetailsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMessageLabel
            // 
            this.lblMessageLabel.AutoSize = true;
            this.lblMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessageLabel.Location = new System.Drawing.Point(3, 60);
            this.lblMessageLabel.Name = "lblMessageLabel";
            this.lblMessageLabel.Padding = new System.Windows.Forms.Padding(10, 6, 0, 6);
            this.lblMessageLabel.Size = new System.Drawing.Size(71, 25);
            this.lblMessageLabel.TabIndex = 2;
            this.lblMessageLabel.Text = "Message:";
            this.lblMessageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblContextLabel
            // 
            this.lblContextLabel.AutoSize = true;
            this.lblContextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContextLabel.Location = new System.Drawing.Point(3, 35);
            this.lblContextLabel.Name = "lblContextLabel";
            this.lblContextLabel.Padding = new System.Windows.Forms.Padding(10, 6, 0, 6);
            this.lblContextLabel.Size = new System.Drawing.Size(71, 25);
            this.lblContextLabel.TabIndex = 1;
            this.lblContextLabel.Text = "Context:";
            this.lblContextLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTimestampLabel
            // 
            this.lblTimestampLabel.AutoSize = true;
            this.lblTimestampLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimestampLabel.Location = new System.Drawing.Point(3, 10);
            this.lblTimestampLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblTimestampLabel.Name = "lblTimestampLabel";
            this.lblTimestampLabel.Padding = new System.Windows.Forms.Padding(10, 6, 0, 6);
            this.lblTimestampLabel.Size = new System.Drawing.Size(71, 25);
            this.lblTimestampLabel.TabIndex = 0;
            this.lblTimestampLabel.Text = "Timestamp:";
            this.lblTimestampLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimestamp.Location = new System.Drawing.Point(80, 10);
            this.lblTimestamp.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Padding = new System.Windows.Forms.Padding(0, 6, 10, 6);
            this.lblTimestamp.Size = new System.Drawing.Size(545, 25);
            this.lblTimestamp.TabIndex = 6;
            this.lblTimestamp.Text = "0000-00-00 00:00:00";
            // 
            // TaskInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 304);
            this.Controls.Add(this.tableLayout);
            this.MaximizeBox = false;
            this.Name = "TaskInfoForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Details";
            this.tableLayout.ResumeLayout(false);
            this.tableLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private System.Windows.Forms.Label lblMessageLabel;
        private System.Windows.Forms.Label lblContextLabel;
        private System.Windows.Forms.Label lblTimestampLabel;
        private System.Windows.Forms.Label lblDetailsLabel;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblContext;
        private System.Windows.Forms.Label lblOutputLabel;
        private System.Windows.Forms.Label lblTimestamp;
    }
}