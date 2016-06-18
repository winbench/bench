using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mastersign.Bench.Dashboard.Properties;

namespace Mastersign.Bench.Dashboard
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            lblVersion.Text = "Version " + Program.Core.Config.GetStringValue(PropertyKeys.Version);
            txtLicenses.Text = Resources.licenses;
            var acks = new List<string>();
            foreach (var ack in Resources.acknowledgements.Split(
                new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                acks.Add(ack.Trim());
            }
            lblAcks.Text = string.Join(" / ", acks);
        }

        private void linkAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.mastersign.de/");
        }
    }
}
