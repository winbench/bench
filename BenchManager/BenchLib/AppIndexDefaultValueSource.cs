using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench
{
    internal class AppIndexDefaultValueSource : IGroupedPropertySource
    {
        public IGroupedPropertySource AppIndex { get; set; }

        public AppIndexDefaultValueSource()
        {
        }

        public AppIndexDefaultValueSource(IGroupedPropertySource appIndex)
        {
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

        public object GetGroupValue(string appId, string key)
        {
            string appTyp;
            switch (key)
            {
                case AppPropertyKeys.Label:
                    return AppFacade.NameFromId(appId);
                case AppPropertyKeys.Typ:
                    return AppTyps.Default;
                case AppPropertyKeys.License:
                    return "unknown";
                case AppPropertyKeys.LicenseUrl:
                    var knownUrls = AppIndex.GetGroupValue(null, ConfigPropertyKeys.KnownLicenses) as IDictionary<string, string>;
                    if (knownUrls == null) return null;
                    var license = AppIndex.GetGroupValue(appId, AppPropertyKeys.License) as string;
                    if (string.IsNullOrEmpty(license)) return null;
                    string knownUrl;
                    return knownUrls.TryGetValue(license, out knownUrl) ? knownUrl : null;
                case AppPropertyKeys.ArchiveTyp:
                    return AppArchiveTyps.Auto;
                case AppPropertyKeys.ArchivePath:
                    return string.Equals(
                        AppIndex.GetGroupValue(appId, AppPropertyKeys.ArchiveTyp) as string,
                        AppArchiveTyps.InnoSetup,
                        StringComparison.InvariantCultureIgnoreCase)
                        ? "{app}" : null;
                case AppPropertyKeys.PackageName:
                    return AppFacade.NameFromId(appId).ToLowerInvariant();
                case AppPropertyKeys.Dir:
                    appTyp = AppIndex.GetGroupValue(appId, AppPropertyKeys.Typ) as string;
                    switch (appTyp)
                    {
                        case AppTyps.NodePackage:
                            return AppIndex.GetGroupValue(AppKeys.Npm, AppPropertyKeys.Dir);
                        case AppTyps.RubyPackage:
                            return AppIndex.GetGroupValue(AppKeys.Ruby, AppPropertyKeys.Dir);
                        case AppTyps.Python2Package:
                            return AppIndex.GetGroupValue(AppKeys.Python2, AppPropertyKeys.Dir);
                        case AppTyps.Python3Package:
                            return AppIndex.GetGroupValue(AppKeys.Python3, AppPropertyKeys.Dir);
                        case AppTyps.Meta:
                            return null;
                        default:
                            return AppFacade.PathSegmentFromId(appId);
                    }
                case AppPropertyKeys.Path:
                    appTyp = AppIndex.GetGroupValue(appId, AppPropertyKeys.Typ) as string;
                    switch (appTyp)
                    {
                        case AppTyps.NodePackage:
                            return AppIndex.GetGroupValue(AppKeys.Npm, AppPropertyKeys.Path);
                        case AppTyps.RubyPackage:
                            return AppIndex.GetGroupValue(AppKeys.Ruby, AppPropertyKeys.Path);
                        case AppTyps.Python2Package:
                            return Path.Combine(
                                AppIndex.GetGroupValue(AppKeys.Python2, AppPropertyKeys.Dir) as string,
                                "Scripts");
                        case AppTyps.Python3Package:
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
                case AppPropertyKeys.Exe:
                    appTyp = AppIndex.GetGroupValue(appId, AppPropertyKeys.Typ) as string;
                    if (appTyp == AppTyps.Default)
                    {
                        return Path.Combine(
                            AppIndex.GetGroupValue(appId, AppPropertyKeys.Dir) as string,
                            AppFacade.NameFromId(appId).ToLowerInvariant() + ".exe");
                    }
                    return null;
                case AppPropertyKeys.ExeTest:
                    return true;
                case AppPropertyKeys.Register:
                    return true;
                case AppPropertyKeys.LauncherExecutable:
                    return AppIndex.GetGroupValue(appId, AppPropertyKeys.Exe);
                case AppPropertyKeys.LauncherArguments:
                    return new[] { "%*" };
                case AppPropertyKeys.LauncherIcon:
                    return AppIndex.GetGroupValue(appId, AppPropertyKeys.LauncherExecutable);
                case AppPropertyKeys.SetupTestFile:
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
                default:
                    throw new NotSupportedException();
            }
        }

        public bool CanGetGroupValue(string group, string name)
        {
            return name == AppPropertyKeys.Typ
                || name == AppPropertyKeys.Label
                || name == AppPropertyKeys.License
                || name == AppPropertyKeys.LicenseUrl
                || name == AppPropertyKeys.ArchiveTyp
                || name == AppPropertyKeys.ArchivePath
                || name == AppPropertyKeys.PackageName
                || name == AppPropertyKeys.Dir
                || name == AppPropertyKeys.Path
                || name == AppPropertyKeys.Exe
                || name == AppPropertyKeys.ExeTest
                || name == AppPropertyKeys.Register
                || name == AppPropertyKeys.LauncherExecutable
                || name == AppPropertyKeys.LauncherArguments
                || name == AppPropertyKeys.LauncherIcon
                || name == AppPropertyKeys.SetupTestFile;
        }
    }
}
