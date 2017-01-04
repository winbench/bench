﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using IOPath = System.IO.Path;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class is a facade to the properties and the state of an app.
    /// It is initialized with the <see cref="IConfiguration"/> object, holding the apps properties
    /// and the ID of the app.
    /// </summary>
    public class AppFacade
    {
        /// <summary>
        /// The namespace separator in an app ID.
        /// </summary>
        public const char NS_SEPARATOR = '.';

        private readonly IConfiguration AppIndex;

        private readonly string AppName;

        /// <summary>
        /// Initializes a new instance of <see cref="AppFacade"/>.
        /// </summary>
        /// <param name="source">The configuration, containing the properties of the app.</param>
        /// <param name="appName">The ID of the app.</param>
        public AppFacade(IConfiguration source, string appName)
        {
            AppIndex = source;
            AppName = appName;
        }

        /// <summary>
        /// Does some expensive checks for the app and caches the result for later requests.
        /// These checks involve interaction with the file system.
        /// </summary>
        public void LoadCachedValues()
        {
            isInstalled = GetIsInstalled();
            isResourceCached = GetIsResourceCached();
            installedVersion = GetInstalledVersion();
        }

        /// <summary>
        /// Clears cached values from the state of the app.
        /// If the state of an app was possibly changed, this method has to be called,
        /// to allow determining the new state from the file system and not just showing
        /// the last cached state.
        /// </summary>
        /// <seealso cref="LoadCachedValues"/>
        public void DiscardCachedValues()
        {
            isInstalled = null;
            isResourceCached = null;
            installedVersion = null;
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

        /// <summary>
        /// Gets the ID of the app.
        /// </summary>
        public string ID { get { return AppName; } }

        internal static string NamespaceFromId(string id)
        {
            var p = id.LastIndexOf(NS_SEPARATOR);
            return p < 0 ? string.Empty : id.Substring(0, p);
        }

        /// <summary>
        /// Gets the namespace part of the apps ID.
        /// </summary>
        public string Namespace => NamespaceFromId(ID);

        internal static string PathSegmentFromId(string id)
        {
            return id.ToLowerInvariant().Replace(NS_SEPARATOR, IOPath.DirectorySeparatorChar);
        }

        /// <summary>
        /// Gets a part for a filesystem path, which represents the id of this app.
        /// </summary>
        public string PathSegment => PathSegmentFromId(ID);

        internal static string NamespacePathSegmentFromId(string id)
        {
            var ns = NamespaceFromId(id);
            return string.IsNullOrEmpty(ns)
                ? string.Empty
                : ns.ToLowerInvariant().Replace(NS_SEPARATOR, IOPath.DirectorySeparatorChar);
        }

        /// <summary>
        /// Gets a part for a filesystem path, which represents the namespace of this app.
        /// </summary>
        public string NamespacePathSegment => NamespacePathSegmentFromId(ID);

        internal static string NameFromId(string id)
        {
            var p = id.LastIndexOf(NS_SEPARATOR);
            return p < 0 ? id : id.Substring(p + 1);
        }

        /// <summary>
        /// Gets the name part of the apps ID.
        /// </summary>
        public string Name => NameFromId(ID);

        /// <summary>
        /// Gets the app library, this app is defined in.
        /// </summary>
        public AppLibrary AppLibrary => AppIndex.GetGroupMetadata(AppName) as AppLibrary;

        /// <summary>
        /// Gets the label of the app.
        /// </summary>
        public string Label { get { return StringValue(PropertyKeys.AppLabel); } }

        /// <summary>
        /// Gets the category, this app belongs to.
        /// E.g. there are <c>Required</c> and <c>Optional</c> apps.
        /// </summary>
        /// <see cref="BenchConfiguration.DefaultAppCategory"/>
        public string Category { get { return AppIndex.GetGroupCategory(AppName); } }

        /// <summary>
        /// <para>The typ of this app.</para>
        /// <para>See for <see cref="AppTyps"/> to compare and list the app typs.</para>
        /// </summary>
        public string Typ { get { return StringValue(PropertyKeys.AppTyp); } }

        /// <summary>
        /// Checks, if this app is a packaged managed by some kind of package manager.
        /// </summary>
        /// <value><c>true</c> if this app is managed by a package manager; otherwise <c>false</c>.</value>
        public bool IsManagedPackage
        {
            get
            {
                var typ = Typ;
                return typ == AppTyps.NodePackage
                    || typ == AppTyps.RubyPackage
                    || typ == AppTyps.Python2Package
                    || typ == AppTyps.Python3Package
                    || typ == AppTyps.NuGetPackage;
            }
        }

        /// <summary>
        /// Gets the version string of the app, or <c>null</c> if the app has no specified version.
        /// </summary>
        /// <remarks>
        /// If the app has the version <c>"latest"</c> it is considered to have no specified version.
        /// </remarks>
        /// <seealso cref="IsVersioned"/>
        public string Version { get { return StringValue(PropertyKeys.AppVersion); } }

        /// <summary>
        /// Checks, if this app has a specified version.
        /// </summary>
        public bool IsVersioned
        {
            get
            {
                return Version != null && !Version.Equals("latest", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private static Regex simpleVersionPattern = new Regex(@"^\d+(\.\d)*$");

        /// <summary>
        /// Checks, if the version of this app is a simple version number
        /// like <c>1.12.5.000</c> or <c>3.4</c>.
        /// </summary>
        public bool IsSimpleVersion
        {
            get { return IsVersioned && simpleVersionPattern.Match(Version).Success; }
        }

        /// <summary>
        /// Gets the URL of the project or vendor website of this app, or <c>null</c> if no website was specified.
        /// </summary>
        public string Website { get { return StringValue(PropertyKeys.AppWebsite); } }

        /// <summary>
        /// Gets a dictionary with labels and URLs for help and documentation.
        /// If an URL is relative, it is considered to be relative to the apps <see cref="Dir"/>.
        /// </summary>
        public IDictionary<string, string> Docs
        {
            get
            {
                return (Value(PropertyKeys.AppDocs) as IDictionary<string, string>)
                  ?? new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// An array with app IDs which are necessary to be installed for this app to work.
        /// </summary>
        public string[] Dependencies
        {
            get { return ListValue(PropertyKeys.AppDependencies); }
            set { UpdateValue(PropertyKeys.AppDependencies, value); }
        }

        /// <summary>
        /// An array of app IDs which depend on this app to be installed.
        /// </summary>
        public string[] Responsibilities
        {
            get { return ListValue(PropertyKeys.AppResponsibilities); }
            set { UpdateValue(PropertyKeys.AppResponsibilities, value); }
        }

        /// <summary>
        /// Checks, whether this app is marked as activated by the user, or not.
        /// </summary>
        /// <value><c>true</c> if the apps ID is marked as activated by the user; otherwise <c>false</c>.</value>
        public bool IsActivated { get { return BoolValue(PropertyKeys.AppIsActivated); } }

        /// <summary>
        /// Checks, whether this app is marked as deactivated by the user, or not.
        /// </summary>
        /// <value><c>true</c> if the apps ID is marked as deactivated by the user; otherwise <c>false</c>.</value>
        public bool IsDeactivated { get { return BoolValue(PropertyKeys.AppIsDeactivated); } }

        /// <summary>
        /// Checks, whether this app is required by the Bench system, or not.
        /// </summary>
        /// <value><c>true</c> if the app is required by Bench; otherwise <c>false</c>.</value>
        public bool IsRequired { get { return BoolValue(PropertyKeys.AppIsRequired); } }

        /// <summary>
        /// Checks, whether this app is dependency of another app.
        /// </summary>
        /// <value><c>true</c> if the app is required by another app; otherwise <c>false</c>.</value>
        public bool IsDependency { get { return BoolValue(PropertyKeys.AppIsDependency); } }

        /// <summary>
        /// Gets the URL of the apps resource, or <c>null</c> if the app has no downloadable resource.
        /// </summary>
        public string Url { get { return StringValue(PropertyKeys.AppUrl); } }

        /// <summary>
        /// Gets a dictionary with HTTP header fields for the download request.
        /// </summary>
        public IDictionary<string, string> DownloadHeaders
        {
            get
            {
                return (Value(PropertyKeys.AppDownloadHeaders) as IDictionary<string, string>)
                  ?? new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Gets a dictionary with HTTP cookies for the download request.
        /// </summary>
        public IDictionary<string, string> DownloadCookies
        {
            get
            {
                return (Value(PropertyKeys.AppDownloadCookies) as IDictionary<string, string>)
                  ?? new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Gets the name of the apps file resource, or <c>null</c>
        /// in case the app has an archive resource or no downloadable resource at all.
        /// </summary>
        public string ResourceFileName { get { return StringValue(PropertyKeys.AppResourceName); } }

        /// <summary>
        /// Gets the name of the apps archive resource, or <c>null</c>
        /// in case the app has a file resource or no downloadable resource at all.
        /// </summary>
        public string ResourceArchiveName { get { return StringValue(PropertyKeys.AppArchiveName); } }

        /// <summary>
        /// Gets the sub path inside of the resource archive, or <c>null</c>
        /// in case the whole archive can be extracted or the app has no archive resource.
        /// </summary>
        public string ResourceArchivePath { get { return StringValue(PropertyKeys.AppArchivePath); } }

        /// <summary>
        /// Gets the typ of the resource archive, or <c>null</c> if the app has no archive resource.
        /// See <see cref="AppArchiveTyps"/> to compare or list the possible typs of an archive resource.
        /// </summary>
        public string ResourceArchiveTyp { get { return StringValue(PropertyKeys.AppArchiveTyp); } }

        /// <summary>
        /// Gets a value, which specifies if the app will be installed even if it is already installed.
        /// </summary>
        public bool Force
        {
            get { return BoolValue(PropertyKeys.AppForce); }
            set { UpdateValue(PropertyKeys.AppForce, value); }
        }

        /// <summary>
        /// The name of the package represented by this app, or <c>null</c> in case
        /// <see cref="IsManagedPackage"/> is <c>false</c>.
        /// </summary>
        public string PackageName { get { return StringValue(PropertyKeys.AppPackageName); } }

        /// <summary>
        /// The name of the target directory for this app.
        /// The target directory is the directory where the app resources are installed.
        /// </summary>
        public string Dir { get { return StringValue(PropertyKeys.AppDir); } }

        /// <summary>
        /// The relative path of the main executable file of the app, or <c>null</c>
        /// in case the app has no executable (e.g. the app is just a group).
        /// The path is relative to the target <see cref="Dir"/> of this app.
        /// </summary>
        public string Exe { get { return StringValue(PropertyKeys.AppExe); } }

        /// <summary>
        /// The relative path to a file, which existence can be used to check if the app is installed,
        /// or <c>null</c> e.g. in case the app is a package managed by a package manager.
        /// The path is relative to the target <see cref="Dir"/> of this app.
        /// </summary>
        public string SetupTestFile { get { return StringValue(PropertyKeys.AppSetupTestFile); } }

        /// <summary>
        /// An array with relative or absolute paths,
        /// which will be added to the environment variable <c>PATH</c> when this app is activated.
        /// If a path is relative, it is relative to the target <see cref="Dir"/> of this app.
        /// </summary>
        /// <seealso cref="Register"/>
        public string[] Path
        {
            get { return ListValue(PropertyKeys.AppPath); }
            set { UpdateValue(PropertyKeys.AppPath, value); }
        }

        /// <summary>
        /// A flag to control if the <see cref="Path"/>s of this app will be added
        /// to the environment variable <c>PATH</c>.
        /// </summary>
        public bool Register { get { return BoolValue(PropertyKeys.AppRegister); } }

        /// <summary>
        /// A dictionary with additional environment variables to setup, when this app is activated.
        /// </summary>
        public IDictionary<string, string> Environment
        {
            get
            {
                return (Value(PropertyKeys.AppEnvironment) as IDictionary<string, string>)
                  ?? new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// An array with paths to executables, which must be adorned.
        /// </summary>
        public string[] AdornedExecutables
        {
            get { return ListValue(PropertyKeys.AppAdornedExecutables); }
            set { UpdateValue(PropertyKeys.AppAdornedExecutables, value); }
        }

        internal void AddAdornedExecutable(string path)
        {
            AdornedExecutables = AddToSet(AdornedExecutables, path);
        }

        internal void RemoveAdornedExecutable(string path)
        {
            AdornedExecutables = RemoveFromSet(AdornedExecutables, path);
        }

        /// <summary>
        /// Gets the base path of the directory containing the adornmend proxy scripts for the executables of this app.
        /// </summary>
        public string AdornmentProxyBasePath
        {
            get
            {
                return IOPath.Combine(
                    AppIndex.GetStringValue(PropertyKeys.AppAdornmentBaseDir),
                    ID.ToLowerInvariant());
            }
        }

        /// <summary>
        /// Checks, whether an executable of this app is marked as adorned, or not.
        /// </summary>
        /// <param name="exePath">The path to the executable in question.</param>
        /// <returns><c>true</c> if the executable must be adorned, otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Gets the path to the adornment wrapper script for a given executable of this app.
        /// </summary>
        /// <param name="exePath">The path to the executable.</param>
        /// <returns>The path to the adornment script.</returns>
        public string GetExecutableProxy(string exePath)
        {
            return IOPath.Combine(AdornmentProxyBasePath,
                IOPath.GetFileNameWithoutExtension(exePath) + ".cmd");
        }

        /// <summary>
        /// Checks, whether execution adornment proxies are required for this app, or not.
        /// </summary>
        public bool IsAdornmentRequired
        {
            get
            {
                return (RegistryKeys.Length > 0 && AppIndex.GetBooleanValue(PropertyKeys.UseRegistryIsolation))
                    || File.Exists(GetCustomScript("pre-run"))
                    || File.Exists(GetCustomScript("post-run"));
            }
        }

        /// <summary>
        /// An array with registry paths relative to the <c>HKCU</c> (current user) hive,
        /// which must be considered for registry isolation.
        /// </summary>
        public string[] RegistryKeys { get { return ListValue(PropertyKeys.AppRegistryKeys); } }

        /// <summary>
        /// The label for the apps launcher, or <c>null</c> if the app has no launcher.
        /// </summary>
        public string Launcher { get { return StringValue(PropertyKeys.AppLauncher); } }

        /// <summary>
        /// The path to the main executable, to be started by the apps launcher,
        /// or <c>null</c> if the app has no launcher.
        /// </summary>
        public string LauncherExecutable { get { return StringValue(PropertyKeys.AppLauncherExecutable); } }

        /// <summary>
        /// An array with command line arguments to be sent to the <see cref="LauncherExecutable"/>
        /// by the launcher.
        /// The last entry in this array must be <c>%*</c> if additional arguments shell be passed
        /// from the launcher to the executable. This is also necessary for drag-and-drop of files
        /// onto the launcher to work.
        /// </summary>
        public string[] LauncherArguments { get { return ListValue(PropertyKeys.AppLauncherArguments); } }

        /// <summary>
        /// A path to an <c>*.ico</c> or <c>*.exe</c> file with the icon for the apps launcher,
        /// or <c>null</c> if the app has no launcher.
        /// </summary>
        public string LauncherIcon { get { return StringValue(PropertyKeys.AppLauncherIcon); } }

        #region Recursive Discovery

        internal IList<string> FindAllDependencies()
        {
            return FindAppsRecursively(PropertyKeys.AppDependencies);
        }

        internal IList<string> FindAllResponsibilities()
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

        internal string GetLauncherFile()
        {
            var launcher = Launcher;
            return launcher != null
                ? IOPath.Combine(
                    AppIndex.GetStringValue(PropertyKeys.LauncherDir),
                    launcher + ".lnk")
                : null;
        }

        internal string GetLauncherScriptFile()
        {
            return IOPath.Combine(
                AppIndex.GetStringValue(PropertyKeys.LauncherScriptDir),
                ID.ToLowerInvariant() + ".cmd");
        }

        /// <summary>
        /// Gets a path to a custom script file for this app.
        /// </summary>
        /// <param name="typ">The typ of the custom script (e.g. <c>setup</c>).</param>
        /// <returns>A path to the script file or <c>null</c> if no custom script exists.</returns>
        public string GetCustomScript(string typ)
        {
            var relativePath = IOPath.Combine(
                AppIndex.GetStringValue(PropertyKeys.AppLibCustomScriptDirName),
                NamespacePathSegment);
            var scriptName = string.Format("{0}.{1}.ps1", Name.ToLowerInvariant(), typ);

            var userPath = IOPath.Combine(
                IOPath.Combine(AppIndex.GetStringValue(PropertyKeys.CustomConfigDir), relativePath),
                scriptName);
            if (File.Exists(userPath)) return userPath;

            if (AppLibrary != null)
            {
                var libraryPath = IOPath.Combine(
                        IOPath.Combine(AppLibrary.BaseDir, relativePath),
                        scriptName);
                if (File.Exists(libraryPath)) return libraryPath;
            }
            return null;
        }

        /// <summary>
        /// Gets a path to a setup resource file or directory.
        /// </summary>
        /// <param name="relativeResourcePath">A relative path to a setup resource.</param>
        /// <returns>An absolute path to the resource or <c>null</c>, if the resource does not exists.</returns>
        public string GetSetupResource(string relativeResourcePath)
        {
            var relativeDirPath = IOPath.Combine(
                AppIndex.GetStringValue(PropertyKeys.AppLibResourceDirName),
                PathSegment);

            var userPath = IOPath.Combine(
                IOPath.Combine(
                    AppIndex.GetStringValue(PropertyKeys.CustomConfigDir),
                    relativeDirPath),
                relativeResourcePath);
            if (File.Exists(userPath) || Directory.Exists(userPath)) return userPath;

            if (AppLibrary != null)
            {
                var libraryPath = IOPath.Combine(
                        IOPath.Combine(AppLibrary.BaseDir, relativeDirPath),
                        relativeResourcePath);
                if (File.Exists(libraryPath) || Directory.Exists(libraryPath)) return libraryPath;
            }
            return null;
        }

        internal string GetVersionFile()
        {
            return IOPath.Combine(
                AppIndex.GetStringValue(PropertyKeys.AppVersionIndexDir),
                ID + ".txt");
        }

        #endregion

        #region Status

        /// <summary>
        /// <para>
        /// Checks, whether is app is active.
        /// An app can be active, because it was marked by the user to be activated,
        /// or because it is required by Bench or it is a dependency for another app.
        /// </para>
        /// <para>
        /// An app is <strong>not active</strong> if it was marked by the user as deactivated,
        /// regardless whether it is required by Bench or another app or marked as activated.
        /// </para>
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !IsDeactivated
                    && (IsRequired || IsDependency || IsActivated);
            }
        }

        /// <summary>
        /// Checks, whether this app has a downloadable app resource, or not.
        /// </summary>
        public bool HasResource
        {
            get
            {
                return Typ == AppTyps.Default &&
                    (ResourceFileName != null || ResourceArchiveName != null);
            }
        }

        /// <summary>
        /// Checks, whether the installation state of this app can be checked, or not.
        /// </summary>
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

        /// <summary>
        /// Checks, whether this app is currently installed, or not.
        /// </summary>
        /// <remarks>
        /// This state is cached for performance reasons.
        /// To be shure to get the real current state, call <see cref="DiscardCachedValues"/>
        /// upfront.
        /// </remarks>
        /// <seealso cref="DiscardCachedValues"/>
        /// <seealso cref="LoadCachedValues"/>
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

        /// <summary>
        /// <para>
        /// Checks, whether this apps resource is currently cached, or not.
        /// </para>
        /// <para>
        /// Returns always <c>false</c>, if the apps <see cref="Typ"/> is not <see cref="AppTyps.Default"/>.
        /// Returns <c>true</c> if the apps <see cref="Typ"/> is <see cref="AppTyps.Default"/>,
        /// but the app has no downloadable resource.
        /// </para>
        /// </summary>
        /// <remarks>
        /// This state is cached for performance reasons.
        /// To be shure to get the real current state, call <see cref="DiscardCachedValues"/>
        /// upfront.
        /// </remarks>
        /// <seealso cref="DiscardCachedValues"/>
        /// <seealso cref="LoadCachedValues"/>
        public bool IsResourceCached
        {
            get
            {
                if (!isResourceCached.HasValue) isResourceCached = GetIsResourceCached();
                return isResourceCached.Value;
            }
        }

        private string installedVersion;
        private readonly object versionFileLockHandle = new object();

        private string GetInstalledVersion()
        {
            var versionFile = GetVersionFile();
            lock (versionFileLockHandle)
            {
                if (File.Exists(versionFile))
                {
                    using (var s = File.Open(versionFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var r = new StreamReader(s, Encoding.UTF8, true))
                    {
                        return r.ReadToEnd().Trim();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void SetInstalledVersion(string version)
        {
            var versionFile = GetVersionFile();
            lock (versionFileLockHandle)
            {
                if (string.IsNullOrEmpty(version))
                {
                    if (File.Exists(versionFile))
                    {
                        File.Delete(versionFile);
                    }
                }
                else
                {
                    using (var s = File.Open(versionFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (var w = new StreamWriter(s, Encoding.UTF8))
                    {
                        w.Write(version.Trim());
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the version string, of the currently installed app.
        /// </summary>
        public string InstalledVersion
        {
            get { return installedVersion ?? (installedVersion = GetInstalledVersion()); }
            set
            {
                SetInstalledVersion(value);
                installedVersion = (value ?? "").Trim() == "" ? null : value;
            }
        }

        /// <summary>
        /// Checks, whether the version of the app definition equals the version of the installed app.
        /// </summary>
        public bool IsVersionUpToDate
        {
            get { return InstalledVersion.Equals(Version ?? string.Empty); }
        }

        /// <summary>
        /// Returns a short string, describing the overall state of the app.
        /// </summary>
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

        /// <summary>
        /// Returns a string with a virtually complete description of the apps overall state.
        /// </summary>
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
                    case AppTyps.NuGetPackage:
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

        /// <summary>
        /// Returns a code for an icon, describing the overall state of this app.
        /// </summary>
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
                            if (!IsVersionUpToDate)
                                return AppStatusIcon.Info;
                            else if (HasResource && !IsResourceCached)
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

        /// <summary>
        /// Checks, whether the app has a resource and the resource is not cached.
        /// </summary>
        public bool CanDownloadResource { get { return HasResource && !IsResourceCached; } }

        /// <summary>
        /// Checks, whether the app has cached resource.
        /// </summary>
        public bool CanDeleteResource { get { return HasResource && IsResourceCached; } }

        /// <summary>
        /// Checks, whether this app can be installed.
        /// </summary>
        public bool CanInstall
        {
            get
            {
                return CanCheckInstallation && (!IsInstalled || Force)
                    || !CanCheckInstallation && GetCustomScript("setup") != null;
            }
        }

        /// <summary>
        /// Checks, whether this app can be uninstalled.
        /// </summary>
        public bool CanUninstall
        {
            get
            {
                return CanCheckInstallation && IsInstalled
                    || !CanCheckInstallation && GetCustomScript("remove") != null;
            }
        }

        /// <summary>
        /// Checks, whether this app can be reinstalled.
        /// </summary>
        public bool CanReinstall
        {
            get
            {
                return CanCheckInstallation && IsInstalled
                        && (!HasResource || IsResourceCached)
                        && !IsManagedPackage
                    || !CanCheckInstallation
                        && GetCustomScript("remove") != null
                        && GetCustomScript("setup") != null;
            }
        }

        /// <summary>
        /// Checks, whether this app can be upgraded to a more recent version.
        /// </summary>
        /// <remarks>
        /// This method does not check if there really is a more recent version of this app.
        /// </remarks>
        public bool CanUpgrade
        {
            get
            {
                return
                    // Default app with no version or version difference
                    CanCheckInstallation && IsInstalled
                        && !IsManagedPackage
                        && (!IsVersioned || !IsVersionUpToDate)
                    // Default app with custom setup and remove
                    || !CanCheckInstallation
                        && !IsManagedPackage
                        && GetCustomScript("remove") != null
                        && GetCustomScript("setup") != null;
            }
        }

        /// <summary>
        /// Checks, whether this app is active (activated or required) but not installed.
        /// </summary>
        public bool ShouldBeInstalled
        {
            get
            {
                return IsActive
                  && (CanCheckInstallation && !IsInstalled
                    || !CanCheckInstallation && GetCustomScript("setup") != null);
            }
        }

        /// <summary>
        /// Checks, whether this app is not activated or even deactivated but installed.
        /// </summary>
        public bool ShouldBeRemoved
        {
            get
            {
                return !IsActive
                    && (CanCheckInstallation && IsInstalled
                        || !CanCheckInstallation && GetCustomScript("remove") != null);
            }
        }

        #endregion

        #region Configuration

        internal void Activate()
        {
            AppIndex.SetGroupValue(AppName, PropertyKeys.AppIsActivated, true);
            ActivateDependencies();
        }

        internal void ActivateAsRequired()
        {
            AppIndex.SetGroupValue(AppName, PropertyKeys.AppIsRequired, true);
            ActivateDependencies();
        }

        internal void ActivateAsDependency()
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

        /// <summary>
        /// Marks this app as deactivetd.
        /// </summary>
        public void Deactivate()
        {
            AppIndex.SetGroupValue(AppName, PropertyKeys.AppIsDeactivated, true);
        }

        /// <summary>
        /// Make some implicit values and relationships explicit in the property values.
        /// </summary>
        internal void SetupAutoConfiguration()
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
                    AddDependency(AppKeys.RubyGems);
                    break;
                case AppTyps.Python2Package:
                    AddDependency(AppKeys.Python2);
                    break;
                case AppTyps.Python3Package:
                    AddDependency(AppKeys.Python3);
                    break;
                case AppTyps.NuGetPackage:
                    AddDependency(AppKeys.NuGet);
                    break;
            }
        }

        /// <summary>
        /// Track down all apps, which refer to this app as a dependency
        /// and store the list app IDs in the app property <c>Responsibilities</c>.
        /// </summary>
        internal void TrackResponsibilities()
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

        #region PropertyListing

        /// <summary>
        /// An array with all property keys, which are known by the Bench system.
        /// </summary>
        public static readonly string[] KnownPropertyKeys = new[]
            {
                PropertyKeys.AppTyp,
                PropertyKeys.AppLabel,
                PropertyKeys.AppWebsite,
                PropertyKeys.AppDocs,
                PropertyKeys.AppVersion,
                PropertyKeys.AppIsActive,
                PropertyKeys.AppIsRequired,
                PropertyKeys.AppIsActivated,
                PropertyKeys.AppIsDeactivated,
                PropertyKeys.AppHasResource,
                PropertyKeys.AppIsResourceCached,
                PropertyKeys.AppInstalledVersion,
                PropertyKeys.AppDependencies,
                PropertyKeys.AppIsDependency,
                PropertyKeys.AppForce,
                PropertyKeys.AppSetupTestFile,
                PropertyKeys.AppPackageName,
                PropertyKeys.AppUrl,
                PropertyKeys.AppDownloadCookies,
                PropertyKeys.AppDownloadHeaders,
                PropertyKeys.AppResourceName,
                PropertyKeys.AppArchiveName,
                PropertyKeys.AppArchiveTyp,
                PropertyKeys.AppArchivePath,
                PropertyKeys.AppDir,
                PropertyKeys.AppExe,
                PropertyKeys.AppRegister,
                PropertyKeys.AppPath,
                PropertyKeys.AppEnvironment,
                PropertyKeys.AppAdornedExecutables,
                PropertyKeys.AppRegistryKeys,
                PropertyKeys.AppLauncher,
                PropertyKeys.AppLauncherExecutable,
                PropertyKeys.AppLauncherArguments,
                PropertyKeys.AppLauncherIcon,
            };

        /// <summary>
        /// Checks whether a property name is known to the Bench system or not.
        /// </summary>
        /// <param name="propertyName">The name of the app property.</param>
        /// <returns><c>true</c> if the property is known; otherwise <c>false</c>.</returns>
        public static bool IsKnownProperty(string propertyName)
        {
            foreach (var name in KnownPropertyKeys)
            {
                if (name.Equals(propertyName)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns all known properties.
        /// </summary>
        /// <returns>An array with key/value pairs. </returns>
        public KeyValuePair<string, object>[] KnownProperties
        {
            get
            {
                var result = new List<KeyValuePair<string, object>>();
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppTyp, this.Typ));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppLabel, this.Label));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppWebsite, this.Website));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppDocs, this.Docs));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppVersion, this.Version));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppIsActive, this.IsActive));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppIsRequired, this.IsRequired));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppIsActivated, this.IsActivated));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppIsDeactivated, this.IsDeactivated));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppHasResource, this.HasResource));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppIsResourceCached, this.IsResourceCached));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppInstalledVersion, this.InstalledVersion));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppDependencies, this.Dependencies));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppIsDependency, this.IsDependency));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppForce, this.Force));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppSetupTestFile, this.SetupTestFile));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppPackageName, this.PackageName));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppUrl, this.Url));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppDownloadCookies, this.DownloadCookies));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppDownloadHeaders, this.DownloadHeaders));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppResourceName, this.ResourceFileName));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppArchiveName, this.ResourceArchiveName));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppArchivePath, this.ResourceArchivePath));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppDir, this.Dir));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppExe, this.Exe));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppRegister, this.Register));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppPath, this.Path));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppEnvironment, this.Environment));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppAdornedExecutables, this.AdornedExecutables));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppRegistryKeys, this.RegistryKeys));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppLauncher, this.Launcher));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppLauncherExecutable, this.LauncherExecutable));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppLauncherArguments, this.LauncherArguments));
                result.Add(new KeyValuePair<string, object>(PropertyKeys.AppLauncherIcon, this.LauncherIcon));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Returns all unknown properties.
        /// </summary>
        /// <returns>An array with key/value pairs.</returns>
        public KeyValuePair<string, object>[] UnknownProperties
        {
            get
            {
                var result = new List<KeyValuePair<string, object>>();
                foreach (var name in AppIndex.PropertyNames(ID))
                {
                    if (IsKnownProperty(name)) continue;
                    result.Add(new KeyValuePair<string, object>(name, Value(name)));
                }
                return result.ToArray();
            }
        }

        #endregion



        /// <summary>
        /// Returns a string, containing the apps typ and ID.
        /// </summary>
        /// <returns>A short string representation of the app.</returns>
        public override string ToString()
        {
            return string.Format("App[{0}] {1}", Typ, ID);
        }
    }
}
