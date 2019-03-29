using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.Bench.PropertyCollections;

namespace Mastersign.Bench
{
    internal class AppIndexDefaultValueSource : IGroupedPropertySource
    {
        public IConfiguration Config { get; set; }

        public IObjectLibrary AppIndex { get; set; }

        public AppIndexDefaultValueSource()
        {
        }

        public AppIndexDefaultValueSource(IConfiguration config, IObjectLibrary appIndex)
        {
            Config = config;
            AppIndex = appIndex;
        }

        public string GetGroupCategory(string group)
        {
            throw new NotSupportedException();
        }

        public object GetGroupMetadata(string group)
        {
            throw new NotSupportedException();
        }

        public string GetGroupDocumentation(string group)
        {
            throw new NotSupportedException();
        }

        private bool Use64Bit => Config.GetBooleanValue(ConfigPropertyKeys.Use64Bit);

        public object GetGroupValue(string appId, string key)
        {
            string appTyp;
            switch (key)
            {
                // Machine Architecure Switch

                case AppPropertyKeys.Version:
                case AppPropertyKeys.Dir:
                case AppPropertyKeys.Path:
                case AppPropertyKeys.Environment:
                case AppPropertyKeys.Exe:
                case AppPropertyKeys.AdornedExecutables:
                case AppPropertyKeys.RegistryKeys:
                case AppPropertyKeys.SetupTestFile:
                case AppPropertyKeys.ExeTestArguments:
                case AppPropertyKeys.LauncherExecutable:
                case AppPropertyKeys.LauncherArguments:
                case AppPropertyKeys.LauncherIcon:
                case AppPropertyKeys.Url:
                case AppPropertyKeys.DownloadHeaders:
                case AppPropertyKeys.DownloadCookies:
                case AppPropertyKeys.ResourceName:
                case AppPropertyKeys.ArchiveName:
                case AppPropertyKeys.ArchiveTyp:
                case AppPropertyKeys.ArchivePath:
                    return Use64Bit
                        ? AppIndex.GetGroupValue(appId, key + AppPropertyKeys.ARCH_64BIT_POSTFIX)
                        : AppIndex.GetGroupValue(appId, key + AppPropertyKeys.ARCH_32BIT_POSTFIX);
                
                // Common App Properties

                case AppPropertyKeys.Label:
                    return AppFacade.NameFromId(appId);
                case AppPropertyKeys.Typ:
                    return AppTyps.Default;
                case AppPropertyKeys.License:
                    return "unknown";
                case AppPropertyKeys.LicenseUrl:
                    var knownUrls = Config.GetValue(ConfigPropertyKeys.KnownLicenses) as IDictionary<string, string>;
                    if (knownUrls == null) return null;
                    var license = AppIndex.GetGroupValue(appId, AppPropertyKeys.License) as string;
                    if (string.IsNullOrEmpty(license)) return null;
                    string knownUrl;
                    return knownUrls.TryGetValue(license, out knownUrl) ? knownUrl : null;
                case AppPropertyKeys.Dir + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.Dir + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    appTyp = AppIndex.GetGroupValue(appId, AppPropertyKeys.Typ) as string;
                    switch (appTyp)
                    {
                        case AppTyps.NodePackage:
                            return AppIndex.GetGroupValue(AppKeys.NodeJS, AppPropertyKeys.Dir);
                        case AppTyps.RubyPackage:
                            return AppIndex.GetGroupValue(AppKeys.Ruby, AppPropertyKeys.Dir);
                        case AppTyps.PythonPackage:
                        case AppTyps.PythonWheel:
                            return File.Exists(AppIndex.GetStringGroupValue(AppKeys.Python3, AppPropertyKeys.Exe))
                                ? AppIndex.GetGroupValue(AppKeys.Python3, AppPropertyKeys.Dir)
                                : AppIndex.GetGroupValue(AppKeys.Python2, AppPropertyKeys.Dir);
                        case AppTyps.Python2Package:
                        case AppTyps.Python2Wheel:
                            return AppIndex.GetGroupValue(AppKeys.Python2, AppPropertyKeys.Dir);
                        case AppTyps.Python3Package:
                        case AppTyps.Python3Wheel:
                            return AppIndex.GetGroupValue(AppKeys.Python3, AppPropertyKeys.Dir);
                        case AppTyps.Meta:
                            return null;
                        case AppTyps.Group:
                            return null;
                        default:
                            return AppFacade.PathSegmentFromId(appId);
                    }
                case AppPropertyKeys.Path + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.Path + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    appTyp = AppIndex.GetGroupValue(appId, AppPropertyKeys.Typ) as string;
                    switch (appTyp)
                    {
                        case AppTyps.NodePackage:
                            return AppIndex.GetGroupValue(AppKeys.NodeJS, AppPropertyKeys.Path);
                        case AppTyps.RubyPackage:
                            return AppIndex.GetGroupValue(AppKeys.Ruby, AppPropertyKeys.Path);
                        case AppTyps.PythonPackage:
                        case AppTyps.PythonWheel:
                            return File.Exists(AppIndex.GetStringGroupValue(AppKeys.Python3, AppPropertyKeys.Exe))
                                ? Path.Combine(
                                    AppIndex.GetGroupValue(AppKeys.Python3, AppPropertyKeys.Dir) as string,
                                    "Scripts")
                                : Path.Combine(
                                    AppIndex.GetGroupValue(AppKeys.Python2, AppPropertyKeys.Dir) as string,
                                    "Scripts");
                        case AppTyps.Python2Package:
                        case AppTyps.Python2Wheel:
                            return Path.Combine(
                                AppIndex.GetGroupValue(AppKeys.Python2, AppPropertyKeys.Dir) as string,
                                "Scripts");
                        case AppTyps.Python3Package:
                        case AppTyps.Python3Wheel:
                            return Path.Combine(
                                AppIndex.GetGroupValue(AppKeys.Python3, AppPropertyKeys.Dir) as string,
                                "Scripts");
                        case AppTyps.NuGetPackage:
                            return Path.Combine(
                                Path.Combine(
                                    AppIndex.GetGroupValue(appId, AppPropertyKeys.Dir) as string,
                                    AppIndex.GetGroupValue(appId, AppPropertyKeys.PackageName) as string),
                                "tools");
                        default:
                            return AppIndex.GetGroupValue(appId, AppPropertyKeys.Dir);
                    }
                case AppPropertyKeys.Register:
                    return true;
                case AppPropertyKeys.Exe + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.Exe + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    appTyp = AppIndex.GetGroupValue(appId, AppPropertyKeys.Typ) as string;
                    if (appTyp == AppTyps.Default)
                    {
                        return Path.Combine(
                            AppIndex.GetGroupValue(appId, AppPropertyKeys.Dir) as string,
                            AppFacade.NameFromId(appId).ToLowerInvariant() + ".exe");
                    }
                    return null;
                case AppPropertyKeys.SetupTestFile + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.SetupTestFile + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    appTyp = AppIndex.GetGroupValue(appId, AppPropertyKeys.Typ) as string;
                    switch (appTyp)
                    {
                        case AppTyps.NuGetPackage:
                            return Path.Combine(
                                Path.Combine(
                                    AppIndex.GetGroupValue(appId, AppPropertyKeys.Dir) as string,
                                    AppIndex.GetGroupValue(appId, AppPropertyKeys.PackageName) as string),
                                AppIndex.GetGroupValue(appId, AppPropertyKeys.PackageName) + ".nupkg");
                        default:
                            return AppIndex.GetGroupValue(appId, AppPropertyKeys.Exe);
                    }
                case AppPropertyKeys.ExeTest:
                    return true;
                case AppPropertyKeys.LauncherExecutable + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.LauncherExecutable + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    return AppIndex.GetGroupValue(appId, AppPropertyKeys.Exe);
                case AppPropertyKeys.LauncherArguments + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.LauncherArguments + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    return new[] { "%*" };
                case AppPropertyKeys.LauncherWorkingDir:
                    return Config.GetStringValue(ConfigPropertyKeys.HomeDir);
                case AppPropertyKeys.LauncherIcon + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.LauncherIcon + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    return AppIndex.GetGroupValue(appId, AppPropertyKeys.LauncherExecutable);
                
                // Default App Properties

                case AppPropertyKeys.ArchiveTyp + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.ArchiveTyp + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    return AppArchiveTyps.Auto;
                case AppPropertyKeys.ArchivePath + AppPropertyKeys.ARCH_32BIT_POSTFIX:
                case AppPropertyKeys.ArchivePath + AppPropertyKeys.ARCH_64BIT_POSTFIX:
                    return string.Equals(
                        AppIndex.GetGroupValue(appId, AppPropertyKeys.ArchiveTyp) as string,
                        AppArchiveTyps.InnoSetup,
                        StringComparison.InvariantCultureIgnoreCase)
                        ? "{app}" : null;
                case AppPropertyKeys.PackageName:
                    return AppFacade.NameFromId(appId).ToLowerInvariant();
                default:
                    throw new NotSupportedException();
            }
        }

