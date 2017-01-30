using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Mastersign.Bench.Dashboard
{
    public partial class DownloadList : UserControl
    {
        private readonly Dictionary<DownloadTask, DownloadControl> downloadControls
            = new Dictionary<DownloadTask, DownloadControl>();

        public DownloadList()
        {
            InitializeComponent();
            Disposed += DisposedHandler;
        }

        private void DisposedHandler(object sender, EventArgs e)
        {
            Downloader = null;
        }

        private Downloader downloader;

        public Downloader Downloader
        {
            get { return downloader; }
            set
            {
                if (downloader != null)
                {
                    UnbindDownloader();
                }
                downloader = value;
                if (downloader != null)
                {
                    BindDownloader();
                }
            }
        }

        private void BindDownloader()
        {
            downloader.IsWorkingChanged += DownloaderIsWorkingChangedHandler;
            downloader.DownloadStarted += DownloadStartedHandler;
            downloader.DownloadProgress += DownloadProgressHandler;
            downloader.DownloadEnded += DownloadEndedHandler;
            ClearDownloadTasks();
            // Potential inconsitency ... add already running download tasks
        }

        private void UnbindDownloader()
        {
            downloader.IsWorkingChanged -= DownloaderIsWorkingChangedHandler;
            downloader.DownloadStarted -= DownloadStartedHandler;
            downloader.DownloadProgress -= DownloadProgressHandler;
            downloader.DownloadEnded -= DownloadEndedHandler;
        }

        private void DownloaderIsWorkingChangedHandler(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EventHandler)DownloaderIsWorkingChangedHandler, sender, e);
                return;
            }
            if (downloader.IsWorking)
            {
                ClearDownloadTasks();
            }
        }

        private void DownloadStartedHandler(object sender, DownloadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EventHandler<DownloadEventArgs>)DownloadStartedHandler, sender, e);
                return;
            }
            AddDownloadTask(e.Task);
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
                RemoveDownloadTask(e.Task);
            }
            else
            {
                var t = e.Task;
                DownloadControl c;
                if (downloadControls.TryGetValue(t, out c))
                {
                    c.ErrorMessage = t.ErrorMessage;
                }
            }
        }

        private void AddDownloadTask(DownloadTask t)
        {
            var control = new DownloadControl();
            control.Visible = false;
            control.Left = ClientRectangle.Left;
            control.Width = ClientSize.Width;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            control.FileName = Path.GetFileName(t.TargetFile);
            downloadControls.Add(t, control);
            Controls.Add(control);
            UpdateLayout();
        }

        private void RemoveDownloadTask(DownloadTask t)
        {
            Controls.Remove(downloadControls[t]);
            downloadControls.Remove(t);
            UpdateLayout();
        }

        private void ClearDownloadTasks()
        {
            downloadControls.Clear();
            Controls.Clear();
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