using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class DownloadTask
    {
        public string Id { get; private set; }

        public Uri Url { get; private set; }

        public IDictionary<string, string> Headers { get; set; }

        public IDictionary<string, string> Cookies { get; set; }

        public string TargetFile { get; private set; }

        public Uri ResolvedUrl { get; set; }

        public bool UrlResolutionFailed { get; set; }

        public long DownloadedBytes { get; set; }

        public bool HasEnded { get; set; }

        public bool Success { get; set; }

        public int FailedAttempts { get; set; }

        public string ErrorMessage { get; set; }

        public DownloadTask(string id, Uri url, string targetFile)
        {
            Id = id;
            Url = url;
            TargetFile = targetFile;
        }
    }
}
