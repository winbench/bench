using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO.Pipes;
using System.Diagnostics;
using System.Windows.Forms;
using ConEmu.WinForms;
using Mastersign.Bench.RemoteExecHost;

namespace Mastersign.Bench.Dashboard
{
    class ConEmuExecutionHost : IProcessExecutionHost
    {
        private const string PowerShellHostScript = "PsExecHost.ps1";

        private const int START_UP_TIMEOUT = 10000;
        private const int RETRY_INTERVAL = 250;

        private readonly ConEmuControl control;

        private readonly Core core;

        private readonly string conEmuExe;

        private readonly Semaphore hostSemaphore = new Semaphore(1, 1);

        private XmlDocument config;

        private string currentToken;
        private ConEmuSession currentSession;

        private bool reachedAtLeastOnce = false;
        private IProcessExecutionHost backupHost;

        private bool reloadConfigBeforeNextExecution = false;

        public ConEmuExecutionHost(Core core, ConEmuControl control, string conEmuExe)
        {
            this.core = core;
            this.control = control;
            this.conEmuExe = conEmuExe;
            backupHost = new DefaultExecutionHost();
            config = LoadConfigFromResource();
            StartPowerShellExecutionHost();
            this.core.ConfigReloaded += CoreConfigReloadedHandler;
        }

        private void CoreConfigReloadedHandler(object sender, EventArgs e)
        {
            reloadConfigBeforeNextExecution = true;
        }

        private XmlDocument LoadConfigFromResource()
        {
            var doc = new XmlDocument();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Resources.ConEmu.xml";
            using (var s = assembly.GetManifestResourceStream(GetType(), resourceName))
            {
                doc.Load(s);
            }
            return doc;
        }

        private bool IsConEmuInstalled => File.Exists(conEmuExe);

        private ConEmuStartInfo BuildStartInfo(string cwd, string executable, string arguments)
        {
            // http://www.windowsinspired.com/understanding-the-command-line-string-and-arguments-received-by-a-windows-program/

            var cmdLine = (arguments.Contains("\"") ? "\"" : "")
                + CommandLine.EscapeArgument(executable) + " " + arguments;
            var si = new ConEmuStartInfo();
            si.ConEmuExecutablePath = conEmuExe;
            si.ConsoleProcessCommandLine = cmdLine;
            si.BaseConfiguration = config;
            si.StartupDirectory = cwd;
            si.IsReadingAnsiStream = false;
            si.WhenConsoleProcessExits = WhenConsoleProcessExits.CloseConsoleEmulator;
            return si;
        }

        private ConEmuSession StartProcess(ConEmuStartInfo startInfo)
        {
            if (control.InvokeRequired)
            {
                return (ConEmuSession)control.Invoke(
                    (ConEmuStarter)(si => { return control.Start(si); }),
                    startInfo);
            }
            return control.Start(startInfo);
        }

        private void StartPowerShellExecutionHost()
        {
            if (!IsConEmuInstalled)
            {
                return;
            }
            var config = core.Config;
            var hostScript = Path.Combine(config.GetStringValue(PropertyKeys.BenchScripts), PowerShellHostScript);
            if (!File.Exists(hostScript))
            {
                throw new FileNotFoundException("The PowerShell host script was not found.");
            }
            currentToken = Guid.NewGuid().ToString("D");
            var cwd = config.GetStringValue(PropertyKeys.BenchRoot);
            var startInfo = BuildStartInfo(cwd, PowerShell.Executable,
                "\"" + string.Join(" ", "-NoProfile", "-NoLogo",
                    "-ExecutionPolicy", "Unrestricted",
                    "-File", "\"" + hostScript + "\"",
                    "-Token", currentToken));
            currentSession = StartProcess(startInfo);
            currentSession.ConsoleEmulatorClosed += (s, o) =>
            {
                currentToken = null;
                currentSession = null;
            };
        }

        private bool IsPowerShellExecutionHostRunning =>
            currentSession != null;

        private void WaitForSessionToEnd()
        {
            while (currentSession != null)
            {
                if (control.InvokeRequired)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    Application.DoEvents();
                }
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

        private void WaitForPowerShellExecutionHost()
        {
            if (reachedAtLeastOnce) return;
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
            reachedAtLeastOnce = available;
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
            WaitForSessionToEnd();
        }

        public ProcessExecutionResult RunProcess(BenchEnvironment env,
            string cwd, string executable, string arguments,
            ProcessMonitoring monitoring)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(ConEmuExecutionHost));
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

        public void StartProcess(BenchEnvironment env,
            string cwd, string executable, string arguments,
            ProcessExitCallback cb, ProcessMonitoring monitoring)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(ConEmuExecutionHost));
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

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            core.ConfigReloaded -= CoreConfigReloadedHandler;
            StopPowerShellExecutionHost();
            backupHost.Dispose();
            backupHost = null;
        }

        private delegate ConEmuSession ConEmuStarter(ConEmuStartInfo si);
    }
}
