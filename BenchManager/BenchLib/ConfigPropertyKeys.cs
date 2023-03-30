using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class contains string constants for all configuration properties.
    /// </summary>
    public static class ConfigPropertyKeys
    {
#pragma warning disable CS1591 // warning for missing XML doc comment

        public const string Version = "Version";
        public const string VersionFile = "VersionFile";
        public const string VersionUrl = "VersionUrl";
        public const string AutoUpdateCheck = "AutoUpdateCheck";
        public const string UpdateUrlTemplate = "UpdateUrlTemplate";
        public const string BootstrapUrlTemplate = "BootstrapUrlTemplate";
        public const string Website = "Website";

        public const string UserName = "UserName";
        public const string UserEmail = "UserEmail";

        public const string BenchDrive = "BenchDrive";
        public const string BenchRoot = "BenchRoot";
        public const string BenchAuto = "BenchAuto";
        public const string BenchBin = "BenchBin";
        public const string BenchScripts = "BenchScripts";

        public const string UserConfigDir = "UserConfigDir";
        public const string UserConfigFile = "UserConfigFile";
        public const string UserConfigTemplateFile = "UserConfigTemplateFile";
        public const string UserConfigRepository = "UserConfigRepository";
        public const string UserConfigInitDirectory = "UserConfigInitDirectory";
        public const string UserConfigInitZipFile = "UserConfigInitZipFile";
        public const string SiteConfigFileName = "SiteConfigFileName";
        public const string SiteConfigTemplateFile = "SiteConfigTemplateFile";

        public const string UserAppIndexTemplateFile = "UserAppIndexTemplateFile";
        public const string AppActivationFile = "AppActivationFile";
        public const string AppActivationTemplateFile = "AppActivationTemplateFile";
        public const string AppDeactivationFile = "AppDeactivationFile";
        public const string AppDeactivationTemplateFile = "AppDeactivationTemplateFile";
        public const string AppsVersionIndexDir = "AppsVersionIndexDir";

        public const string WizzardIntegrateIntoUserProfile = "WizzardIntegrateIntoUserProfile";
        public const string WizzardStartAutoSetup = "WizzardStartAutoSetup";
        public const string WizzardApps = "WizzardApps";
        public const string WizzardSelectedApps = "WizzardSelectedApps";
        public const string QuickAccessCmd = "QuickAccessCmd";
        public const string QuickAccessPowerShell = "QuickAccessPowerShell";
        public const string QuickAccessPowerShellCore = "QuickAccessPowerShellCore";
        public const string QuickAccessBash = "QuickAccessBash";

        public const string DashboardSetupAppListColumns = "DashboardSetupAppListColumns";
        public const string DashboardSavePositions = "DashboardSavePositions";
        public const string DashboardMainPosition = "DashboardMainPosition";
        public const string DashboardSetupPosition = "DashboardSetupPosition";
        public const string DashboardMarkdownViewerPosition = "DashboardMarkdownViewerPosition";

        public const string ConEmuConfigFile = "ConEmuConfigFile";
        public const string ConEmuConfigTemplateFile = "ConEmuConfigTemplateFile";

        public const string TempDir = "TempDir";
        public const string CacheDir = "CacheDir";
        public const string AppsCacheDir = "AppsCacheDir";
        public const string AppsInstallDir = "AppsInstallDir";
        public const string AppLibs = "AppLibs";
        public const string AppLibsInstallDir = "AppLibsInstallDir";
        public const string AppLibsCacheDir = "AppLibsCacheDir";
        public const string AppLibIndexFileName = "AppLibIndexFileName";
        public const string AppLibCustomScriptDirName = "AppLibCustomScriptDirName";
        public const string AppLibResourceDirName = "AppLibResourceDirName";
        public const string KnownLicenses = "KnownLicenses";
        public const string LogDir = "LogDir";
        public const string LogFile = "LogFile";
        public const string LogLevel = "LogLevel";
        public const string AppsAdornmentBaseDir = "AppsAdornmentBaseDir";
        public const string AppsRegistryBaseDir = "AppsRegistryBaseDir";
        public const string LauncherDir = "LauncherDir";
        public const string LauncherScriptDir = "LauncherScriptDir";
        public const string HomeDir = "HomeDir";
        public const string AppDataDir = "AppDataDir";
        public const string LocalAppDataDir = "LocalAppDataDir";
        public const string OverrideHome = "OverrideHome";
        public const string OverrideTemp = "OverrideTemp";
        public const string IgnoreSystemPath = "IgnoreSystemPath";
        public const string RegisterInUserProfile = "RegisterInUserProfile";
        public const string UseRegistryIsolation = "UseRegistryIsolation";
        public const string CustomPath = "EnvironmentPath";
        public const string CustomEnvironment = "Environment";
        public const string Allow64Bit = "Allow64Bit";
        public const string Use64Bit = "Use64Bit";

        public const string ProjectRootDir = "ProjectRootDir";
        public const string ProjectArchiveDir = "ProjectArchiveDir";
        public const string ProjectArchiveFormat = "ProjectArchiveFormat";

        public const string UseProxy = "UseProxy";
        public const string ProxyBypass = "ProxyBypass";
        public const string HttpProxy = "HttpProxy";
        public const string HttpsProxy = "HttpsProxy";
        public const string HttpsSecurityProtocols = "HttpsSecurityProtocols";
        public const string DownloadAttempts = "DownloadAttempts";
        public const string ParallelDownloads = "ParallelDownloads";

        public const string TextEditorApp = "TextEditorApp";
        public const string MarkdownEditorApp = "MarkdownEditorApp";

#pragma warning restore CS1591 // warning for missing XML doc comment
    }
}
