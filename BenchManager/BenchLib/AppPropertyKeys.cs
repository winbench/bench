using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class contains string constants for all app properties.
    /// </summary>
    public static class AppPropertyKeys
    {
        /// <summary>
        /// The postfix to property names, defining a value specific to 32Bit systems.
        /// </summary>
        public const string ARCH_32BIT_POSTFIX = "32Bit";

        /// <summary>
        /// The postfix to property names, defining a value specific to 64Bit systems.
        /// </summary>
        public const string ARCH_64BIT_POSTFIX = "64Bit";

#pragma warning disable CS1591 // warning for missing XML doc comment

        // Common App Properties

        public const string IsActive = "IsActive";
        public const string IsActivated = "IsActivated";
        public const string IsDeactivated = "IsDeactivated";
        public const string IsSupported = "IsSupported";
        public const string IsRequired = "IsRequired";
        public const string IsDependency = "IsDependency";
        public const string Label = "Label";
        public const string Typ = "Typ";
        public const string Version = "Version"; // supports architecture alternatives
        public const string InstalledVersion = "InstalledVersion";
        public const string Website = "Website";
        public const string Docs = "Docs";
        public const string License = "License";
        public const string LicenseUrl = "LicenseUrl";
        public const string Dependencies = "Dependencies";
        public const string Responsibilities = "Responsibilities";
        public const string Force = "Force";
        public const string Dir = "Dir"; // supports architecture alternatives
        public const string Path = "Path"; // supports architecture alternatives
        public const string Register = "Register";
        public const string Environment = "Environment"; // supports architecture alternatives
        public const string Exe = "Exe"; // supports architecture alternatives
        public const string AdornedExecutables = "AdornedExecutables"; // supports architecture alternatives
        public const string RegistryKeys = "RegistryKeys"; // supports architecture alternatives
        public const string Only64Bit = "Only64Bit";
        public const string SetupTestFile = "SetupTestFile"; // supports architecture alternatives
        public const string ExeTest = "ExeTest";
        public const string ExeTestArguments = "ExeTestArguments"; // supports architecture alternatives
        public const string Launcher = "Launcher";
        public const string LauncherExecutable = "LauncherExecutable"; // supports architecture alternatives
        public const string LauncherArguments = "LauncherArguments"; // supports architecture alternatives
        public const string LauncherWorkingDir = "LauncherWorkingDir";
        public const string LauncherIcon = "LauncherIcon"; // supports architecture alternatives

        // Default App Properties

        public const string HasResource = "HasResource";
        public const string IsResourceCached = "IsResourceCached";
        public const string Url = "Url"; // supports architecture alternatives
        public const string DownloadHeaders = "DownloadHeaders"; // supports architecture alternatives
        public const string DownloadCookies = "DownloadCookies"; // supports architecture alternatives
        public const string ResourceName = "ResourceName"; // supports architecture alternatives
        public const string ArchiveName = "ArchiveName"; // supports architecture alternatives
        public const string ArchiveTyp = "ArchiveTyp"; // supports architecture alternatives
        public const string ArchivePath = "ArchivePath"; // supports architecture alternatives

        // Package App Properties

        public const string PackageName = "PackageName";

#pragma warning restore CS1591 // warning for missing XML doc comment
    }
}
