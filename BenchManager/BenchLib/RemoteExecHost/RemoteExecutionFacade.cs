using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mastersign.Bench.RemoteExecHost
{
    /// <summary>
    /// This class provides statically the queued remote commands and
    /// dynamically serves as the <c>SingleCall</c> remote object
    /// for <see cref="IRemoteExecHost"/>.
    /// </summary>
    public class RemoteExecutionFacade : MarshalByRefObject, IRemoteExecHost
    {
        private static readonly Queue<RemoteCommand> cmdQueue = new Queue<RemoteCommand>();

        private static readonly ManualResetEvent queueEvent = new ManualResetEvent(false);

        private static readonly object queueLock = new object();

        /// <summary>
        /// Waits for the next command which is received remotely.
        /// Blocks until a command arrives.
        /// </summary>
        /// <returns>A remote command object.</returns>
        public static RemoteCommand WaitForCommand()
        {
            var wait = false;
            lock (queueLock)
            {
                if (cmdQueue.Count == 0)
                {
                    queueEvent.Reset();
                    wait = true;
                }
            }
            if (wait) queueEvent.WaitOne();
            lock (queueLock)
            {
                return cmdQueue.Dequeue();
            }
        }

        private static void EnqueueCommand(RemoteCommand cmd)
        {
            lock (queueLock)
            {
                cmdQueue.Enqueue(cmd);
                queueEvent.Set();
            }
        }

        /// <summary>
        /// Requests the execution of a program.
        /// </summary>
        /// <param name="requ">The request parameter.</param>
        /// <returns>The execution result.</returns>
        public ExecutionResult Execute(ExecutionRequest requ)
        {
            var cmd = new RemoteExecutionRequest(requ);
            EnqueueCommand(cmd);
            return cmd.WaitForResult();
        }

        /// <summary>
        /// Requests the execution host to reload the Bench configuration.
        /// </summary>
        public void Reload()
        {
            EnqueueCommand(new RemoteCommand(RemoteCommandType.Reload));
        }

        /// <summary>
        /// Requests the execution host to shut down.
        /// </summary>
        public void Shutdown()
        {
            EnqueueCommand(new RemoteCommand(RemoteCommandType.Shutdown));
        }
    }
}
