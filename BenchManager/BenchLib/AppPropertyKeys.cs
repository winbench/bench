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
#pragma warning disable CS1591 // warning for missing XML doc comment

        // Common App Properties

        public const string IsActive = "IsActive";
        public const string IsActivated = "IsActivated";
        public const string IsDeactivated = "IsDeactivated";
        public const string IsRequired = "IsRequired";
        public const string IsDependency = "IsDependency";
        public const string Label = "Label";
        public const string Typ = "Typ";
        public const string Version = "Version";
        public const string InstalledVersion = "InstalledVersion";
        public const string Website = "Website";
        public const string Docs = "Docs";
        public const string License = "License";
        public const string LicenseUrl = "LicenseUrl";
        public const string Dependencies = "Dependencies";
        public const string Responsibilities = "Responsibilities";
        public const string Force = "Force";
        public const string Dir = "Dir";
        public const string Path = "Path";
        public const string Register = "Register";
        public const string Environment = "Environment";
        public const string Exe = "Exe";
        public const string AdornedExecutables = "AdornedExecutables";
        public const string RegistryKeys = "RegistryKeys";
        public const string ExeTest = "ExeTest";
        public const string ExeTestArguments = "ExeTestArguments";
        public const string Launcher = "Launcher";
        public const string LauncherExecutable = "LauncherExecutable";
        public const string LauncherArguments = "LauncherArguments";
        public const string LauncherIcon = "LauncherIcon";

        // Default App Properties

        public const string HasResource = "HasResource";
        public const string IsResourceCached = "IsResourceCached";
        public const string Url = "Url";
        public const string DownloadHeaders = "DownloadHeaders";
        public const string DownloadCookies = "DownloadCookies";
        public const string ResourceName = "ResourceName";
        public const string ArchiveName = "ArchiveName";
        public const string ArchiveTyp = "ArchiveTyp";
        public const string ArchivePath = "ArchivePath";
        public const string SetupTestFile = "SetupTestFile";

        // Package App Properties

        public const string PackageName = "PackageName";

#pragma warning restore CS1591 // warning for missing XML doc comment
    }
}
