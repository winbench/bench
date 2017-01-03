using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// The different kinds of configuration and app library files.
    /// </summary>
    [Flags]
    public enum ConfigurationFileType : int
    {
        #region File Groups

        /// <summary>All kind of configuration and app library files</summary>
        All = 0xFFFF,

        /// <summary>All configuration files (Bench, User, Site)</summary>
        Config = 0x000F,

        /// <summary>All app library index files</summary>
        AppLib = 0x00F0,

        /// <summary>All app selection lists (Activation, Deactivation)</summary>
        AppSelection = 0x0F00,

        #endregion

        #region Specific File Types

        /// <summary>The built-in Bench configuration file</summary>
        BenchConfig = 0x0001,

        /// <summary>The user configuration file</summary>
        UserConfig = 0x0002,

        /// <summary>A site configuration file</summary>
        SiteConfig = 0x0004,

        /// <summary>An index file of a loaded app library</summary>
        BenchAppLib = 0x0010,

        /// <summary>The index file of the user app library</summary>
        UserAppLib = 0x0020,

        /// <summary>The app activation list file</summary>
        Activation = 0x0100,

        /// <summary>The app deactivation list file</summary>
        Deactivation = 0x0200,

        #endregion
    }
}
