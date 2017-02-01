using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// The different kinds of paths, selectable for a Bench environment transfer.
    /// </summary>
    [Flags]
    public enum TransferPaths : int
    {
        /// <summary>
        /// The Bench system files.
        /// </summary>
        System = 0x0000,

        /// <summary>
        /// The directory with the user configuration and the user app library.
        /// </summary>
        UserConfiguration = 0x0001,

        /// <summary>
        /// The home directory with the settings and user profile files.
        /// </summary>
        HomeDirectory = 0x0002,

        /// <summary>
        /// The directory with the projects.
        /// </summary>
        ProjectDirectory = 0x0004,

        /// <summary>
        /// The loaded app libraries.
        /// </summary>
        AppLibraries = 0x0030,

        /// <summary>
        /// The cached resources of the required apps.
        /// </summary>
        RequiredAppResourceCache = 0x0100,

        /// <summary>
        /// The cached resources of all apps.
        /// </summary>
        AppResourceCache = 0x0300,

        /// <summary>
        /// The required apps as cached and installed.
        /// </summary>
        RequiredApps = 0x1100,

        /// <summary>
        /// All apps as cached and installed.
        /// </summary>
        Apps = 0x3300,

        /// <summary>
        /// The complete Bench environment.
        /// </summary>
        All = 0xFFFF
    }
}
