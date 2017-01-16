using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mastersign.Bench.RemoteExecHost
{
    /// <summary>
    /// A class to represent a remotely requested command.
    /// </summary>
    public class RemoteCommand
    {
        /// <summary>
        /// The type of this command.
        /// </summary>
        public RemoteCommandType Type;

        /// <summary>
        /// Initializes a new instance of <see cref="RemoteCommand"/>.
        /// </summary>
        /// <param name="type">The command type.</param>
        public RemoteCommand(RemoteCommandType type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// A class to represent a remotely requested process execution .
    /// </summary>
    public class RemoteExecutionRequest : RemoteCommand
    {
        /// <summary>
        /// The parameter describing the execution.
        /// </summary>
        public ExecutionRequest Parameter;

        private ManualResetEvent e = new ManualResetEvent(false);

        private ExecutionResult result;

        /// <summary>
        /// Initializes a new instance of <see cref="RemoteExecutionRequest"/>.
        /// </summary>
        /// <param name="parameter">The parameter describing the execution.</param>
        public RemoteExecutionRequest(ExecutionRequest parameter)
            : base(RemoteCommandType.Execution)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// Hands the results of the execution to who ever calls <see cref="WaitForResult"/>.
        /// </summary>
        /// <param name="result">The execution result.</param>
        public void NotifyResult(ExecutionResult result)
        {
            this.result = result;
            e.Set();
        }

        /// <summary>
        /// Gets the result of the execution.
        /// Blocks until the result was notified with <see cref="NotifyResult(ExecutionResult)"/>.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult WaitForResult()
        {
            e.WaitOne();
            return result;
        }
    }

    /// <summary>
    /// The different remote command types.
    /// </summary>
    public enum RemoteCommandType
    {
        /// <summary>
        /// A process execution request (<see cref="IRemoteExecHost.Execute(ExecutionRequest)"/>).
        /// </summary>
        Execution,

        /// <summary>
        /// A request to reload the Bench configuration (<see cref="IRemoteExecHost.Reload"/>).
        /// </summary>
        Reload,

        /// <summary>
        /// A request to shutdown the remote execution host (<see cref="IRemoteExecHost.Shutdown"/>).
        /// </summary>
        Shutdown,
    }
}
