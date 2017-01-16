using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading;

namespace Mastersign.Bench.RemoteExecHost
{
    /// <summary>
    /// A client to communicate via IPC with a remote execution host.
    /// </summary>
    public class RemoteExecHostClient : IDisposable
    {
        private IpcChannel ipcChannel;

        private IRemoteExecHost execHost;

        /// <summary>
        /// Initialize a new instance of <see cref="RemoteExecHostClient"/>.
        /// </summary>
        /// <param name="token">A unique string to identify the server.</param>
        public RemoteExecHostClient(string token)
        {
            ipcChannel = new IpcChannel("Bench_ExecHost_Client_" + token);
            ChannelServices.RegisterChannel(ipcChannel, false);

            execHost = (IRemoteExecHost)Activator.GetObject(
                typeof(IRemoteExecHost),
                "ipc://Bench_ExecHost_" + token + "/RemoteExecHost");
        }

        /// <summary>
        /// The <see cref="IRemoteExecHost"/> proxy object to communicate with the server.
        /// </summary>
        public IRemoteExecHost ExecHost => execHost;

        /// <summary>
        /// Checks if this instance is already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

            execHost = null;
            ChannelServices.UnregisterChannel(ipcChannel);
            ipcChannel = null;
        }
    }
}
