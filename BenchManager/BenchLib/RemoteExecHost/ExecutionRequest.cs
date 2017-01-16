using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.RemoteExecHost
{
    /// <summary>
    /// A serializable execution request for remote execution hosts.
    /// </summary>
    [Serializable]
    public class ExecutionRequest
    {
        /// <summary>
        /// The directory in which the execution shell take place.
        /// </summary>
        public string WorkingDirectory;

        /// <summary>
        /// An absolute path to the executable to run.
        /// </summary>
        public string Executable;

        /// <summary>
        /// The command line argument string to pass to the executable.
        /// </summary>
        public string Arguments;

        /// <summary>
        /// Initializes a new instance of <see cref="ExecutionRequest"/>.
        /// </summary>
        /// <param name="wd">An absolute path to the working directory for the execution.</param>
        /// <param name="cmd">An absolute path to the executable.</param>
        /// <param name="cmdArgs">The command line argument string.</param>
        public ExecutionRequest(string wd, string cmd, string cmdArgs)
        {
            WorkingDirectory = wd;
            Executable = cmd;
            Arguments = cmdArgs;
        }
    }
}
