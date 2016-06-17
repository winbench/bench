using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class DownloadEventArgs : EventArgs
    {
        public DownloadTask Task { get; private set; }

        public DownloadEventArgs(DownloadTask task)
        {
            Task = task;
        }
    }

    public class DownloadProgressEventArgs : DownloadEventArgs
    {
        public long LoadedBytes { get; private set; }

        public long TotalBytes { get; private set; }

        public int Percentage { get; private set; }

        public DownloadProgressEventArgs(DownloadTask task, long loadedBytes, long totalBytes, int percentage)
            : base(task)
        {
            LoadedBytes = loadedBytes;
            TotalBytes = totalBytes;
            Percentage = percentage;
        }
    }
}
