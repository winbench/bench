using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace Mastersign.Bench
{
    public class DefaultExecutionHost : IProcessExecutionHost
    {
        public void StartProcess(BenchEnvironment env,
            string cwd, string exe, string arguments,
            ProcessExitCallback cb, ProcessMonitoring monitoring)
        {
            AsyncManager.StartTask(() =>
            {
                cb(RunProcess(env, cwd, exe, arguments, monitoring));
            });
        }

        public ProcessExecutionResult RunProcess(BenchEnvironment env,
            string cwd, string exe, string arguments,
            ProcessMonitoring monitoring)
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
            var collectOutput = (monitoring & ProcessMonitoring.Output) == ProcessMonitoring.Output;
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
