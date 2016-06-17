using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench
{
    public class AppIndexDefaultValueSource : IGroupedPropertySource
    {
        public IGroupedPropertySource AppIndex { get; set; }

        public AppIndexDefaultValueSource()
        {
        }

        public AppIndexDefaultValueSource(IGroupedPropertySource appIndex)
        {
            AppIndex = appIndex;
        }

        public object GetGroupValue(string appName, string key)
        {
            string appTyp;
            switch (key)
            {
                case PropertyKeys.AppTyp:
                    return AppTyps.Default;
                case PropertyKeys.AppArchiveTyp:
                    return AppArchiveTyps.Auto;
                case PropertyKeys.AppPackageName:
                    return appName.ToLowerInvariant();
                case PropertyKeys.AppDir:
                    appTyp = AppIndex.GetGroupValue(appName, PropertyKeys.AppTyp) as string;
                    switch (appTyp)
                    {
                        case AppTyps.NodePackage:
                            return AppIndex.GetGroupValue(AppKeys.Npm, PropertyKeys.AppDir);
                        case AppTyps.RubyPackage:
                            return AppIndex.GetGroupValue(AppKeys.Ruby, PropertyKeys.AppDir);
                        case AppTyps.Python2Package:
                            return AppIndex.GetGroupValue(AppKeys.Python2, PropertyKeys.AppDir);
                        case AppTyps.Python3Package:
                            return AppIndex.GetGroupValue(AppKeys.Python3, PropertyKeys.AppDir);
                        case AppTyps.Meta:
                            return null;
                        default:
                            return appName.ToLowerInvariant();
                    }
                case PropertyKeys.AppPath:
                    appTyp = AppIndex.GetGroupValue(appName, PropertyKeys.AppTyp) as string;
                    switch (appTyp)
                    {
                        case AppTyps.NodePackage:
                            return AppIndex.GetGroupValue(AppKeys.Npm, PropertyKeys.AppPath);
                        case AppTyps.RubyPackage:
                            return AppIndex.GetGroupValue(AppKeys.Ruby, PropertyKeys.AppPath);
                        case AppTyps.Python2Package:
                            return Path.Combine(
                                AppIndex.GetGroupValue(AppKeys.Python2, PropertyKeys.AppDir) as string,
                                "Scripts");
                        case AppTyps.Python3Package:
                            return Path.Combine(
                                AppIndex.GetGroupValue(AppKeys.Python3, PropertyKeys.AppDir) as string,
                                "Scripts");
                        default:
                            return AppIndex.GetGroupValue(appName, PropertyKeys.AppDir);
                    }
                case PropertyKeys.AppExe:
                    appTyp = AppIndex.GetGroupValue(appName, PropertyKeys.AppTyp) as string;
                    if (appTyp == AppTyps.Default)
                    {
                        return Path.Combine(
                            AppIndex.GetGroupValue(appName, PropertyKeys.AppDir) as string,
                            appName.ToLowerInvariant() + ".exe");
                    }
                    return null;
                case PropertyKeys.AppRegister:
                    return true;
                case PropertyKeys.AppLauncherExecutable:
                    return AppIndex.GetGroupValue(appName, PropertyKeys.AppExe);
                case PropertyKeys.AppLauncherArguments:
                    return new[] { "%*" };
                case PropertyKeys.AppLauncherIcon:
                    return AppIndex.GetGroupValue(appName, PropertyKeys.AppLauncherExecutable);
                case PropertyKeys.AppSetupTestFile:
                    return AppIndex.GetGroupValue(appName, PropertyKeys.AppExe);
                default:
                    throw new NotSupportedException();
            }
        }

        public bool CanGetGroupValue(string group, string name)
        {
            return name == PropertyKeys.AppTyp
                || name == PropertyKeys.AppArchiveTyp
                || name == PropertyKeys.AppPackageName
                || name == PropertyKeys.AppDir
                || name == PropertyKeys.AppPath
                || name == PropertyKeys.AppExe
                || name == PropertyKeys.AppRegister
                || name == PropertyKeys.AppLauncherExecutable
                || name == PropertyKeys.AppLauncherArguments
                || name == PropertyKeys.AppLauncherIcon
                || name == PropertyKeys.AppSetupTestFile;
        }
    }
}
