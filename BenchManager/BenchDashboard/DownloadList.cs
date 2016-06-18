using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Mastersign.Bench.Dashboard
{
    public partial class DownloadList : UserControl
    {
        public DownloadList()
        {
            InitializeComponent();
        }

        private readonly Dictionary<DownloadTask, DownloadControl> downloadControls
            = new Dictionary<DownloadTask, DownloadControl>();

        private Downloader downloader;

        public Downloader Downloader
        {
            get { return downloader; }
            set
            {
                if (downloader != null)
                {
                    downloader.DownloadStarted -= DownloadStartedHandler;
                    downloader.DownloadProgress -= DownloadProgressHandler;
                    downloader.DownloadEnded -= DownloadEndedHandler;
                    downloadControls.Clear();
                    Controls.Clear();
                }
                downloader = value;
                if (downloader != null)
                {
                    downloader.DownloadStarted += DownloadStartedHandler;
                    downloader.DownloadProgress += DownloadProgressHandler;
                    downloader.DownloadEnded += DownloadEndedHandler;
                }
            }
        }

        private void DownloadStartedHandler(object sender, DownloadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EventHandler<DownloadEventArgs>)DownloadStartedHandler, sender, e);
                return;
            }
            var control = new DownloadControl();
            control.Visible = false;
            control.Left = ClientRectangle.Left;
            control.Width = ClientSize.Width;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            control.FileName = Path.GetFileName(e.Task.TargetFile);
            downloadControls.Add(e.Task, control);
            Controls.Add(control);
            UpdateLayout();
        }

        private void DownloadProgressHandler(object sender, DownloadProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EventHandler<DownloadProgressEventArgs>)DownloadProgressHandler, sender, e);
                return;
            }
            var t = e.Task;
            DownloadControl c;
            if (downloadControls.TryGetValue(t, out c))
            {
                c.LoadedBytes = e.LoadedBytes;
                c.Percentage = e.Percentage;
            }
        }

        private void DownloadEndedHandler(object sender, DownloadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EventHandler<DownloadEventArgs>)DownloadEndedHandler, sender, e);
                return;
            }
            if (e.Task.Success)
            {
                Controls.Remove(downloadControls[e.Task]);
                downloadControls.Remove(e.Task);
                UpdateLayout();
            }
            else
            {
                downloadControls[e.Task].ErrorMessage = e.Task.ErrorMessage;
            }
        }

        private void UpdateLayout()
        {
            SuspendLayout();
            var p = ClientRectangle.Top;
            foreach (Control c in Controls)
            {
                c.Top = p;
                c.Visible = true;
                p += c.Height;
            }
            ResumeLayout();
        }
    }
}