using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// A Bench manager is an object which knows the most important components of a Bench system.
    /// </summary>
    public interface IBenchManager
    {
        /// <summary>
        /// The configuration of the Bench system.
        /// </summary>
        BenchConfiguration Config { get; }

        /// <summary>
        /// The environment variables of the Bench system.
        /// </summary>
        BenchEnvironment Env { get; }

        /// <summary>
        /// The downloader for downloading app resources.
        /// </summary>
        Downloader Downloader { get; }

        /// <summary>
        /// The user interface to communicate with the user.
        /// </summary>
        IUserInterface UI { get; }

        /// <summary>
        /// The host for starting and running Windows processes.
        /// </summary>
        IProcessExecutionHost ProcessExecutionHost { get; }
    }
}
