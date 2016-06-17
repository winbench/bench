using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IProcessExecutionHost
    {
        void StartProcess(BenchEnvironment env, string cwd, string executable, string arguments, ProcessExitCallback cb, ProcessMonitoring monitorin);

        ProcessExecutionResult RunProcess(BenchEnvironment env, string cwd, string executable, string arguments, ProcessMonitoring monitoring);
    }

    [Flags]
    public enum ProcessMonitoring : int
    {
        ExitCode = 0x01,
        Output = 0x02,
        ExitCodeAndOutput = 0x03,
    }

    public class ProcessExecutionResult
    {
        public int ExitCode { get; private set; }

        public string Output { get; private set; }

        public ProcessExecutionResult(int exitCode, string output)
        {
            ExitCode = exitCode;
            Output = output;
        }

        public ProcessExecutionResult(int exitCode) 
            : this(exitCode, null)
        {
        }
    }
}
