using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

using IOPath = System.IO.Path;

namespace Mastersign.Bench
{
    public class AppFacade
    {
        private readonly IConfiguration AppIndex;

        private readonly string AppName;

        public AppFacade(IConfiguration source, string appName)
        {
            AppIndex = source;
            AppName = appName;
        }

        public void LoadCachedValues()
        {
            isInstalled = GetIsInstalled();
            isResourceCached = GetIsResourceCached();
        }

        public void DiscardCachedValues()
        {
            isInstalled = null;
            isResourceCached = null;
        }

        private object Value(string property)
        {
            return AppIndex.GetGroupValue(AppName, property);
        }

        private string StringValue(string property)
        {
            return AppIndex.GetStringGroupValue(AppName, property);
        }

        private bool BoolValue(string property)
        {
            return AppIndex.GetBooleanGroupValue(AppName, property);
        }

        private int IntValue(string property)
        {
            return AppIndex.GetInt32GroupValue(AppName, property);
        }

        private string[] ListValue(string property)
        {
            var value = Value(property);
            if (value is string) return new[] { (string)value };
            return (value as string[]) ?? new string[0];
        }

        private void UpdateValue(string property, object value)
        {
            AppIndex.SetGroupValue(AppName, property, value);
        }

        public string ID { get { return AppName; } }

        public string Category { get { return AppIndex.GetGroupCategory(AppName); } }

        public string Typ { get { return StringValue(PropertyKeys.AppTyp); } }

        public bool IsManagedPackage
        {
            get
            {
                var typ = Typ;
                return typ == AppTyps.NodePackage
                    || typ == AppTyps.RubyPackage
                    || typ == AppTyps.Python2Package
                    || typ == AppTyps.Python3Package;
            }
        }

        public string Version { get { return StringValue(PropertyKeys.AppVersion); } }

