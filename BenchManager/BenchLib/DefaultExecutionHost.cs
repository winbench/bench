using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class is the default implementation of <see cref="IProcessExecutionHost"/>.
    /// It starts processes invisible in the background
    /// and allows no user interaction during the process execution.
    /// </summary>
    public class DefaultExecutionHost : IProcessExecutionHost
    {
        /// <summary>
        /// Starts a Windows process in an asynchronous fashion.
        /// </summary>
        /// <param name="env">The environment variables of Bench.</param>
        /// <param name="cwd">The working directory, to start the process in.</param>
        /// <param name="exe">The path to the executable.</param>
        /// <param name="arguments">The string with the command line arguments.</param>
        /// <param name="cb">The handler method to call when the execution of the process finishes.</param>
        /// <param name="monitoring">A flag to control the level of monitoring.</param>
        /// <seealso cref="CommandLine.FormatArgumentList(string[])"/>
        public void StartProcess(BenchEnvironment env,
            string cwd, string exe, string arguments,
            ProcessExitCallback cb, ProcessMonitoring monitoring)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(DefaultExecutionHost));
            }
            if (cb != null)
            {
                AsyncManager.StartTask(() =>
                {
                    cb(RunProcess(env, cwd, exe, arguments, monitoring));
                });
            }
            else
            {
                StringBuilder sbStd, sbErr;
                StartProcess(env, cwd, exe, arguments, false, out sbStd, out sbErr);
            }
        }

        /// <summary>
        /// Starts a Windows process in a synchronous fashion.
        /// </summary>
        /// <param name="env">The environment variables of Bench.</param>
        /// <param name="cwd">The working directory, to start the process in.</param>
        /// <param name="exe">The path to the executable.</param>
        /// <param name="arguments">The string with the command line arguments.</param>
        /// <param name="monitoring">A flag to control the level of monitoring.</param>
        /// <returns>An instance of <see cref="ProcessExecutionResult"/> with the exit code
        /// and optionally the output of the process.</returns>
        /// <seealso cref="CommandLine.FormatArgumentList(string[])"/>
        public ProcessExecutionResult RunProcess(BenchEnvironment env,
            string cwd, string exe, string arguments,
            ProcessMonitoring monitoring)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(DefaultExecutionHost));
            }
            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
            StringBuilder sbStd, sbErr;
            var p = StartProcess(env, cwd, exe, arguments, collectOutput, out sbStd, out sbErr);
            p.WaitForExit();
            if (collectOutput)
            {
                string output;
                try
                {
                    output = FormatOutput(sbStd.ToString())
                        + Environment.NewLine
                        + FormatOutput(sbErr.ToString());
                }
                catch (Exception e)
                {
                    output = e.ToString();
                }
                return new ProcessExecutionResult(p.ExitCode, output);
            }
            else
            {
                return new ProcessExecutionResult(p.ExitCode);
            }
        }

        private Process StartProcess(BenchEnvironment env, string cwd, string exe, string arguments, 
            bool collectOutput, out StringBuilder stdOut, out StringBuilder errOut)
        {
            var p = new Process();
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("The executable could not be found.", exe);
            }
            if (!Directory.Exists(cwd))
            {
                throw new DirectoryNotFoundException("The working directory could not be found: " + cwd);
            }
            PreparePowerShellScriptExecution(ref exe, ref arguments);
            StringBuilder sbStd = null;
            StringBuilder sbErr = null;
            var si = new ProcessStartInfo(exe, arguments);
            si.UseShellExecute = false;
            si.WorkingDirectory = cwd;
            si.CreateNoWindow = collectOutput;
            si.RedirectStandardOutput = collectOutput;
            si.RedirectStandardError = collectOutput;
            env.Load(si.EnvironmentVariables);
            p.StartInfo = si;
            if (collectOutput)
            {
                sbStd = new StringBuilder();
                p.OutputDataReceived += (s, e) => sbStd.AppendLine(e.Data);
                sbErr = new StringBuilder();
                p.ErrorDataReceived += (s, e) => sbErr.AppendLine(e.Data);
            }
            p.Start();
            if (collectOutput)
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }
            stdOut = sbStd;
            errOut = sbErr;
            return p;
        }

        /// <summary>
        /// Checks, whether this instance was disposed, or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes this instance and frees all ocupied resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
        }

        private static void PreparePowerShellScriptExecution(ref string exe, ref string args)
        {
            if (Path.GetExtension(exe).Equals(".ps1", StringComparison.InvariantCultureIgnoreCase))
            {
                var command = string.Format(
                    "$cmdArgs = $ExecutionContext.InvokeCommand.InvokeScript(\"{1}\"); & \"{0}\" @cmdArgs",
                    exe, args.Replace("\"", "`\""));
                var encodedCommand = Convert.ToBase64String(Encoding.Unicode.GetBytes(command));
                exe = PowerShell.Executable;
                args = CommandLine.FormatArgumentList(
                    "-ExecutionPolicy", "Unrestricted", "-NoLogo", "-NoProfile", "-OutputFormat", "Text",
                    "-EncodedCommand", encodedCommand);
            }
        }

        private static string CliXmlFormatter(Match m)
        {
            var xml = m.Value;
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(m.Value);
            }
            catch (Exception) { }
            var sb = new StringBuilder();
            var nsm = new XmlNamespaceManager(new NameTable());
            nsm.AddNamespace("ps", "http://schemas.microsoft.com/powershell/2004/04");
            foreach (XmlElement e in doc.SelectNodes("/ps:Objs/ps:Obj/ps:ToString", nsm))
            {
                sb.AppendLine(e.InnerText);
            }
            return sb.ToString();
        }

        private static string FormatOutput(string str)
        {
            str = str.Trim();
            var preamble = "#< CLIXML";
            if (str.StartsWith(preamble))
            {
                str = str.Substring(preamble.Length).TrimStart();
            }
            var xmlObjsPattern = new Regex(@"\<Objs ([\s\S]*?)\<\/Objs\>");
            str = xmlObjsPattern.Replace(str, CliXmlFormatter);
            return str;
        }
    }
}
