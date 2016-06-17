extern alias v40async;
using v40async::ConEmu.WinForms;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Mastersign.Bench.Dashboard
{
    class ConEmuExecutionHost : IProcessExecutionHost
    {
        private readonly ConEmuControl control;

        private readonly string conEmuExe;

        private XmlDocument config;

        private volatile int runningProcesses = 0;

        public event EventHandler ProcessIsRunningChanged;

        public bool ProcessIsRunning { get { return runningProcesses > 0; } }

        private IProcessExecutionHost backupHost;

        public ConEmuExecutionHost(ConEmuControl control, string conEmuExe)
        {
            this.control = control;
            this.conEmuExe = conEmuExe;
            backupHost = new DefaultExecutionHost();
            config = LoadConfigFromResource();
        }

        private void OnProcessIsRunningChanged()
        {
            var handler = ProcessIsRunningChanged;
            if (handler != null) handler(this, EventArgs.Empty);
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

        private bool IsConEmuInstalled { get { return File.Exists(conEmuExe); } }

        private ConEmuStartInfo BuildStartInfo(BenchEnvironment env, string cwd, string executable, string arguments, bool collectOutput)
        {
            // http://www.windowsinspired.com/understanding-the-command-line-string-and-arguments-received-by-a-windows-program/

            var cmdLine = (arguments.Contains("\"") ? "\"" : "")
                + CommandLine.EscapeArgument(executable) + " " + arguments;
            var si = new ConEmuStartInfo();
            si.ConEmuExecutablePath = conEmuExe;
            si.ConsoleProcessCommandLine = cmdLine;
            si.BaseConfiguration = config;
            si.StartupDirectory = cwd;
            si.IsReadingAnsiStream = collectOutput;
            env.Load((k, v) => si.SetEnv(k, v));
            env.Load();
            si.WhenConsoleProcessExits = WhenConsoleProcessExits.CloseConsoleEmulator;
            return si;
        }

        private void NotifyProcessStart()
        {
            runningProcesses++;
            if (runningProcesses == 1) OnProcessIsRunningChanged();
        }

        private void NotifyProcessEnd()
        {
            runningProcesses--;
            if (runningProcesses == 0) OnProcessIsRunningChanged();
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

        public ProcessExecutionResult RunProcess(BenchEnvironment env,
            string cwd, string executable, string arguments,
            ProcessMonitoring monitoring)
        {
            if (!IsConEmuInstalled)
            {
                return backupHost.RunProcess(env, cwd, executable, arguments, monitoring);
            }
            if (!File.Exists(executable))
            {
                throw new FileNotFoundException("The executable could not be found.", executable);
            }
            if (!Directory.Exists(cwd))
            {
                throw new DirectoryNotFoundException("The working directory could not be found: " + cwd);
            }
            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
            var startInfo = BuildStartInfo(env, cwd, executable, arguments, collectOutput);
            NotifyProcessStart();
            var session = StartProcess(startInfo);
            StringBuilder sb = collectOutput ? new StringBuilder() : null;
            if (collectOutput)
            {
                session.AnsiStreamChunkReceived += (s, e) => sb.Append(e.GetMbcsText());
            }
            var t = session.WaitForConsoleEmulatorCloseAsync();
            t.Wait();
            NotifyProcessEnd();
            var exitCode = t.IsFaulted ? 999999 : session.GetConsoleProcessExitCode();
            if (collectOutput)
            {
                return new ProcessExecutionResult(exitCode, sb.ToString());
            }
            else
            {
                return new ProcessExecutionResult(exitCode);
            }
        }

        public void StartProcess(BenchEnvironment env,
            string cwd, string executable, string arguments,
            ProcessExitCallback cb, ProcessMonitoring monitoring)
        {
            if (!IsConEmuInstalled)
            {
                backupHost.StartProcess(env, cwd, executable, arguments, cb, monitoring);
                return;
            }
            if (!File.Exists(executable))
            {
                throw new FileNotFoundException("The executable could not be found.", executable);
            }
            if (!Directory.Exists(cwd))
            {
                throw new DirectoryNotFoundException("The working directory could not be found: " + cwd);
            }
            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
            var startInfo = BuildStartInfo(env, cwd, executable, arguments, collectOutput);
            NotifyProcessStart();
            var session = StartProcess(startInfo);
            var sb = new StringBuilder();
            if (collectOutput)
            {
                session.AnsiStreamChunkReceived += (s, e) => sb.Append(e.GetMbcsText());
            }
            var t = session.WaitForConsoleEmulatorCloseAsync();
            AsyncManager.StartTask(() =>
            {
                t.Wait();
                NotifyProcessEnd();
                var exitCode = t.IsFaulted ? 999999 : session.GetConsoleProcessExitCode();
                var result = collectOutput
                    ? new ProcessExecutionResult(exitCode, sb.ToString())
                    : new ProcessExecutionResult(exitCode);
                cb(result);
            });
        }

        private delegate ConEmuSession ConEmuStarter(ConEmuStartInfo si);
    }
}
