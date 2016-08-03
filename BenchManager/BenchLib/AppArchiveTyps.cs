using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This static class contains string constants for the supported archive typs.
    /// </summary>
    public static class AppArchiveTyps
    {
        /// <summary>Automatic detection of the archive typ.</summary>
        public const string Auto = "auto";

        /// <summary>A generic archive supported by 7-Zip.</summary>
        public const string Generic = "generic";

        /// <summary>An MSI package, supported by LessMSI.</summary>
        public const string Msi = "msi";

        /// <summary>An Inno Setup Program, supported by Inno Setup Unpacker.</summary>
        public const string InnoSetup = "inno";

        /// <summary>A custom archive, which will be extracted by a custom extraction script.</summary>
        public const string Custom = "custom";
    }
}
