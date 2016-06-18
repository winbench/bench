using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    public partial class DownloadControl : UserControl
    {
        public DownloadControl()
        {
            InitializeComponent();
        }

        public string FileName
        {
            get { return lblFileName.Text; }
            set { lblFileName.Text = value; }
        }

        private long loadedBytes = 0;
        public long LoadedBytes
        {
            get { return loadedBytes; }
            set
            {
                loadedBytes = value;
                lblReceived.Text = string.Format("{0} KB", loadedBytes / 1024);
            }
        }

        public int Percentage
        {
            get { return progressBar.Value; }
            set
            {
                progressBar.Value = value;
                progressBar.Style = progressBar.Value > 0
                    ? ProgressBarStyle.Continuous
                    : ProgressBarStyle.Marquee;
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                lblError.Text = errorMessage;
                progressBar.Visible = errorMessage == null;
                lblError.Visible = errorMessage != null;
            }
        }
    }
}
