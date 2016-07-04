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

namespace Mastersign.Bench.Dashboard
{
    class ConEmuExecutionHost : IProcessExecutionHost
    {
        private const string PowerShellHostScript = "PsExecHost.ps1";

        private const int CONNECTION_TIMEOUT = 5000;

        private const string EXITCODE_LINE_FORMAT = "EXITCODE {0} ";
        private const string TRANSCRIPTPATH_LINE_FORMAT = "TRANSCRIPT {0} ";

        private readonly ConEmuControl control;

        private readonly Core core;

        private readonly string conEmuExe;

        private readonly Semaphore hostSemaphore = new Semaphore(1, 1);

        private XmlDocument config;

        private string currentToken;
        private ConEmuSession currentSession;

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

        private IEnumerable<string> SendCommand(string command, params string[] arguments)
        {
            if (!IsPowerShellExecutionHostRunning) throw new InvalidOperationException();
            hostSemaphore.WaitOne();
            try
            {
                var client = new NamedPipeClientStream(".", currentToken, PipeDirection.InOut);
                TextWriter w;
                TextReader r;
                try
                {
                    client.Connect(CONNECTION_TIMEOUT);
                    w = new StreamWriter(client, Encoding.UTF8, 4, true);
                    r = new StreamReader(client, Encoding.UTF8, false, 4, true);
                }
                catch (TimeoutException)
                {
                    yield break;
                }
                catch (IOException ioEx)
                {
                    Debug.WriteLine(ioEx);
                    yield break;
                }
                w.WriteLine(command);
                foreach (var arg in arguments)
                {
                    w.WriteLine(arg);
                }
                w.Flush();
                while (client.IsConnected)
                {
                    var l = r.ReadLine();
                    if (l != null)
                    {
                        yield return l;
                    }
                }
                r.Dispose();
                client.Dispose();
            }
            finally
            {
                hostSemaphore.Release();
            }
        }

        private void ReloadConfiguration()
        {
            if (!IsPowerShellExecutionHostRunning) return;
            SendCommand("reload").Any(l => l == "OK");
            reloadConfigBeforeNextExecution = false;
        }

        private void StopPowerShellExecutionHost()
        {
            if (!IsPowerShellExecutionHostRunning) return;
            SendCommand("close").Any(l => l == "OK");
            WaitForSessionToEnd();
        }

        private bool ParseExitCode(string line, ref int exitCode)
        {
            var exitCodePrefix = string.Format(EXITCODE_LINE_FORMAT, currentToken);
            if (line.StartsWith(exitCodePrefix))
            {
                var number = line.Substring(exitCodePrefix.Length);
                int tmp;
                if (int.TryParse(number, out tmp)) exitCode = tmp;
                return true;
            }
            return false;
        }

        private bool ParseTranscriptPath(string line, ref string transcriptPath)
        {
            var exitCodePrefix = string.Format(TRANSCRIPTPATH_LINE_FORMAT, currentToken);
            if (line.StartsWith(exitCodePrefix))
            {
                transcriptPath = line.Substring(exitCodePrefix.Length);
                return true;
            }
            return false;
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
            if (reloadConfigBeforeNextExecution)
            {
                ReloadConfiguration();
            }
            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
            var response = SendCommand("exec", cwd, executable, arguments);
            var exitCode = 999999;
            var transcriptPath = default(string);
            foreach (var l in response)
            {
                ParseExitCode(l, ref exitCode);
                ParseTranscriptPath(l, ref transcriptPath);
            }
            var output = default(string);
            if (collectOutput && transcriptPath != null && File.Exists(transcriptPath))
            {
                output = File.ReadAllText(transcriptPath, Encoding.Default);
                File.Delete(transcriptPath);
            }
            return new ProcessExecutionResult(exitCode, output);
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
            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
            var response = SendCommand("exec", cwd, executable, arguments);
            AsyncManager.StartTask(() =>
            {
                var exitCode = 999999;
                var transcriptPath = default(string);
                foreach (var l in response)
                {
                    ParseExitCode(l, ref exitCode);
                    ParseTranscriptPath(l, ref transcriptPath);
                }
                var output = default(string);
                if (collectOutput && transcriptPath != null && File.Exists(transcriptPath))
                {
                    output = File.ReadAllText(transcriptPath, Encoding.Default);
                    File.Delete(transcriptPath);
                }
                var result = new ProcessExecutionResult(exitCode, output);
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
