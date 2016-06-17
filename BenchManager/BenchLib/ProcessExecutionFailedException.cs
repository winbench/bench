using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class ProcessExecutionFailedException : Exception
    {
        public string CommandLine { get; private set; }

        public int ExitCode { get; private set; }

        public string ProcessOutput { get; private set; }

        public ProcessExecutionFailedException(string message,
            string commandLine, 
            int exitCode, string processOutput)
            : base(message)
        {
            CommandLine = commandLine;
            ExitCode = exitCode;
            ProcessOutput = processOutput;
        }
    }
}
