﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class implements a download manager for downloading multiple HTTP(S) resources
    /// in parallel and monitoring their progress.
    /// </summary>
    public class Downloader : IDisposable
    {
        private readonly int ParallelDownloads;

        private readonly object queueLock = new object();

        private readonly Queue<DownloadTask> queue = new Queue<DownloadTask>();

        private readonly WebClient[] webClients;
        private readonly AutoResetEvent[] downloadEvents;
        private volatile int runningDownloads = 0;
        private volatile bool working = false;

        private readonly Semaphore availableTasks;

        /// <summary>
        /// This event is fired, when a download started.
        /// </summary>
        public event EventHandler<DownloadEventArgs> DownloadStarted;

        /// <summary>
        /// This event is fired, when the progress of a running download was updated.
        /// </summary>
        public event EventHandler<DownloadProgressEventArgs> DownloadProgress;

        /// <summary>
        /// This event is fired, when a download ended.
        /// This can mean success or failure.
        /// </summary>
        public event EventHandler<DownloadEventArgs> DownloadEnded;

        /// <summary>
        /// This event is fired, when all queued download tasks have ended.
        /// </summary>
        public event EventHandler WorkFinished;

        /// <summary>
        /// This event is fired, when the downloader starts with the first download after
        /// doing nothing, or if the downloader finished the last queued download task.
        /// </summary>
        public event EventHandler IsWorkingChanged;

        /// <summary>
        /// Gets or sets the proxy configuration for HTTP connections.
        /// </summary>
        public WebProxy HttpProxy { get; set; }

        /// <summary>
        /// Gets or sets the proxy configuration for HTTPS connections.
        /// </summary>
        public WebProxy HttpsProxy { get; set; }

        /// <summary>
        /// Gets or sets the number of attempts per download task.
        /// </summary>
        public int DownloadAttempts { get; set; }

        /// <summary>
        /// Gets a collection with <see cref="IUrlResolver"/> objects.
        /// This collection can be changed by the user to control how URLs are resolved before
        /// the actual download of a resource starts.
        /// </summary>
        internal ICollection<IUrlResolver> UrlResolver { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Downloader"/> with a default configuration,
        /// allowing only sequential downloads.
        /// </summary>
        public Downloader()
            : this(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Downloader"/> with a configuration,
        /// allowing the given number of parallel downloads.
        /// </summary>
        /// <param name="parallelDownloads">The maximum number of parallel downloads.
        /// This value must be in the interval of <c>1</c> and <c>9999</c>.</param>
        public Downloader(int parallelDownloads)
        {
            if (parallelDownloads < 1 || parallelDownloads > 9999)
            {
                throw new ArgumentOutOfRangeException("parallelDownloads",
                    "The number of parallel downloads must be at least 1 and less than 10000.");
            }
            Debug.WriteLine("Initializing downloader for " + parallelDownloads + " parallel downloads.");
            UrlResolver = new List<IUrlResolver>();
            ParallelDownloads = parallelDownloads;
            webClients = new WebClient[parallelDownloads];
            downloadEvents = new AutoResetEvent[parallelDownloads];
            for (int i = 0; i < parallelDownloads; i++)
            {
                downloadEvents[i] = new AutoResetEvent(false);
            }
            availableTasks = new Semaphore(0, int.MaxValue);

            Debug.WriteLine("Starting worker threads...");
            for (int i = 0; i < parallelDownloads; i++)
            {
                var no = i;
                var t = new Thread(() => Worker(no));
                t.Name = string.Format("DownloadWorker_{0:00}", no);
                t.Priority = ThreadPriority.BelowNormal;
                t.Start();
            }
            DownloadAttempts = 1;
        }

        private void OnDownloadStarted(DownloadTask task)
        {
            Debug.WriteLine("Raising event DownloadStarted for " + task.Id + ", Url=" + task.Url);
            var handler = DownloadStarted;
            if (handler != null)
            {
                handler(this, new DownloadEventArgs(task));
            }
        }

        private void OnDownloadProgress(DownloadTask task, long bytesDownloaded, long totalBytes, int percentage)
        {
            var handler = DownloadProgress;
            if (handler != null)
            {
                handler(this, new DownloadProgressEventArgs(task, bytesDownloaded, totalBytes, percentage));
            }
        }

        private void OnDownloadEnded(DownloadTask task)
        {
            Debug.WriteLine("Raising event DownloadEnded for " + task.Id + ", Error=" + task.ErrorMessage ?? "None");
            var handler = DownloadEnded;
            if (handler != null)
            {
                handler(this, new DownloadEventArgs(task));
            }
        }

        private void OnIsWorkingChanged()
        {
            Debug.WriteLine("Raising event IsWorkingChanged");
            var handler = IsWorkingChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnWorkFinished()
        {
            Debug.WriteLine("Raising event WorkFinished");
            var handler = WorkFinished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Checks, whether the downloader has queued download tasks, or if it currently does nothing.
        /// </summary>
        /// <value><c>true</c> if at least download if in progress or queued;
        /// <c>false</c> if the downloader does nothing.</value>
        public bool IsWorking { get { return working; } }

        /// <summary>
        /// Enqueues a new download task.
        /// </summary>
        /// <param name="task">The download task.</param>
        public void Enqueue(DownloadTask task)
        {
            Debug.WriteLine("Queuing " + task.Id + ", TargetFile=" + task.TargetFile + ", URL=" + task.Url);
            var notify = false;
            lock (queueLock)
            {
                if (IsDisposed) { throw new ObjectDisposedException(GetType().Name); }
                queue.Enqueue(task);
                if (!working)
                {
                    working = true;
                    notify = true;
                }
            }
            availableTasks.Release();
            if (notify)
            {
                OnIsWorkingChanged();
            }
        }

        private void Worker(int no)
        {
            Debug.WriteLine("Starting worker " + no + "...");
            DownloadTask task = null;
            var wc = webClients[no] = new WebClient
            {
                Proxy = new SchemeDispatchProxy(new Dictionary<string, IWebProxy>
                    {
                        {"http", HttpProxy},
                        {"https", HttpsProxy}
                    })
            };
            wc.DownloadProgressChanged += (o, e) =>
            {
                task.DownloadedBytes = e.BytesReceived;
                OnDownloadProgress(task, e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
            };
            wc.DownloadFileCompleted += (o, e) =>
            {
                if (e.Cancelled == false && e.Error == null)
                {
                    task.ErrorMessage = null;
                    task.Success = true;
                    task.HasEnded = true;
                    OnDownloadEnded(task);
                }
                else
                {
                    if (File.Exists(task.TargetFile))
                    {
                        try
                        {
                            File.Delete(task.TargetFile);
                        }
                        catch (IOException ioe)
                        {
                            Debug.WriteLine("Failed to delete file after failed download: " + ioe.Message);
                        }
                    }
                    if (e.Cancelled || IsDisposed)
                    {
                        task.ErrorMessage = "Canceled";
                        task.Success = false;
                        task.HasEnded = true;
                        OnDownloadEnded(task);
                    }
                    else if (e.Error != null)
                    {
                        task.ErrorMessage = FormatErrorMessage(e);
                        task.Success = false;
                        task.FailedAttempts = task.FailedAttempts + 1;
                        if (task.FailedAttempts >= DownloadAttempts)
                        {
                            task.HasEnded = true;
                            OnDownloadEnded(task);
                        }
                        else
                        {
                            Debug.WriteLine("Queuing " + task.Id + " for retry.");
                            Enqueue(task);
                        }
                    }
                }
                runningDownloads--;
                downloadEvents[no].Set();
            };

            while (true)
            {
                CheckWorkState();

                Debug.WriteLine("Worker " + no + " waiting for task...");
                availableTasks.WaitOne();

                // Dispose synchronization resources if canceled
                if (IsDisposed)
                {
                    Debug.WriteLine("Worker " + no + " ending.");
                    downloadEvents[no].Close();
                    downloadEvents[no] = null;
                    break;
                }

                // Aquire next available task
                lock (queueLock)
                {
                    task = queue.Dequeue();
                }
                runningDownloads++;
                Debug.WriteLine("Worker " + no + " beginning with " + task.Id);

                // Prepare web client
                wc.Headers.Clear();
                if (task.Headers != null)
                {
                    foreach (var k in task.Headers.Keys)
                    {
                        wc.Headers.Add(k, task.Headers[k]);
                    }
                }
                if (task.Cookies != null && task.Cookies.Count > 0)
                {
                    List<string> cookieStrings = new List<string>();
                    foreach (var k in task.Cookies.Keys)
                    {
                        cookieStrings.Add(string.Format("{0}={1}", k, task.Cookies[k]));
                    }
                    wc.Headers.Add(HttpRequestHeader.Cookie, string.Join(";", cookieStrings.ToArray()));
                }

                // Notify listener
                if (task.FailedAttempts == 0)
                {
                    OnDownloadStarted(task);
                }

                // Resolve Url
                ResolveUrl(task, wc);

                if (task.UrlResolutionFailed)
                {
                    task.ErrorMessage = "Resolution of the URL failed.";
                    task.Success = false;
                    task.HasEnded = true;
                    OnDownloadEnded(task);
                    downloadEvents[no].Set();
                }
                else
                {
                    // Start download
                    wc.DownloadFileAsync(task.ResolvedUrl ?? task.Url, task.TargetFile);
                }

                // Wait for end
                downloadEvents[no].WaitOne();
            }
        }

        private void CheckWorkState()
        {
            var notify = false;
            lock (queueLock)
            {
                Debug.WriteLine("Checking work state: " + working + ", " + queue.Count);
                if (!working || runningDownloads > 0 || queue.Count > 0)
                {
                    return;
                }
                working = false;
                notify = true;
            }
            if (notify)
            {
                OnWorkFinished();
                OnIsWorkingChanged();
            }
        }

        private void ResolveUrl(DownloadTask task, WebClient wc)
        {
            bool foundResolver;
            var resolvedUrl = ResolveUrl(task.Url, out foundResolver, wc);
            if (foundResolver)
            {
                if (resolvedUrl != null)
                {
                    task.ResolvedUrl = resolvedUrl;
                }
                else
                {
                    task.UrlResolutionFailed = true;
                }
            }
        }

        private Uri ResolveUrl(Uri url, out bool match, WebClient wc, List<Uri> visited = null)
        {
            IUrlResolver resolver = null;
            match = false;
            foreach (var r in UrlResolver)
            {
                if (r.Matches(url))
                {
                    resolver = r;
                    match = true;
                    break;
                }
            }
            if (resolver == null) return url;
            Debug.WriteLine("Found resolver for: " + url);
            if (visited != null && visited.Contains(url))
            {
                Debug.WriteLine("Loop detected. Canceling URL resolution.");
                return null;
            }

            var url2 = resolver.Resolve(url, wc);
            Debug.WriteLine("URL resolved to: " + url2);
            if (url2 == null) return null;
            bool subMatched;
            if (visited == null)
            {
                visited = new List<Uri>();
            }
            visited.Add(url);
            return ResolveUrl(url2, out subMatched, wc, visited);
        }

        private static string FormatErrorMessage(System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var errorMessage = e.Error.Message;
            if (e.Error.InnerException != null)
            {
                if (e.Error is WebException)
                {
                    errorMessage = e.Error.InnerException.Message;
                }
                else
                {
                    errorMessage += " " + e.Error.InnerException.Message;
                }
            }
            return errorMessage;
        }

        /// <summary>
        /// Request the downloader to cancel all pending download tasks and to clear the queue.
        /// </summary>
        public void CancelAll()
        {
            Debug.WriteLine("Canceling current downloads...");

            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                t.Success = false;
                t.HasEnded = true;
                t.ErrorMessage = "Canceled";
                OnDownloadEnded(t);
            }
            for (int i = 0; i < ParallelDownloads; i++)
            {
                webClients[i].CancelAsync();
            }
        }

        /// <summary>
        /// Checks, whether this instance was disposed, or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes the downloader by cancelling all pending download tasks and freeing all ocupied resource.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

            Debug.WriteLine("Disposing downloader...");

            queue.Clear();

            for (int i = 0; i < ParallelDownloads; i++)
            {
                webClients[i].CancelAsync();
            }

            // allow worker to end
            availableTasks.Release(ParallelDownloads);
        }
    }
}
