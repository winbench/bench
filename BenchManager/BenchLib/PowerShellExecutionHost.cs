using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Mastersign.Bench.RemoteExecHost;

namespace Mastersign.Bench
{
    /// <summary>
    /// This <see cref="IProcessExecutionHost"/> launches a PowerShell process
    /// with the <c>PsExecHost.ps1</c> script and uses <see cref="RemoteExecHostClient"/>
    /// to request process executions.
    /// </summary>
    public class PowerShellExecutionHost : IProcessExecutionHost
    {
        private const string PowerShellHostScript = "PsExecHost.ps1";

        private const int START_UP_TIMEOUT = 10000;
        private const int RETRY_INTERVAL = 250;

        private readonly Semaphore hostSemaphore = new Semaphore(1, 1);

        private BenchConfiguration config;

        private IProcessExecutionHost backupHost;

        private string currentToken;

        private Process currentPsProcess;

        private bool reloadConfigBeforeNextExecution = false;

        /// <summary>
        /// Initializes a new instance of <see cref="PowerShellExecutionHost"/>.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        public PowerShellExecutionHost(BenchConfiguration config)
        {
            this.config = config;
            backupHost = new DefaultExecutionHost();
            StartPowerShellExecutionHost();
            WaitForPowerShellExecutionHost(START_UP_TIMEOUT);
        }

        private void CoreConfigReloadedHandler(object sender, EventArgs e)
        {
            reloadConfigBeforeNextExecution = true;
        }

        private ProcessStartInfo BuildStartInfo(string cwd, string executable, string arguments)
        {
            var cmdLine = CommandLine.EscapeArgument(executable) + " " + arguments;
            var si = new ProcessStartInfo();
            si.FileName = PowerShell.Executable;
            si.Arguments = arguments;
            si.WorkingDirectory = cwd;
            si.UseShellExecute = false;
            return si;
        }

        private Process StartProcess(ProcessStartInfo startInfo)
        {
            return Process.Start(startInfo);
        }

        private void StartPowerShellExecutionHost()
        {
            var hostScript = Path.Combine(config.GetStringValue(PropertyKeys.BenchScripts), PowerShellHostScript);
            if (!File.Exists(hostScript))
            {
                throw new FileNotFoundException("The PowerShell host script was not found.");
            }
            currentToken = Guid.NewGuid().ToString("D");
            var cwd = config.GetStringValue(PropertyKeys.BenchRoot);
            var startInfo = BuildStartInfo(cwd, PowerShell.Executable,
                string.Join(" ", new[] {
                    "-NoProfile", "-NoLogo",
                    "-ExecutionPolicy", "Unrestricted",
                    "-File", "\"" + hostScript + "\"",
                    "-Token", currentToken, "-WaitMessage", "\"\"" }));
            currentPsProcess = StartProcess(startInfo);
            currentPsProcess.Exited += (s, o) =>
            {
                currentToken = null;
                currentPsProcess = null;
            };
        }

        private bool IsPowerShellExecutionHostRunning =>
            currentPsProcess != null;

        private void WaitForSessionToEnd()
        {
            while (!currentPsProcess.HasExited)
            {
                currentPsProcess.WaitForExit();
            }
        }

        private void RemoteCall(Action<IRemoteExecHost> task)
        {
            if (!IsPowerShellExecutionHostRunning) throw new InvalidOperationException();
            hostSemaphore.WaitOne();
            try
            {
                using (var client = new RemoteExecHostClient(currentToken))
                {
                    task(client.ExecHost);
                }
            }
            finally
            {
                hostSemaphore.Release();
            }
        }

        private void WaitForPowerShellExecutionHost(int timeout)
        {
            var available = false;
            var t0 = DateTime.Now;
            while(!available && (DateTime.Now - t0).TotalMilliseconds < timeout)
            {
                try
                {
                    string response = null;
                    RemoteCall(h => response = h.Ping());
                    available = response != null;
                }
                catch (Exception)
                {
                    Thread.Sleep(RETRY_INTERVAL);
                }
            }
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
            RemoteCall(h => h.Reload());
            reloadConfigBeforeNextExecution = false;
        }

        private void StopPowerShellExecutionHost()
        {
            if (!IsPowerShellExecutionHostRunning) return;
            RemoteCall(h => h.Shutdown());
            WaitForSessionToEnd();
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
