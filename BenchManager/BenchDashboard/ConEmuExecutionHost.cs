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
    class ConEmuExecutionHost : PowerShellExecutionHostBase
    {
        private readonly ConEmuControl control;

        private readonly Core core;

        private readonly string conEmuExe;

        private XmlDocument config;

        private ConEmuSession currentSession;

        public ConEmuExecutionHost(Core core, ConEmuControl control, string conEmuExe)
            : base(core.Config.BenchRootDir, core.Config.GetStringValue(PropertyKeys.BenchScripts))
        {
            this.core = core;
            this.control = control;
            this.conEmuExe = conEmuExe;
            config = LoadConfigFromResource();
            this.core.ConfigReloaded += CoreConfigReloadedHandler;
        }

        private void CoreConfigReloadedHandler(object sender, EventArgs e)
        {
            RequestConfigurationReload();
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

        protected override void StartPowerShellExecutionHost()
        {
            if (!IsConEmuInstalled) return;
            var startInfo = BuildStartInfo(BenchRoot, PowerShell.Executable,
                "\"" + string.Join(" ", "-NoProfile", "-NoLogo",
                    "-ExecutionPolicy", "Unrestricted",
                    "-File", "\"" + PsExecHostScriptFile + "\"",
                    "-Token", CurrentToken));
            currentSession = StartProcess(startInfo);
            currentSession.ConsoleEmulatorClosed += (s, o) =>
            {
                CurrentToken = null;
                currentSession = null;
            };
        }

        protected override bool IsPowerShellExecutionHostRunning =>
            currentSession != null;

        protected override void WaitForPowerShellExecutionHostToEnd()
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

        protected override void OnDispose()
        {
            this.core.ConfigReloaded -= CoreConfigReloadedHandler;
        }

        private delegate ConEmuSession ConEmuStarter(ConEmuStartInfo si);
    }
}
