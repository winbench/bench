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
        private readonly Core core;

        public AboutDialog(Core core)
        {
            this.core = core;
            InitializeComponent();
            lblVersion.Text = "Version " + Program.Core.Config.GetStringValue(ConfigPropertyKeys.Version);
            txtLicenses.Text = Resources.licenses;
            var acks = new List<string>();
            foreach (var ack in Resources.acknowledgements.Split(
                new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                acks.Add(ack.Trim());
            }
            lblAcks.Text = string.Join(" / ", acks);
            lblUpdate.Text = string.Empty;
            if (core.Config.GetBooleanValue(ConfigPropertyKeys.AutoUpdateCheck))
            {
                CheckForUpdate();
            }
        }

        private void linkAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.mastersign.de/");
        }

        private void VersionDoubleClickHandler(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private async void CheckForUpdate()
        {
            var config = core.Config;
            lblUpdate.Text = "Checking for update...";
            picVersionState.Image = Resources.progress_16_animation;
            var version = await core.GetLatestVersionNumber();
            if (IsDisposed) return;
            if (version != null)
            {
                var currentVersion = config.GetStringValue(ConfigPropertyKeys.Version);
                if (!string.Equals(currentVersion, version))
                {
                    lblUpdate.Text = "Update available: v" + version;
                    picVersionState.Image = Resources.info_16;
                }
                else
                {
                    lblUpdate.Text = string.Empty;
                    picVersionState.Image = Resources.ok_16;
                }
            }
            else
            {
                lblUpdate.Text = "Check for update failed.";
                picVersionState.Image = Resources.warning_16;
            }
        }
    }
}
