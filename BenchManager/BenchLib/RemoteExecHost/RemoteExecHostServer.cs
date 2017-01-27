using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;

namespace Mastersign.Bench.RemoteExecHost
{
    /// <summary>
    /// A server to host a remote execution host via IPC.
    /// The command requests are queued and served statically in <see cref="RemoteExecutionFacade"/>.
    /// </summary>
    public class RemoteExecHostServer : IDisposable
    {
        private IpcChannel ipcChannel;

        /// <summary>
        /// Initializes a new instance of <see cref="RemoteExecHostServer"/>.
        /// </summary>
        /// <param name="token">A unique string to identify this server.</param>
        public RemoteExecHostServer(string token)
        {
            ipcChannel = new IpcChannel("Bench_ExecHost_" + token);
            ChannelServices.RegisterChannel(ipcChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(RemoteExecutionFacade), "RemoteExecHost",
                WellKnownObjectMode.SingleCall);
        }
        
        /// <summary>
        /// Checks if this instance is already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Shuts down the IPC server.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

            ChannelServices.UnregisterChannel(ipcChannel);
            ipcChannel = null;
        }
    }
}
