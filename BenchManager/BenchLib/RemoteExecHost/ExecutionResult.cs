using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.RemoteExecHost
{
    /// <summary>
    /// A serializable execution result for remote execution hosts.
    /// </summary>
    [Serializable]
    public class ExecutionResult
    {
        /// <summary>
        /// The exit code of the executed process.
        /// </summary>
        public readonly int ExitCode;

        /// <summary>
        /// An absolute path to a text file, containing the transcript of the process output.
        /// </summary>
        public readonly string TranscriptPath;

        /// <summary>
        /// Initializes a new instance of <see cref="ExecutionResult"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the process.</param>
        /// <param name="transcriptPath">An absolute path to the transcript file.</param>
        public ExecutionResult(int exitCode, string transcriptPath)
        {
            ExitCode = exitCode;
            TranscriptPath = transcriptPath;
        }
    }
}
