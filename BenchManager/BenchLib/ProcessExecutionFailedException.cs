using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This exception class is used to describe a failed process execution.
    /// This class contains the command line, which started the process,
    /// the exit code of the failed process and optionally the process output.
    /// </summary>
    public class ProcessExecutionFailedException : Exception
    {
        /// <summary>
        /// The command line, which started the process.
        /// </summary>
        public string CommandLine { get; private set; }

        /// <summary>
        /// The exit code of the process.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <summary>
        /// The output of the process, or <c>null</c>.
        /// </summary>
        public string ProcessOutput { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ProcessExecutionFailedException"/>.
        /// </summary>
        /// <param name="message">The error message, associated with the failing process.</param>
        /// <param name="commandLine">The command line which started the process.</param>
        /// <param name="exitCode">The exit code from the process.</param>
        /// <param name="processOutput">The process output or <c>null</c>.</param>
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
