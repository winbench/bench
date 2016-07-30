using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This interface describes the capability to execute Windows processes
    /// in a synchronous or asynchronous fashion.
    /// </summary>
    public interface IProcessExecutionHost : IDisposable
    {
        /// <summary>
        /// Starts a Windows process in an asynchronous fashion.
        /// </summary>
        /// <param name="env">The environment variables of Bench.</param>
        /// <param name="cwd">The working directory, to start the process in.</param>
        /// <param name="executable">The path to the executable.</param>
        /// <param name="arguments">The string with the command line arguments.</param>
        /// <param name="cb">The handler method to call when the execution of the process finishes.</param>
        /// <param name="monitoring">A flag to control the level of monitoring.</param>
        /// <seealso cref="CommandLine.FormatArgumentList(string[])"/>
        void StartProcess(BenchEnvironment env, string cwd, 
            string executable, string arguments, 
            ProcessExitCallback cb, ProcessMonitoring monitoring);

        /// <summary>
        /// Starts a Windows process in a synchronous fashion.
        /// </summary>
        /// <param name="env">The environment variables of Bench.</param>
        /// <param name="cwd">The working directory, to start the process in.</param>
        /// <param name="executable">The path to the executable.</param>
        /// <param name="arguments">The string with the command line arguments.</param>
        /// <param name="monitoring">A flag to control the level of monitoring.</param>
        /// <returns>An instance of <see cref="ProcessExecutionResult"/> with the exit code
        /// and optionally the output of the process.</returns>
        /// <seealso cref="CommandLine.FormatArgumentList(string[])"/>
        ProcessExecutionResult RunProcess(BenchEnvironment env, string cwd, 
            string executable, string arguments, 
            ProcessMonitoring monitoring);
    }

    /// <summary>
    /// An enumeration with the possible levels of process monitoring.
    /// </summary>
    /// <seealso cref="IProcessExecutionHost"/>
    [Flags]
    public enum ProcessMonitoring : int
    {
        /// <summary>
        /// Just record the exit code of the process.
        /// </summary>
        ExitCode = 0x01,

        /// <summary>
        /// Just record the output of the process.
        /// </summary>
        Output = 0x02,

        /// <summary>
        /// Record the exit code and the output of the process.
        /// </summary>
        ExitCodeAndOutput = 0x03,
    }

    /// <summary>
    /// This class represents the result from a Windows process execution.
    /// </summary>
    /// <seealso cref="IProcessExecutionHost"/>
    public class ProcessExecutionResult
    {
        /// <summary>
        /// The exit code of the process.
        /// If this value is <c>0</c>, he process is considered to be successfull.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <summary>
        /// The process output as string, or <c>null</c>.
        /// </summary>
        public string Output { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ProcessExecutionResult"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the process.</param>
        /// <param name="output">The output of the process decoded as a string.</param>
        public ProcessExecutionResult(int exitCode, string output)
        {
            ExitCode = exitCode;
            Output = output;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ProcessExecutionResult"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the process.</param>
        public ProcessExecutionResult(int exitCode)
            : this(exitCode, null)
        {
        }
    }
}
