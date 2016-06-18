namespace Mastersign.Bench.UI
{
    partial class ProxyStepControl
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
            System.Windows.Forms.Label lblExample1;
            System.Windows.Forms.Label lblExample2;
            this.lblHttpProxy = new System.Windows.Forms.Label();
            this.lblHttpsProxy = new System.Windows.Forms.Label();
            this.txtHttpProxy = new System.Windows.Forms.TextBox();
            this.txtHttpsProxy = new System.Windows.Forms.TextBox();
            this.lblProxyBypass = new System.Windows.Forms.Label();
            this.txtProxyBypass = new System.Windows.Forms.TextBox();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            lblExample1 = new System.Windows.Forms.Label();
            lblExample2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHttpProxy
            // 
            this.lblHttpProxy.AutoSize = true;
            this.lblHttpProxy.Location = new System.Drawing.Point(15, 58);
            this.lblHttpProxy.Name = "lblHttpProxy";
            this.lblHttpProxy.Size = new System.Drawing.Size(36, 13);
            this.lblHttpProxy.TabIndex = 2;
            this.lblHttpProxy.Text = "&HTTP";
            // 
            // lblHttpsProxy
            // 
            this.lblHttpsProxy.AutoSize = true;
            this.lblHttpsProxy.Location = new System.Drawing.Point(15, 84);
            this.lblHttpsProxy.Name = "lblHttpsProxy";
            this.lblHttpsProxy.Size = new System.Drawing.Size(43, 13);
            this.lblHttpsProxy.TabIndex = 4;
            this.lblHttpsProxy.Text = "HTTP&S";
            // 
            // lblExample1
            // 
            lblExample1.AutoSize = true;
            lblExample1.ForeColor = System.Drawing.SystemColors.GrayText;
            lblExample1.Location = new System.Drawing.Point(61, 39);
            lblExample1.Name = "lblExample1";
            lblExample1.Size = new System.Drawing.Size(139, 13);
            lblExample1.TabIndex = 1;
            lblExample1.Text = "e.g.: http://10.0.20.1:3128/";
            // 
            // txtHttpProxy
            // 
            this.txtHttpProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHttpProxy.Location = new System.Drawing.Point(64, 55);
            this.txtHttpProxy.Name = "txtHttpProxy";
            this.txtHttpProxy.Size = new System.Drawing.Size(385, 20);
            this.txtHttpProxy.TabIndex = 3;
            // 
            // txtHttpsProxy
            // 
            this.txtHttpsProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHttpsProxy.Location = new System.Drawing.Point(64, 81);
            this.txtHttpsProxy.Name = "txtHttpsProxy";
            this.txtHttpsProxy.Size = new System.Drawing.Size(385, 20);
            this.txtHttpsProxy.TabIndex = 5;
            // 
            // lblProxyBypass
            // 
            this.lblProxyBypass.AutoSize = true;
            this.lblProxyBypass.Location = new System.Drawing.Point(15, 127);
            this.lblProxyBypass.Name = "lblProxyBypass";
            this.lblProxyBypass.Size = new System.Drawing.Size(41, 13);
            this.lblProxyBypass.TabIndex = 7;
            this.lblProxyBypass.Text = "&Bypass";
            // 
            // txtProxyBypass
            // 
            this.txtProxyBypass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyBypass.Location = new System.Drawing.Point(64, 124);
            this.txtProxyBypass.Name = "txtProxyBypass";
            this.txtProxyBypass.Size = new System.Drawing.Size(385, 20);
            this.txtProxyBypass.TabIndex = 8;
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.AutoSize = true;
            this.chkUseProxy.Location = new System.Drawing.Point(64, 15);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(119, 17);
            this.chkUseProxy.TabIndex = 0;
            this.chkUseProxy.Text = "&Use HTTP(S) Proxy";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            this.chkUseProxy.CheckedChanged += new System.EventHandler(this.chkUseProxy_CheckedChanged);
            // 
            // lblExample2
            // 
            lblExample2.AutoSize = true;
            lblExample2.ForeColor = System.Drawing.SystemColors.GrayText;
            lblExample2.Location = new System.Drawing.Point(61, 108);
            lblExample2.Name = "lblExample2";
            lblExample2.Size = new System.Drawing.Size(158, 13);
            lblExample2.TabIndex = 6;
            lblExample2.Text = "e.g.: localhost, mycompany.com";
            // 
            // ProxyStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(lblExample2);
            this.Controls.Add(this.chkUseProxy);
            this.Controls.Add(this.txtProxyBypass);
            this.Controls.Add(this.lblProxyBypass);
            this.Controls.Add(lblExample1);
            this.Controls.Add(this.txtHttpsProxy);
            this.Controls.Add(this.txtHttpProxy);
            this.Controls.Add(this.lblHttpsProxy);
            this.Controls.Add(this.lblHttpProxy);
            this.Name = "ProxyStepControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHttpProxy;
        private System.Windows.Forms.TextBox txtHttpsProxy;
        private System.Windows.Forms.Label lblProxyBypass;
        private System.Windows.Forms.TextBox txtProxyBypass;
        private System.Windows.Forms.CheckBox chkUseProxy;
        private System.Windows.Forms.Label lblHttpProxy;
        private System.Windows.Forms.Label lblHttpsProxy;
    }
}