        public bool CanGetGroupValue(string group, string name)
        {
            return name == AppPropertyKeys.Typ
                || name == AppPropertyKeys.Label
                || name == AppPropertyKeys.Version
                || name == AppPropertyKeys.License
                || name == AppPropertyKeys.LicenseUrl
                || name == AppPropertyKeys.Dir
                || name == AppPropertyKeys.Dir + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.Dir + AppPropertyKeys.ARCH_64BIT_POSTFIX
                || name == AppPropertyKeys.Path
                || name == AppPropertyKeys.Path + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.Path + AppPropertyKeys.ARCH_64BIT_POSTFIX
                || name == AppPropertyKeys.Register
                || name == AppPropertyKeys.Environment
                || name == AppPropertyKeys.Exe
                || name == AppPropertyKeys.Exe + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.Exe + AppPropertyKeys.ARCH_64BIT_POSTFIX
                || name == AppPropertyKeys.AdornedExecutables
                || name == AppPropertyKeys.RegistryKeys
                || name == AppPropertyKeys.SetupTestFile
                || name == AppPropertyKeys.SetupTestFile + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.SetupTestFile + AppPropertyKeys.ARCH_64BIT_POSTFIX
                || name == AppPropertyKeys.ExeTest
                || name == AppPropertyKeys.ExeTestArguments
                || name == AppPropertyKeys.LauncherExecutable
                || name == AppPropertyKeys.LauncherExecutable + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.LauncherExecutable + AppPropertyKeys.ARCH_64BIT_POSTFIX
                || name == AppPropertyKeys.LauncherArguments
                || name == AppPropertyKeys.LauncherArguments + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.LauncherArguments + AppPropertyKeys.ARCH_64BIT_POSTFIX
                || name == AppPropertyKeys.LauncherWorkingDir
                || name == AppPropertyKeys.LauncherIcon
                || name == AppPropertyKeys.LauncherIcon + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.LauncherIcon + AppPropertyKeys.ARCH_64BIT_POSTFIX

                || name == AppPropertyKeys.Url
                || name == AppPropertyKeys.DownloadHeaders
                || name == AppPropertyKeys.DownloadCookies
                || name == AppPropertyKeys.ResourceName
                || name == AppPropertyKeys.ArchiveName
                || name == AppPropertyKeys.ArchiveTyp
                || name == AppPropertyKeys.ArchiveTyp + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.ArchiveTyp + AppPropertyKeys.ARCH_64BIT_POSTFIX
                || name == AppPropertyKeys.ArchivePath
                || name == AppPropertyKeys.ArchivePath + AppPropertyKeys.ARCH_32BIT_POSTFIX
                || name == AppPropertyKeys.ArchivePath + AppPropertyKeys.ARCH_64BIT_POSTFIX

                || name == AppPropertyKeys.PackageName
                ;
        }
    }
}
