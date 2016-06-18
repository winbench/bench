using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.UI
{
    public partial class ProxyStepControl : WizzardStepControl
    {
        public ProxyStepControl()
        {
            Description = "Setup HTTP(S) proxy...";
            InitializeComponent();
            UpdateControls();
        }

        public bool UseProxy
        {
            get { return chkUseProxy.Checked; }
            set { chkUseProxy.Checked = value; }
        }

        public string HttpProxy
        {
            get { return txtHttpProxy.Text; }
            set { txtHttpProxy.Text = value; }
        }

        public string HttpsProxy
        {
            get { return txtHttpsProxy.Text; }
            set { txtHttpsProxy.Text = value; }
        }

        public string[] ProxyBypass
        {
            get
            {
                var list = new List<string>();
                var rawList = txtProxyBypass.Text.Split(
                    new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var e in rawList)
                {
                    list.Add(e.Trim());
                }
                return list.ToArray();
            }
            set { txtProxyBypass.Text = string.Join(", ", value ?? new string[0]); }
        }

        private void chkUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            var useProxy = chkUseProxy.Checked;
            lblHttpProxy.Enabled = useProxy;
            txtHttpProxy.Enabled = useProxy;
            lblHttpsProxy.Enabled = useProxy;
            txtHttpsProxy.Enabled = useProxy;
            lblProxyBypass.Enabled = useProxy;
            txtProxyBypass.Enabled = useProxy;
        }
    }
}
