using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Mastersign.Bench.RemoteExecHost;

namespace Mastersign.Bench
{
    /// <summary>
    /// This <see cref="IProcessExecutionHost"/> launches a PowerShell process
    /// with the <c>PsExecHost.ps1</c> script and uses <see cref="RemoteExecHostClient"/>
    /// to request process executions.
    /// </summary>
    public class PowerShellExecutionHost : PowerShellExecutionHostBase
    {
        private Process currentPsProcess;

        /// <summary>
        /// Initializes a new instance of <see cref="PowerShellExecutionHost"/>.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        public PowerShellExecutionHost(BenchConfiguration config)
            : base(config.BenchRootDir, config.GetStringValue(ConfigPropertyKeys.BenchScripts))
        {
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

        /// <summary>
        /// Starts the PowerShell process and runs the <c>PsExecHost.ps1</c>.
        /// </summary>
        protected override void StartPowerShellExecutionHost()
        {
            var startInfo = BuildStartInfo(BenchRoot, PowerShell.Executable,
                string.Join(" ", new[] {
                    "-NoProfile", "-NoLogo",
                    "-ExecutionPolicy", "Unrestricted",
                    "-File", "\"" + PsExecHostScriptFile + "\"",
                    "-Token", CurrentToken, "-WaitMessage", "\"\"" }));
            currentPsProcess = Process.Start(startInfo);
            currentPsProcess.Exited += (s, o) =>
            {
                CurrentToken = null;
                currentPsProcess = null;
            };
        }

        /// <summary>
        /// Checks is the PowerShell process is running.
        /// </summary>
        protected override bool IsPowerShellExecutionHostRunning =>
            currentPsProcess != null;

        /// <summary>
        /// Waits for the PowerShell process to end.
        /// Is called after the shut down request was send.
        /// </summary>
        protected override void WaitForPowerShellExecutionHostToEnd()
        {
            while (!currentPsProcess.HasExited)
            {
                currentPsProcess.WaitForExit();
            }
        }
    }
}
