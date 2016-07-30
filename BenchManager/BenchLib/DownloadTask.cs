using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class represents a HTTP(S) resource to be downloaded by a <see cref="Downloader"/>.
    /// </summary>
    public class DownloadTask
    {
        /// <summary>
        /// A string identifying the download uniquely amont all other downloads.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The URL of the HTTP(S) resource.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// Additional HTTP header fields to send to the server when requesting the HTTP(S) resource.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Cookies to send to the server when requesting the HTTP(S) resource.
        /// </summary>
        public IDictionary<string, string> Cookies { get; set; }

        /// <summary>
        /// A path to the file, to store the data of the downloaded resource in.
        /// </summary>
        public string TargetFile { get; private set; }

        /// <summary>
        /// The resolve URL of the HTTP(S) resource.
        /// This property is set by the <see cref="Downloader"/> when resolving the URL.
        /// </summary>
        public Uri ResolvedUrl { get; set; }

        /// <summary>
        /// A value indicating, whether the resolution of the URL failed, or not.
        /// </summary>
        public bool UrlResolutionFailed { get; set; }

        /// <summary>
        /// The number of downloaded bytes.
        /// </summary>
        public long DownloadedBytes { get; set; }

        /// <summary>
        /// A value indicating, whether this download has ended (successfully, or with failure), or not.
        /// </summary>
        public bool HasEnded { get; set; }

        /// <summary>
        /// A value indicating, whether this download was completed successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The number of failed download attempts.
        /// </summary>
        public int FailedAttempts { get; set; }

        /// <summary>
        /// A string describing the last error, occured during the download, or <c>null</c> if no error occurred.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DownloadTask"/>.
        /// </summary>
        /// <param name="id">The unique ID of this download task.</param>
        /// <param name="url">The URL describing the HTTP(S) rsource.</param>
        /// <param name="targetFile">A path to the target file.</param>
        public DownloadTask(string id, Uri url, string targetFile)
        {
            Id = id;
            Url = url;
            TargetFile = targetFile;
        }
    }
}
