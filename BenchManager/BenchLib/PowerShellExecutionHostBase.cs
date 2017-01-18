using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Mastersign.Bench.RemoteExecHost;

namespace Mastersign.Bench
{
    /// <summary>
    /// THe base class for implementations of <see cref="IProcessExecutionHost"/>,
    /// which uses the <c>PsExecHost.ps1</c> PowerShell script as a remote execution host.
    /// </summary>
    public abstract class PowerShellExecutionHostBase : IProcessExecutionHost
    {
        private const string HOST_SCRIPT_FILE = "PsExecHost.ps1";
        private const int RETRY_INTERVAL = 250;
        private const int START_UP_TIMEOUT = 10000;

        private readonly string benchRoot;

        private readonly string scriptsDirectory;

        private IProcessExecutionHost backupHost;

        private readonly Semaphore hostSemaphore = new Semaphore(1, 1);

        private bool calledSuccessfully = false;

        private bool reloadConfigBeforeNextExecution = false;

        /// <summary>
        /// Initializes a new instance of <see cref="PowerShellExecutionHostBase"/>.
        /// </summary>
        /// <param name="benchRoot">The root directory of the Bench environment.</param>
        /// <param name="scriptsDirectory">The directory where to find the <c>PsExecHost.ps1</c> script file.</param>
        protected PowerShellExecutionHostBase(string benchRoot, string scriptsDirectory)
        {
            this.benchRoot = benchRoot;
            this.scriptsDirectory = scriptsDirectory;
            if (!File.Exists(PsExecHostScriptFile))
            {
                throw new FileNotFoundException("The PowerShell host script was not found.");
            }
            this.backupHost = new DefaultExecutionHost();
            CurrentToken = Guid.NewGuid().ToString("D");
            StartPowerShellExecutionHost();
        }

        /// <summary>
        /// The root directory of the Bench environment.
        /// </summary>
        protected string BenchRoot => benchRoot;

        /// <summary>
        /// An absolute path to the <c>PsExecHost.ps1</c> script file.
        /// </summary>
        protected string PsExecHostScriptFile => Path.Combine(scriptsDirectory, HOST_SCRIPT_FILE);

        /// <summary>
        /// The current token to identify the IPC connection.
        /// </summary>
        protected string CurrentToken { get; set; }

        /// <summary>
        /// Call this to request a reload of the Bench configuration
        /// in the remote execution host, before the next execution request is processed.
        /// </summary>
        protected void RequestConfigurationReload()
        {
            reloadConfigBeforeNextExecution = true;
        }

        /// <summary>
        /// Starts the PowerShell process and runs the <c>PsExecHost.ps1</c>.
        /// </summary>
        /// <remarks>Needs to be implemented in a derived class.</remarks>
        protected abstract void StartPowerShellExecutionHost();

        /// <summary>
        /// Checks is the PowerShell process is running.
        /// </summary>
        /// <remarks>Needs to be implemented in a derived class.</remarks>
        protected abstract bool IsPowerShellExecutionHostRunning { get; }

        /// <summary>
        /// Waits for the PowerShell process to end.
        /// Is called after the shut down request was send.
        /// </summary>
        /// <remarks>Needs to be implemented in a derived class.</remarks>
        protected abstract void WaitForPowerShellExecutionHostToEnd();

        private void RemoteCall(Action<IRemoteExecHost> task)
        {
            if (!IsPowerShellExecutionHostRunning) throw new InvalidOperationException();
            hostSemaphore.WaitOne();
            try
            {
                using (var client = new RemoteExecHostClient(CurrentToken))
                {
                    task(client.ExecHost);
                }
            }
            finally
            {
                hostSemaphore.Release();
            }
        }

        private void WaitForPowerShellExecutionHost()
        {
            if (calledSuccessfully) return;
            var available = false;
            var t0 = DateTime.Now;
            while (!available && (DateTime.Now - t0).TotalMilliseconds < START_UP_TIMEOUT)
            {
                try
                {
                    string response = null;
                    RemoteCall(h => response = h.Ping());
                    available = response != null;
                }
                catch (Exception)
                {
                    Debug.WriteLine("Attempt to reach the remote execution host failed.");
                    Thread.Sleep(RETRY_INTERVAL);
                }
            }
            calledSuccessfully = available;
        }

        private static string CleanUpPowerShellTranscript(string transcript)
        {
            transcript = transcript.Trim();
            var lines = transcript.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length == 0) return transcript;
            // check if first line contains a sequence of the same character
            if (lines[0].Length > 2 && lines[0] == new string(lines[0][0], lines[0].Length))
            {
                var separator = lines[0];
                var outputLines = new List<string>();
                var p = 1;
                // search for the end of the header
                for (int i = p; i < lines.Length; i++)
                {
                    p++;
                    if (lines[i] == separator) break;
                }
                // collect lines until the start of the footer
                for (int i = p; i < lines.Length; i++)
                {
                    if (lines[i] == separator) break;
                    outputLines.Add(lines[i]);
                }
                return string.Join(Environment.NewLine, outputLines.ToArray()).Trim();
            }
            return transcript;
        }

