using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class defines the arguments of a download related event.
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        /// <summary>
        /// The download task, the event is related to.
        /// </summary>
        public DownloadTask Task { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DownloadEventArgs"/>.
        /// </summary>
        /// <param name="task">The download task, affected by this event.</param>
        public DownloadEventArgs(DownloadTask task)
        {
            Task = task;
        }
    }

    /// <summary>
    /// This class defines the arguments of the <see cref="Downloader.DownloadProgress"/> event.
    /// </summary>
    public class DownloadProgressEventArgs : DownloadEventArgs
    {
        /// <summary>
        /// The number of bytes already downloaded.
        /// </summary>
        public long LoadedBytes { get; private set; }

        /// <summary>
        /// The total number of bytes this HTTP(S) resource encompasses.
        /// </summary>
        public long TotalBytes { get; private set; }

        /// <summary>
        /// The percentage of the progress.
        /// </summary>
        public int Percentage { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DownloadProgressEventArgs"/>.
        /// </summary>
        /// <param name="task">The affected download task.</param>
        /// <param name="loadedBytes">The number of already downloaded bytes.</param>
        /// <param name="totalBytes">The total number of bytes, this HTTP(S) resource encompasses.</param>
        /// <param name="percentage">The percentage of the progress.</param>
        public DownloadProgressEventArgs(DownloadTask task, long loadedBytes, long totalBytes, int percentage)
            : base(task)
        {
            LoadedBytes = loadedBytes;
            TotalBytes = totalBytes;
            Percentage = percentage;
        }
    }
}
