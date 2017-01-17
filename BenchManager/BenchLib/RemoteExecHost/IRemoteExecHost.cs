using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.RemoteExecHost
{
    /// <summary>
    /// The interface for a remote execution host.
    /// </summary>
    public interface IRemoteExecHost
    {
        /// <summary>
        /// Does nothing but checking the communication roundtrip.
        /// </summary>
        /// <returns>Some string, not <c>null</c>.</returns>
        string Ping();

        /// <summary>
        /// Requests the execution of a program.
        /// </summary>
        /// <param name="requ">The request parameter.</param>
        /// <returns>The execution result.</returns>
        ExecutionResult Execute(ExecutionRequest requ);

        /// <summary>
        /// Requests the execution host to reload the Bench configuration.
        /// </summary>
        void Reload();

        /// <summary>
        /// Requests the execution host to shut down.
        /// </summary>
        void Shutdown(); 
    }
}