        public bool IsVersioned
        {
            get
            {
                return Version != null && !Version.Equals("latest", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public string Website { get { return StringValue(PropertyKeys.AppWebsite); } }

        public string[] Dependencies
        {
            get { return ListValue(PropertyKeys.AppDependencies); }
            set { UpdateValue(PropertyKeys.AppDependencies, value); }
        }

        public string[] Responsibilities
        {
            get { return ListValue(PropertyKeys.AppResponsibilities); }
            set { UpdateValue(PropertyKeys.AppResponsibilities, value); }
        }

        public bool IsActivated { get { return BoolValue(PropertyKeys.AppIsActivated); } }

        public bool IsDeactivated { get { return BoolValue(PropertyKeys.AppIsDeactivated); } }

        public bool IsRequired { get { return BoolValue(PropertyKeys.AppIsRequired); } }

        public bool IsDependency { get { return BoolValue(PropertyKeys.AppIsDependency); } }

        public string Url { get { return StringValue(PropertyKeys.AppUrl); } }

        public IDictionary<string, string> DownloadHeaders
        {
            get { return Value(PropertyKeys.AppDownloadHeaders) as IDictionary<string, string>; }
        }

        public IDictionary<string, string> DownloadCookies
        {
            get { return Value(PropertyKeys.AppDownloadCookies) as IDictionary<string, string>; }
        }

        public string ResourceFileName { get { return StringValue(PropertyKeys.AppResourceName); } }

        public string ResourceArchiveName { get { return StringValue(PropertyKeys.AppArchiveName); } }

        public string ResourceArchivePath { get { return StringValue(PropertyKeys.AppArchivePath); } }

        public string ResourceArchiveTyp { get { return StringValue(PropertyKeys.AppArchiveTyp); } }

        public bool Force
        {
            get { return BoolValue(PropertyKeys.AppForce); }
            set { UpdateValue(PropertyKeys.AppForce, value); }
        }

        public string PackageName { get { return StringValue(PropertyKeys.AppPackageName); } }

        public string Dir { get { return StringValue(PropertyKeys.AppDir); } }

        public string Exe { get { return StringValue(PropertyKeys.AppExe); } }

        public string SetupTestFile { get { return StringValue(PropertyKeys.AppSetupTestFile); } }

        public string[] Path
        {
            get { return ListValue(PropertyKeys.AppPath); }
            set { UpdateValue(PropertyKeys.AppPath, value); }
        }

        public bool Register { get { return BoolValue(PropertyKeys.AppRegister); } }

        public IDictionary<string, string> Environment
        {
            get { return Value(PropertyKeys.AppEnvironment) as IDictionary<string, string>; }
        }

        public string[] AdornedExecutables
        {
            get { return ListValue(PropertyKeys.AppAdornedExecutables); }
            set { UpdateValue(PropertyKeys.AppAdornedExecutables, value); }
        }

        public void AddAdornedExecutable(string path)
        {
            AdornedExecutables = AddToSet(AdornedExecutables, path);
        }

        public void RemoveAdornedExecutable(string path)
        {
            AdornedExecutables = RemoveFromSet(AdornedExecutables, path);
        }

        public string AdornmentProxyBasePath
        {
            get
            {
                return IOPath.Combine(
                    AppIndex.GetStringValue(PropertyKeys.AppAdornmentBaseDir),
                    ID.ToLowerInvariant());
            }
        }

        public bool IsExecutableAdorned(string exePath)
        {
            if (!IOPath.IsPathRooted(exePath))
            {
                exePath = IOPath.Combine(Dir, exePath);
            }
            foreach (var p in AdornedExecutables)
            {
                var adornedPath = IOPath.IsPathRooted(p) ? p : IOPath.Combine(Dir, p);
                if (exePath.Equals(adornedPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetExecutableProxy(string exePath)
        {
            return IOPath.Combine(AdornmentProxyBasePath,
                IOPath.GetFileNameWithoutExtension(exePath) + ".cmd");
        }

        public string[] RegistryKeys { get { return ListValue(PropertyKeys.AppRegistryKeys); } }

        public string Launcher { get { return StringValue(PropertyKeys.AppLauncher); } }

        public string LauncherExecutable { get { return StringValue(PropertyKeys.AppLauncherExecutable); } }

        public string[] LauncherArguments { get { return ListValue(PropertyKeys.AppLauncherArguments); } }

        public string LauncherIcon { get { return StringValue(PropertyKeys.AppLauncherIcon); } }

        #region Recursive Discovery

        public IList<string> FindAllDependencies()
        {
            return FindAppsRecursively(PropertyKeys.AppDependencies);
        }

        public IList<string> FindAllResponsibilities()
        {
            return FindAppsRecursively(PropertyKeys.AppResponsibilities);
        }

        private IList<string> FindAppsRecursively(string listPropertyName)
        {
            // HashSet<string> would be more appropriate, but it is not contained in .NET 2.0
            var hashset = new Dictionary<string, bool>();
            hashset.Add(ID, false);
            TrackIdList(ID, listPropertyName, hashset);
            var result = new List<string>();
            foreach (var appName in AppIndex.Groups())
            {
                if (hashset.ContainsKey(appName)) result.Add(appName);
            }
            return result;
        }

        private void TrackIdList(string appName, string listPropertyName, IDictionary<string, bool> hashset)
        {
            foreach (var dependency in AppIndex.GetStringListGroupValue(appName, listPropertyName))
            {
                if (hashset.ContainsKey(dependency)) continue;
                hashset.Add(dependency, true);
                TrackIdList(dependency, listPropertyName, hashset);
            }
        }

        #endregion

        #region File Discovery

        public string GetLauncherFile()
        {
            var launcher = Launcher;
            return launcher != null
                ? IOPath.Combine(
                    AppIndex.GetStringValue(PropertyKeys.LauncherDir),
                    launcher + ".lnk")
                : null;
        }

        public string GetLauncherScriptFile()
        {
            return IOPath.Combine(
                AppIndex.GetStringValue(PropertyKeys.LauncherScriptDir),
                ID.ToLowerInvariant() + ".cmd");
        }

        public string GetCustomScriptFile(string typ)
        {
            var path = IOPath.Combine(
                IOPath.Combine(AppIndex.GetStringValue(PropertyKeys.BenchAuto), "apps"),
                ID.ToLowerInvariant() + "." + typ + ".ps1");
            return File.Exists(path) ? path : null;
        }

        #endregion

        #region Status

        public bool IsActive
        {
            get
            {
                return !IsDeactivated
                    && (IsRequired || IsDependency || IsActivated);
            }
        }

        public bool HasResource
        {
            get
            {
                return Typ == AppTyps.Default &&
                    (ResourceFileName != null || ResourceArchiveName != null);
            }
        }

        public bool CanCheckInstallation
        {
            get
            {
                return SetupTestFile != null
                    || Typ == AppTyps.NodePackage
                    || Typ == AppTyps.RubyPackage
                    || Typ == AppTyps.Python2Package
                    || Typ == AppTyps.Python3Package;
            }
        }

        private bool? isInstalled;

        private bool GetIsInstalled()
        {
            switch (Typ)
            {
                case AppTyps.NodePackage:
                    var npmDir = AppIndex.GetStringGroupValue(AppKeys.Npm, PropertyKeys.AppDir);
                    var npmPackageDir = IOPath.Combine(
                        IOPath.Combine(npmDir, "node_modules"),
                        PackageName);
                    return Directory.Exists(npmPackageDir);
                case AppTyps.RubyPackage:
                    var rubyDir = AppIndex.GetStringGroupValue(AppKeys.Ruby, PropertyKeys.AppDir);
                    var rubyVersion = new Version(AppIndex.GetStringGroupValue(AppKeys.Ruby, PropertyKeys.AppVersion));
                    var generalVersion = new Version(rubyVersion.Major, rubyVersion.Minor, 0);
                    var generalVersionStr = generalVersion.ToString(3);
                    var gemDirBase = IOPath.Combine(rubyDir,
                        string.Format(@"lib\ruby\gems\{0}\gems", generalVersionStr));
                    if (!Directory.Exists(gemDirBase)) return false;
                    var folders = Directory.GetDirectories(gemDirBase, PackageName + "-*");
                    return folders.Length > 0;
                case AppTyps.Python2Package:
                    var python2Dir = AppIndex.GetStringGroupValue(AppKeys.Python2, PropertyKeys.AppDir);
                    var pip2PackageDir = IOPath.Combine(
                        IOPath.Combine(python2Dir, "lib"),
                        IOPath.Combine("site-packages", PackageName));
                    return Directory.Exists(pip2PackageDir);
                case AppTyps.Python3Package:
                    var python3Dir = AppIndex.GetStringGroupValue(AppKeys.Python3, PropertyKeys.AppDir);
                    var pip3PackageDir = IOPath.Combine(
                        IOPath.Combine(python3Dir, "lib"),
                        IOPath.Combine("site-packages", PackageName));
                    return Directory.Exists(pip3PackageDir);
                default:
                    return File.Exists(SetupTestFile);
            }
        }

        public bool IsInstalled
        {
            get
            {
                if (!isInstalled.HasValue) isInstalled = GetIsInstalled();
                return isInstalled.Value;
            }
        }

        private bool? isResourceCached;

        private bool GetIsResourceCached()
        {
            switch (Typ)
            {
                case AppTyps.Default:
                    return ResourceFileName != null
                        ? File.Exists(IOPath.Combine(AppIndex.GetStringValue(PropertyKeys.DownloadDir), ResourceFileName))
                        : ResourceArchiveName != null
                            ? File.Exists(IOPath.Combine(AppIndex.GetStringValue(PropertyKeys.DownloadDir), ResourceArchiveName))
                            : true;
                default:
                    return false;
            }
        }

        public bool IsResourceCached
        {
            get
            {
                if (!isResourceCached.HasValue) isResourceCached = GetIsResourceCached();
                return isResourceCached.Value;
            }
        }

        public string ShortStatus
        {
            get
            {
                if (CanCheckInstallation && IsInstalled)
                {
                    if (HasResource && !IsResourceCached)
                        return "not cached";
                    else
                        return "installed";
                }
                else
                {
                    if (IsDeactivated)
                    {
                        if (HasResource && IsResourceCached)
                            return "cached";
                        else
                            return "deactivated";
                    }
                    else
                    {
                        if (IsActive)
                        {
                            if (CanCheckInstallation)
                                return "pending";
                            else
                                return "active";
                        }
                        else
                        {
                            if (HasResource && IsResourceCached)
                                return "cached";
                            else
                                return "inactive";
                        }
                    }
                }
            }
        }

        public string LongStatus
        {
            get
            {
                switch (Typ)
                {
                    case AppTyps.Meta:
                    case AppTyps.Default:
                        if (CanCheckInstallation && IsInstalled)
                        {
                            if (IsDeactivated)
                            {
                                if (HasResource && IsResourceCached)
                                    return "App is deactivated, but cached and installed.";
                                else
                                    return "App is deactivated, but installed.";
                            }
                            else
                            {
                                if (IsActivated)
                                {
                                    if (HasResource && !IsResourceCached)
                                        return "App is active and installed, but its resource is not cached.";
                                    else
                                        return "App is active and installed.";
                                }
                                else if (IsActive)
                                {
                                    if (HasResource && !IsResourceCached)
                                        return "App is required and installed, but its resource is not cached.";
                                    else
                                        return "App is required and installed.";
                                }
                                else
                                {
                                    if (HasResource && !IsResourceCached)
                                        return "App is not active, but installed.";
                                    else
                                        return "App is not active, but cached and installed.";
                                }
                            }
                        }
                        else if (!CanCheckInstallation)
                        {
                            if (IsDeactivated)
                            {
                                if (HasResource && IsResourceCached)
                                    return "App is deactivated, but cached.";
                                else
                                    return "App is deactivated.";
                            }
                            else
                            {
                                if (IsActivated)
                                {
                                    if (HasResource && !IsResourceCached)
                                        return "App is active, but not cached.";
                                    else
                                        return "App is active.";
                                }
                                else if (IsActive)
                                {
                                    if (HasResource && !IsResourceCached)
                                        return "App is required, but not cached.";
                                    else
                                        return "App is required.";
                                }
                                else
                                {
                                    if (HasResource && IsResourceCached)
                                        return "App is not active, but cached.";
                                    else
                                        return "App is not active.";
                                }
                            }
                        }
                        else
                        {
                            if (IsDeactivated)
                            {
                                if (HasResource && IsResourceCached)
                                    return "App is deactivated, but cached.";
                                else
                                    return "App is deactivated.";
                            }
                            else
                            {
                                if (IsActivated)
                                {
                                    if (HasResource && !IsResourceCached)
                                        return "App is active, but not cached or installed.";
                                    else
                                        return "App is active, but not installed.";
                                }
                                else if (IsActive)
                                {
                                    if (HasResource && !IsResourceCached)
                                        return "App is required, but not cached or installed.";
                                    else
                                        return "App is required, but not installed.";
                                }
                                else
                                {
                                    if (HasResource && IsResourceCached)
                                        return "App is not active, but cached.";
                                    else
                                        return "App is not active.";
                                }
                            }
                        }
                    case AppTyps.NodePackage:
                    case AppTyps.RubyPackage:
                    case AppTyps.Python2Package:
                    case AppTyps.Python3Package:
                        if (IsInstalled)
                        {
                            if (IsDeactivated)
                                return "Package is deactivated, but installed.";
                            else
                            {
                                if (IsActivated)
                                    return "Package is active and installed.";
                                else if (IsActive)
                                    return "Package is required and installed.";
                                else
                                    return "Package is not active, but installed.";
                            }
                        }
                        else
                        {
                            if (IsDeactivated)
                                return "Package is deactivated.";
                            else
                            {
                                if (IsActivated)
                                    return "Package is activated, but not installed.";
                                else if (IsActive)
                                    return "Package is required, but not installed.";
                                else
                                    return "Package is not active.";
                            }
                        }
                    default:
                        return "Unkown app typ.";
                }
            }
        }

        public AppStatusIcon StatusIcon
        {
            get
            {
                if (CanCheckInstallation && IsInstalled)
                {
                    if (IsDeactivated)
                        return AppStatusIcon.Warning;
                    else
                    {
                        if (IsActive)
                        {
                            if (HasResource && !IsResourceCached)
                                return AppStatusIcon.Info;
                            else
                                return AppStatusIcon.OK;
                        }
                        else
                            return AppStatusIcon.Tolerated;
                    }
                }
                else
                {
                    if (IsDeactivated)
                    {
                        if (HasResource && IsResourceCached)
                            return AppStatusIcon.Info;
                        else
                            return AppStatusIcon.Blocked;
                    }
                    else
                    {
                        if (IsActive)
                        {
                            if (CanCheckInstallation)
                                return AppStatusIcon.Task;
                            else
                            {
                                if (HasResource && !IsResourceCached)
                                    return AppStatusIcon.Info;
                                else
                                    return AppStatusIcon.OK;
                            }
                        }
                        else
                        {
                            if (HasResource && IsResourceCached)
                                return AppStatusIcon.Cached;
                            else
                                return AppStatusIcon.None;
                        }
                    }
                }
            }
        }

        #endregion

        #region Possible Actions

        public bool CanDownloadResource { get { return HasResource && !IsResourceCached; } }

        public bool CanDeleteResource { get { return HasResource && IsResourceCached; } }

        public bool CanInstall
        {
            get
            {
                return CanCheckInstallation && (!IsInstalled || Force)
                    || !CanCheckInstallation && GetCustomScriptFile("setup") != null;
            }
        }

        public bool CanUninstall
        {
            get
            {
                return CanCheckInstallation && IsInstalled
                    || !CanCheckInstallation && GetCustomScriptFile("remove") != null;
            }
        }

        public bool CanReinstall
        {
            get
            {
                return CanCheckInstallation && IsInstalled
                        && (!HasResource || IsResourceCached)
                        && !IsManagedPackage
                    || !CanCheckInstallation
                        && GetCustomScriptFile("remove") != null
                        && GetCustomScriptFile("setup") != null;
            }
        }

        public bool CanUpgrade
        {
            get
            {
                return
                    CanCheckInstallation && IsInstalled
                        && HasResource && !IsVersioned
                        && !IsManagedPackage
                    || !CanCheckInstallation
                        && HasResource
                        && !IsManagedPackage
                        && GetCustomScriptFile("remove") != null
                        && GetCustomScriptFile("setup") != null;
            }
        }

        public bool ShouldBeInstalled
        {
            get
            {
                return IsActive
                  && (CanCheckInstallation && !IsInstalled
                    || !CanCheckInstallation && GetCustomScriptFile("setup") != null);
            }
        }

        public bool ShouldBeRemoved
        {
            get
            {
                return !IsActive
                    && (CanCheckInstallation && IsInstalled
                        || !CanCheckInstallation && GetCustomScriptFile("remove") != null);
            }
        }

        #endregion

        #region Configuration

        public void Activate()
        {
            AppIndex.SetGroupValue(AppName, PropertyKeys.AppIsActivated, true);
            ActivateDependencies();
        }

        public void ActivateAsRequired()
        {
            AppIndex.SetGroupValue(AppName, PropertyKeys.AppIsRequired, true);
            ActivateDependencies();
        }

        public void ActivateAsDependency()
        {
            AppIndex.SetGroupValue(AppName, PropertyKeys.AppIsDependency, true);
            ActivateDependencies();
        }

        private void ActivateDependencies()
        {
            foreach (var depName in Dependencies)
            {
                var depApp = new AppFacade(AppIndex, depName);
                if (!depApp.IsActive)
                {
                    depApp.ActivateAsDependency();
                }
            }
        }

        public void Deactivate()
        {
            AppIndex.SetGroupValue(AppName, PropertyKeys.AppIsDeactivated, true);
        }

        public void SetupAutoConfiguration()
        {
            SetupAutoDependencies();
            SetupAdornmentForRegistryIsolation();
            SetupAdornmentPath();
        }

        private void AddDependency(string app)
        {
            Dependencies = AddToSet(Dependencies, app);
        }

        private void SetupAutoDependencies()
        {
            switch (Typ)
            {
                case AppTyps.NodePackage:
                    AddDependency(AppKeys.Npm);
                    break;
                case AppTyps.RubyPackage:
                    AddDependency(AppKeys.Ruby);
                    break;
                case AppTyps.Python2Package:
                    AddDependency(AppKeys.Python2);
                    break;
                case AppTyps.Python3Package:
                    AddDependency(AppKeys.Python3);
                    break;
            }
        }

        public void TrackResponsibilities()
        {
            foreach (var dependency in Dependencies)
            {
                var oldSet = AppIndex.GetStringListGroupValue(dependency, PropertyKeys.AppResponsibilities);
                var newSet = AddToSet(oldSet, ID);
                AppIndex.SetGroupValue(dependency, PropertyKeys.AppResponsibilities, newSet);
            }
        }

        private void SetupAdornmentForRegistryIsolation()
        {
            if (RegistryKeys.Length > 0 && AdornedExecutables.Length == 0)
            {
                AddAdornedExecutable(Exe);
            }
        }

        private void SetupAdornmentPath()
        {
            if (AdornedExecutables.Length > 0)
            {
                var proxyDir = IOPath.Combine(
                    AppIndex.GetStringValue(PropertyKeys.AppAdornmentBaseDir),
                    AppName.ToLowerInvariant());
                Path = AppendToList(Path, proxyDir);
            }
        }

        private static string[] AppendToList(string[] list, string value)
        {
            var result = new string[list.Length + 1];
            Array.Copy(list, result, list.Length);
            result[list.Length] = value;
            return result;
        }

        private static string[] AddToSet(string[] list, string value)
        {
            var result = new List<string>(list);
            if (!result.Contains(value))
            {
                result.Add(value);
                return result.ToArray();
            }
            else
            {
                return list;
            }
        }

        private static string[] RemoveFromSet(string[] list, string value)
        {
            var result = new List<string>(list);
            if (result.Contains(value))
            {
                result.Remove(value);
                return result.ToArray();
            }
            else
            {
                return list;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("App[{0}] {1}", Typ, ID);
        }
    }
}
