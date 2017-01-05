using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Mastersign.Bench.Markdown;
using Microsoft.Win32;

namespace Mastersign.Bench
{
    /// <summary>
    /// The merged configuration and app library for a Bench environment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The configuration is merged by loading the following files:
    /// </para>
    /// <list type="bullet">
    ///     <item>
    ///         <term>default</term>
    ///         <description><c>res\config.md</c></description>
    ///     </item>
    ///     <item>
    ///         <term>custom</term>
    ///         <description><c>config\config.md</c></description>
    ///     </item>
    ///     <item>
    ///         <term>site</term>
    ///         <description><c>bench-site.md</c> files (filename can be changed via default/custom config)</description>
    ///     </item>
    /// </list>
    /// <para>
    /// The app library is merged by loading the following files:
    /// </para>
    /// <list type="bullet">
    ///     <item>
    ///       <term>default</term>
    ///       <description><c>res\apps.md</c></description>
    ///     </item>
    ///     <item>
    ///         <term>custom</term>
    ///         <description><c>config\apps.md</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public class BenchConfiguration : ResolvingPropertyCollection
    {
        private const string AUTO_DIR = @"auto";
        private const string BIN_DIR = AUTO_DIR + @"\bin";
        private const string SCRIPTS_DIR = AUTO_DIR + @"\lib";

        /// <summary>
        /// The relative path of the Bench configuration file.
        /// </summary>
        public const string CONFIG_FILE = @"res\config.md";

        /// <summary>
        /// The property group category, which contains app definitions of required apps.
        /// </summary>
        public const string DefaultAppCategory = "Required";

        private readonly AppIndexFacade appIndexFacade;

        /// <summary>
        /// The absolute path to the root directory of Bench.
        /// </summary>
        public string BenchRootDir { get; private set; }

        private string siteConfigFileName; // cached to prevent overriding by custom configuration

        /// <summary>
        /// A flag which indicates if the app library was loaded during initialization of the <see cref="BenchConfiguration"/>.
        /// </summary>
        public bool WithAppIndex { get; private set; }

        /// <summary>
        /// A flag which indicates if the custom configuration was loaded during initialization of the <see cref="BenchConfiguration"/>.
        /// </summary>
        public bool WithCustomConfiguration { get; private set; }

        /// <summary>
        /// A flag which indicates if the site configuration was loaded during the initialization of the <see cref="BenchConfiguration"/>.
        /// </summary>
        public bool WithSiteConfiguration { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchConfiguration"/>
        /// loading all configuration and app library files.
        /// </summary>
        /// <param name="benchRootDir">The absolute path to the root directory of Bench.</param>
        public BenchConfiguration(string benchRootDir)
            : this(benchRootDir, true, true, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchConfiguration"/>
        /// loading the specified set of configuration and app library files.
        /// </summary>
        /// <param name="benchRootDir">The absolute path to the root directory of Bench.</param>
        /// <param name="loadAppIndex">A flag to control if app library files are going to be loaded.</param>
        /// <param name="loadCustomConfiguration">A flag to control if custom configuration files are going to be loaded.</param>
        /// <param name="loadSiteConfiguration">A flag to control if site configuration files are going to be loaded.</param>
        public BenchConfiguration(string benchRootDir, bool loadAppIndex, bool loadCustomConfiguration, bool loadSiteConfiguration)
        {
            BenchRootDir = benchRootDir;
            WithAppIndex = loadAppIndex;
            WithCustomConfiguration = loadCustomConfiguration;
            WithSiteConfiguration = loadSiteConfiguration;
            AddResolver(new GroupedVariableResolver(this));
            AddResolver(new VariableResolver(this));
            AddResolver(new PathResolver(IsPathProperty, GetBaseForPathProperty));

            var parser = new MarkdownPropertyParser
            {
                Target = this,
                GroupBeginCue = new Regex("^[\\*\\+-]\\s+ID:\\s*(`?)(?<group>\\S+?)\\1$"),
                GroupEndCue = new Regex("^\\s*$"),
                CollectGroupDocs = true,
            };

            var configFile = Path.Combine(benchRootDir, CONFIG_FILE);
            Debug.WriteLine("Looking for default configuration: " + configFile);
            if (!File.Exists(configFile))
            {
                throw new FileNotFoundException("The default configuration for Bench was not found.", configFile);
            }
            using (var configStream = File.OpenRead(configFile))
            {
                Debug.WriteLine("Reading default configuration ...");
                parser.Parse(configStream);
            }

            siteConfigFileName = GetStringValue(PropertyKeys.SiteConfigFileName);

            if (loadCustomConfiguration)
            {
                var customConfigFile = GetStringValue(PropertyKeys.CustomConfigFile);
                Debug.WriteLine("Looking for custom config file: " + customConfigFile);
                if (File.Exists(customConfigFile))
                {
                    using (var customConfigStream = File.OpenRead(customConfigFile))
                    {
                        Debug.WriteLine("Reading custom configuration ...");
                        parser.Parse(customConfigStream);
                    }
                }
            }

            if (loadSiteConfiguration)
            {
                Debug.WriteLine("Looking for site config file(s): " + siteConfigFileName);
                var siteConfigFiles = FindSiteConfigFiles(benchRootDir, siteConfigFileName);
                foreach (var file in siteConfigFiles)
                {
                    using (var siteConfigStream = File.OpenRead(file))
                    {
                        Debug.WriteLine("Reading site configuration '" + file + "' ...");
                        parser.Parse(siteConfigStream);
                    }
                }
            }

            if (loadAppIndex)
            {
                foreach (var l in AppLibraries)
                {
                    var appIndexFile = Path.Combine(l.BaseDir, GetStringValue(PropertyKeys.AppLibIndexFileName));
                    Debug.WriteLine("Looking for app library index: " + appIndexFile);
                    if (File.Exists(appIndexFile))
                    {
                        parser.CurrentGroupMetadata = l;
                        using (var appIndexStream = File.OpenRead(appIndexFile))
                        {
                            Debug.WriteLine("Reading index of app library '{0}' ...", l.ID);
                            parser.Parse(appIndexStream);
                        }
                        parser.CurrentGroupMetadata = null;
                    }
                    else
                    {
                        Debug.WriteLine("Index file of app library '{0}' not found.", l.ID);
                    }
                }

                if (loadCustomConfiguration)
                {
                    var customAppIndexFile = Path.Combine(
                        GetStringValue(PropertyKeys.CustomConfigDir),
                        GetStringValue(PropertyKeys.AppLibIndexFileName));
                    Debug.WriteLine("Looking for custom app library index: " + customAppIndexFile);
                    if (File.Exists(customAppIndexFile))
                    {
                        using (var customAppIndexStream = File.OpenRead(customAppIndexFile))
                        {
                            Debug.WriteLine("Reading custom app library index ...");
                            parser.Parse(customAppIndexStream);
                        }
                    }
                }
            }

            AddResolver(new DictionaryValueResolver(this));
            GroupedDefaultValueSource = new AppIndexDefaultValueSource(this);
            appIndexFacade = new AppIndexFacade(this);

            AutomaticConfiguration();
            AutomaticActivation(loadCustomConfiguration);
            RecordResponsibilities();
        }

        private static string[] FindSiteConfigFiles(string benchRootDir, string fileName)
        {
            var results = new List<string>();
            var searchPath = benchRootDir;
            while (!string.IsNullOrEmpty(searchPath))
            {
                var testPath = Path.Combine(searchPath, fileName);
                if (File.Exists(testPath)) results.Add(testPath);
                searchPath = Path.GetDirectoryName(searchPath);
            }
            results.Reverse();
            return results.ToArray();
        }

        /// <summary>
        /// Search for all existing site configuration files in the root directory of Bench
        /// and its parents.
        /// </summary>
        /// <returns>An array with the absolute paths of the found site configuration files.</returns>
        public string[] FindSiteConfigFiles()
        {
            return FindSiteConfigFiles(BenchRootDir, siteConfigFileName);
        }

        /// <summary>
        /// Lists the configuration files of the Bench environment.
        /// </summary>
        /// <param name="type">The kind of files to list.</param>
        /// <param name="actuallyLoaded">If <c>true</c>, only files which are actually loaded
        /// by this instance are listed.</param>
        /// <param name="mustExist">If <c>true</c>, only existing files are listed;
        /// otherwise optional and non existing files are listed to.</param>
        /// <returns>A list with configuration file descriptors.</returns>
        public ConfigurationFile[] GetConfigurationFiles(
            ConfigurationFileType type = ConfigurationFileType.All, 
            bool actuallyLoaded = false, bool mustExist = true)
        {
            if (actuallyLoaded) mustExist = true;
            var files = new List<ConfigurationFile>();
            if ((type & ConfigurationFileType.BenchConfig) == ConfigurationFileType.BenchConfig)
            {
                files.Add(new ConfigurationFile(ConfigurationFileType.BenchConfig, 0,
                    Path.Combine(BenchRootDir, CONFIG_FILE)));
            }
            if (!actuallyLoaded || WithCustomConfiguration)
            {
                if ((type & ConfigurationFileType.UserConfig) == ConfigurationFileType.UserConfig)
                {

                    var userConfigFile = GetStringValue(PropertyKeys.CustomConfigFile);
                    if (!mustExist || File.Exists(userConfigFile))
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.UserConfig, 1,
                            userConfigFile));
                    }
                }
            }
            if (!actuallyLoaded || WithSiteConfiguration)
            {
                if ((type & ConfigurationFileType.SiteConfig) == ConfigurationFileType.SiteConfig)
                {
                    var siteConfigFiles = FindSiteConfigFiles();
                    for (int i = 0; i < siteConfigFiles.Length; i++)
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.SiteConfig, 10 + i,
                            siteConfigFiles[i]));
                    }
                }
            }
            if (!actuallyLoaded || WithAppIndex)
            {
                if ((type & ConfigurationFileType.BenchAppLib) == ConfigurationFileType.BenchAppLib)
                {
                    var appLibraries = AppLibraries;
                    for (var i = 0; i < appLibraries.Length; i++)
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.BenchAppLib, 100 + i,
                            Path.Combine(
                                appLibraries[i].BaseDir,
                                GetStringValue(PropertyKeys.AppLibIndexFileName))));
                    }
                }
            }
            if (!actuallyLoaded || (WithAppIndex && WithCustomConfiguration))
            {
                if ((type & ConfigurationFileType.UserAppLib) == ConfigurationFileType.UserAppLib)
                {
                    var userAppLib = Path.Combine(
                        GetStringValue(PropertyKeys.CustomConfigDir),
                        GetStringValue(PropertyKeys.AppLibIndexFileName));
                    if (!mustExist || File.Exists(userAppLib))
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.UserAppLib, 999,
                            userAppLib));
                    }
                }
            }
            if (!actuallyLoaded || (WithAppIndex && WithCustomConfiguration))
            {
                if ((type & ConfigurationFileType.Activation) == ConfigurationFileType.Activation)
                {
                    var activationFile = GetStringValue(PropertyKeys.AppActivationFile);
                    if (!mustExist || File.Exists(activationFile))
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.Activation, 1000,
                            activationFile));
                    }
                }
                if ((type & ConfigurationFileType.Deactivation) == ConfigurationFileType.Deactivation)
                {
                    var deactivationFile = GetStringValue(PropertyKeys.AppDeactivationFile);
                    if (!mustExist || File.Exists(deactivationFile))
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.Deactivation, 1001,
                            deactivationFile));
                    }
                }
            }
            return files.ToArray();
        }

        /// <summary>
        /// Gets an array with absolute paths for all configuration files
        /// used to compile this configuration.
        /// </summary>
        public string[] Sources
        {
            get
            {
                var files = GetConfigurationFiles(actuallyLoaded: true, mustExist: true);
                var result = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    result[i] = files[i].Path;
                }
                return result;
            }
        }

        private string GetVolatileEnvironmentVariable(string name)
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Volatile Environment", false))
            {
                return key.GetValue(name, null) as string;
            }
        }

        private void AutomaticConfiguration()
        {
            SetValue(PropertyKeys.BenchRoot, BenchRootDir);
            SetValue(PropertyKeys.BenchDrive, Path.GetPathRoot(BenchRootDir));
            SetValue(PropertyKeys.BenchAuto, Path.Combine(BenchRootDir, AUTO_DIR));
            SetValue(PropertyKeys.BenchBin, Path.Combine(BenchRootDir, BIN_DIR));
            SetValue(PropertyKeys.BenchScripts, Path.Combine(BenchRootDir, SCRIPTS_DIR));

            var versionFile = GetValue(PropertyKeys.VersionFile) as string;
            var version = File.Exists(versionFile) ? File.ReadAllText(versionFile, Encoding.UTF8).Trim() : "0.0.0";
            SetValue(PropertyKeys.Version, version);

            if (!GetBooleanValue(PropertyKeys.OverrideHome))
            {
                SetValue(PropertyKeys.HomeDir, GetVolatileEnvironmentVariable("USERPROFILE"));
                SetValue(PropertyKeys.AppDataDir, GetVolatileEnvironmentVariable("APPDATA"));
                SetValue(PropertyKeys.LocalAppDataDir, GetVolatileEnvironmentVariable("LOCALAPPDATA"));
            }
            if (!GetBooleanValue(PropertyKeys.OverrideTemp))
            {
                SetValue(PropertyKeys.TempDir, Path.GetTempPath());
            }

            foreach (var app in Apps)
            {
                app.SetupAutoConfiguration();
            }
        }

        private void AutomaticActivation(bool withCustomConfiguration)
        {
            // activate required apps

            foreach (var app in Apps.ByCategory(DefaultAppCategory))
            {
                Debug.WriteLine(string.Format("Activating required app '{0}'", app.ID));
                app.ActivateAsRequired();
            }

            if (withCustomConfiguration)
            {
                // activate manually activated apps

                var activationFile = new ActivationFile(GetStringValue(PropertyKeys.AppActivationFile));
                foreach (var appName in activationFile)
                {
                    if (Apps.Exists(appName))
                    {
                        Debug.WriteLine(string.Format("Activating app '{0}'", appName));
                        Apps[appName].Activate();
                    }
                }

                // deactivate manually deactivated apps

                var deactivationFile = new ActivationFile(GetStringValue(PropertyKeys.AppDeactivationFile));
                foreach (var appName in deactivationFile)
                {
                    if (Apps.Exists(appName))
                    {
                        Debug.WriteLine(string.Format("Deactivating app '{0}'", appName));
                        Apps[appName].Deactivate();
                    }
                }
            }
        }

        private void RecordResponsibilities()
        {
            foreach (var app in new List<AppFacade>(Apps))
            {
                app.TrackResponsibilities();
            }
        }

        private bool IsPathProperty(string app, string property)
        {
            if (string.IsNullOrEmpty(app))
            {
                return property.EndsWith("File")
                    || property.EndsWith("Dir");
            }
            return property == PropertyKeys.AppDir
                || property == PropertyKeys.AppPath
                || property == PropertyKeys.AppExe
                || property == PropertyKeys.AppSetupTestFile
                || property == PropertyKeys.AppAdornedExecutables
                || property == PropertyKeys.AppLauncherExecutable
                || property == PropertyKeys.AppLauncherIcon;
        }

        private string GetBaseForPathProperty(string app, string property)
        {
            if (string.IsNullOrEmpty(app))
            {
                return BenchRootDir;
            }
            else if (property == PropertyKeys.AppDir)
            {
                return GetStringValue(PropertyKeys.LibDir);
            }
            else
            {
                return GetStringGroupValue(app, PropertyKeys.AppDir);
            }
        }

        /// <summary>
        /// The merged definition of the Bench apps as a <see cref="AppIndexFacade"/>.
        /// </summary>
        public AppIndexFacade Apps { get { return appIndexFacade; } }

        /// <summary>
        /// The app libraries defined in the configuration property <c>AppLibs</c>.
        /// </summary>
        public AppLibrary[] AppLibraries
        {
            get
            {
                var result = new List<AppLibrary>();
                foreach (var item in GetStringListValue(PropertyKeys.AppLibs))
                {
                    var kvp = DictionaryValueResolver.ParseKeyValuePair(item);
                    if (string.IsNullOrEmpty(kvp.Key)) continue;
                    var id = kvp.Key;
                    Uri url;
                    if (!Uri.TryCreate(ExpandAppLibraryUrl(kvp.Value), UriKind.Absolute, out url) ||
                        (!string.Equals("http", url.Scheme, StringComparison.InvariantCultureIgnoreCase) &&
                         !string.Equals("https", url.Scheme, StringComparison.InvariantCultureIgnoreCase) &&
                         !string.Equals("file", url.Scheme, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (string.Equals(result[i].ID, id))
                        {
                            result.RemoveAt(i);
                            break;
                        }
                    }
                    result.Add(new AppLibrary(this, id, url));
                }
                return result.ToArray();
            }
        }

        /// <summary>
        /// Gets an app library by its ID.
        /// </summary>
        /// <param name="id">The ID of the app library.</param>
        /// <returns>An <see cref="AppLibrary"/> object or <c>null</c> if the ID was not found.</returns>
        public AppLibrary GetAppLibrary(string id)
        {
            foreach (var l in AppLibraries)
            {
                if (l.ID == id) return l;
            }
            return null;
        }

        private static readonly Regex GitHubUrlPattern = new Regex(@"^github:(?<ns>[\dA-Za-z-_]+)/(?<name>[\dA-Za-z-_]+)$");
        private static readonly string GitHubUrlTemplate = "https://github.com/{0}/{1}/archive/master.zip";

        private static string ExpandAppLibraryUrl(string url)
        {
            var m = GitHubUrlPattern.Match(url);
            if (m.Success)
            {
                return string.Format(GitHubUrlTemplate,
                    m.Groups["ns"].Value, m.Groups["name"].Value);
            }
            return url;
        }

        /// <summary>
        /// Reloads the set of configuration files, specified during construction.
        /// Call this method to create an updated instance of <see cref="BenchConfiguration"/>
        /// after one of the configuration files was changed.
        /// </summary>
        /// <returns>A new instance of <see cref="BenchConfiguration"/>,
        /// which has loaded the same set of configuration files as this instance.</returns>
        public BenchConfiguration Reload()
        {
            return new BenchConfiguration(BenchRootDir,
                WithAppIndex, WithCustomConfiguration, WithSiteConfiguration);
        }
    }
}