        private void ReloadConfiguration()
        {
            if (!IsPowerShellExecutionHostRunning) return;
            WaitForPowerShellExecutionHost();
            RemoteCall(h => h.Reload());
            reloadConfigBeforeNextExecution = false;
        }

        private void StopPowerShellExecutionHost()
        {
            if (!IsPowerShellExecutionHostRunning) return;
            WaitForPowerShellExecutionHost();
            RemoteCall(h => h.Shutdown());
            WaitForPowerShellExecutionHostToEnd();
        }

        /// <summary>
        /// Starts a Windows process in a synchronous fashion.
        /// </summary>
        /// <param name="env">The environment variables of Bench.</param>
        /// <param name="cwd">The working directory, to start the process in.</param>
        /// <param name="executable">The path to the executable.</param>
        /// <param name="arguments">The string with the command line arguments.</param>
        /// <param name="monitoring">A flag to control the level of monitoring.</param>
        /// <returns>An instance of <see cref="ProcessExecutionResult"/> with the exit code
        /// and optionally the output of the process.</returns>
        /// <seealso cref="CommandLine.FormatArgumentList(string[])"/>
        public ProcessExecutionResult RunProcess(BenchEnvironment env,
            string cwd, string executable, string arguments,
            ProcessMonitoring monitoring)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(PowerShellExecutionHost));
            }
            if (!IsPowerShellExecutionHostRunning)
            {
                return backupHost.RunProcess(env, cwd, executable, arguments, monitoring);
            }
            WaitForPowerShellExecutionHost();
            if (reloadConfigBeforeNextExecution)
            {
                ReloadConfiguration();
            }

            ExecutionResult result = null;
            RemoteCall(h => result = h.Execute(new ExecutionRequest(cwd, executable, arguments)));

            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
            if (result != null)
            {
                var transcriptPath = result.TranscriptPath;
                var output = default(string);
                if (collectOutput && transcriptPath != null && File.Exists(transcriptPath))
                {
                    output = File.ReadAllText(transcriptPath, Encoding.Default);
                    output = CleanUpPowerShellTranscript(output);
                    File.Delete(transcriptPath);
                }
                return new ProcessExecutionResult(result.ExitCode, output);
            }
            else
            {
                return new ProcessExecutionResult(99999, null);
            }
        }

        /// <summary>
        /// Starts a Windows process in an asynchronous fashion.
        /// </summary>
        /// <param name="env">The environment variables of Bench.</param>
        /// <param name="cwd">The working directory, to start the process in.</param>
        /// <param name="executable">The path to the executable.</param>
        /// <param name="arguments">The string with the command line arguments.</param>
        /// <param name="cb">The handler method to call when the execution of the process finishes.</param>
        /// <param name="monitoring">A flag to control the level of monitoring.</param>
        /// <seealso cref="CommandLine.FormatArgumentList(string[])"/>
        public void StartProcess(BenchEnvironment env,
            string cwd, string executable, string arguments,
            ProcessExitCallback cb, ProcessMonitoring monitoring)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(PowerShellExecutionHost));
            }
            if (!IsPowerShellExecutionHostRunning)
            {
                backupHost.StartProcess(env, cwd, executable, arguments, cb, monitoring);
                return;
            }
            WaitForPowerShellExecutionHost();
            if (reloadConfigBeforeNextExecution)
            {
                ReloadConfiguration();
            }

            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
            AsyncManager.StartTask(() =>
            {
                ExecutionResult remoteExecResult = null;
                RemoteCall(h => remoteExecResult = h.Execute(new ExecutionRequest(cwd, executable, arguments)));
                ProcessExecutionResult result;
                if (remoteExecResult != null)
                {
                    var transcriptPath = remoteExecResult.TranscriptPath;
                    var output = default(string);
                    if (collectOutput && transcriptPath != null && File.Exists(transcriptPath))
                    {
                        output = File.ReadAllText(transcriptPath, Encoding.Default);
                        output = CleanUpPowerShellTranscript(output);
                        File.Delete(transcriptPath);
                    }
                    result = new ProcessExecutionResult(remoteExecResult.ExitCode, output);
                }
                else
                {
                    result = new ProcessExecutionResult(99999, null);
                }
                cb(result);
            });
        }

        /// <summary>
        /// Checks if this instance of already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Shuts down this execution host and kills the attached PowerShell process.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            StopPowerShellExecutionHost();
            backupHost.Dispose();
            backupHost = null;
        }
    }
}
