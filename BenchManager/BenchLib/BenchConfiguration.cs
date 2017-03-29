using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Mastersign.Bench.PropertyCollections;
using Microsoft.Win32;

namespace Mastersign.Bench
{
    /// <summary>
    /// The merged configuration for a Bench environment.
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
    ///         <description><c>bench-site.md</c> files (filename can be changed via custom config)</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public class BenchConfiguration : ResolvingPropertyCollection
    {
        private const string AUTO_DIR = "auto";
        private const string RES_DIR = "res";
        private const string BIN_DIR = AUTO_DIR + @"\bin";
        private const string SCRIPTS_DIR = AUTO_DIR + @"\lib";

        /// <summary>
        /// The relative path of the Bench configuration file.
        /// </summary>
        public const string CONFIG_FILE = RES_DIR + @"\config.md";

        /// <summary>
        /// The relative path of the PowerShell API library file.
        /// </summary>
        public const string MAIN_PS_LIB_FILE = SCRIPTS_DIR + @"\bench.lib.ps1";

        private static readonly string[] BENCH_CHECK_FILES = new[]
        {
            CONFIG_FILE,
            MAIN_PS_LIB_FILE,
        };

        /// <summary>
        /// Checks if the given path is a valid root path of a Bench environment.
        /// </summary>
        /// <param name="possibleBenchRootPath">The absolute path to a possible Bench environment.</param>
        /// <returns><c>true</c> if the given path is a path to a valid Bench environment.</returns>
        public static bool IsValidBenchRoot(string possibleBenchRootPath)
        {
            if (possibleBenchRootPath == null)
                throw new ArgumentNullException(nameof(possibleBenchRootPath));
            if (!Path.IsPathRooted(possibleBenchRootPath))
                throw new ArgumentException("The given path is not absolute.");
            if (!Directory.Exists(possibleBenchRootPath))
                return false;
            foreach (var path in BENCH_CHECK_FILES)
            {
                var absolutePath = Path.Combine(possibleBenchRootPath, path);
                if (!File.Exists(absolutePath)) return false;
            }
            return true;
        }

        /// <summary>
        /// The absolute path to the root directory of Bench.
        /// </summary>
        public string BenchRootDir { get; private set; }

        private string siteConfigFileName; // cached to prevent overriding by custom configuration

        private AppIndex appProperties;

        /// <summary>
        /// A flag which indicates if the app library was loaded during initialization of the <see cref="BenchConfiguration"/>.
        /// </summary>
        public bool WithAppIndex { get; private set; }

        /// <summary>
        /// A flag which indicates if the user configuration was loaded during initialization of the <see cref="BenchConfiguration"/>.
        /// </summary>
        public bool WithUserConfiguration { get; private set; }

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
        /// <param name="loadUserConfiguration">A flag to control if user configuration files are going to be loaded.</param>
        /// <param name="loadSiteConfiguration">A flag to control if site configuration files are going to be loaded.</param>
        public BenchConfiguration(string benchRootDir, bool loadAppIndex, bool loadUserConfiguration, bool loadSiteConfiguration)
        {
            BenchRootDir = benchRootDir;
            WithAppIndex = loadAppIndex;
            WithUserConfiguration = loadUserConfiguration;
            WithSiteConfiguration = loadSiteConfiguration;

            AddResolver(new VariableResolver(this));
            AddResolver(new PropertyPathResolver(IsPathProperty, GetBaseForPathProperty));
            AddResolver(new DictionaryValueResolver());

            var parser = MarkdownPropertyParserFactory.Create(this);

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

            siteConfigFileName = GetStringValue(ConfigPropertyKeys.SiteConfigFileName);

            if (loadUserConfiguration)
            {
                var userConfigFile = GetStringValue(ConfigPropertyKeys.UserConfigFile);
                Debug.WriteLine("Looking for user config file: " + userConfigFile);
                if (File.Exists(userConfigFile))
                {
                    using (var userConfigStream = File.OpenRead(userConfigFile))
                    {
                        Debug.WriteLine("Reading user configuration ...");
                        parser.Parse(userConfigStream);
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

            AutomaticConfiguration();

            if (loadAppIndex)
            {
                appProperties = new AppIndex(this, loadUserConfiguration);
            }
        }


        private bool IsPathProperty(string property)
        {
            return property.EndsWith("File")
                || property.EndsWith("Dir");
        }

        private string GetBaseForPathProperty(string property)
        {
            return BenchRootDir;
        }

        /// <summary>
        /// Checks whether the home directory of this Bench environment 
        /// can be transfered in case of an export or cloning.
        /// </summary>
        public bool CanTransferHomeDirectory
            => GetBooleanValue(ConfigPropertyKeys.OverrideHome)
            && GetStringValue(ConfigPropertyKeys.HomeDir)
                .StartsWith(BenchRootDir, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Checks whether the project directory of this Bench environment
        /// can be transfered in case of an export or cloning.
        /// </summary>
        public bool CanTransferProjectDirectory
            => GetStringValue(ConfigPropertyKeys.ProjectRootDir)
                .StartsWith(BenchRootDir, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Checks whether the cached app libraries of this Bench environment
        /// can be transfered in case of an export or cloning.
        /// </summary>
        public bool CanTransferAppLibraryCache
            => GetStringValue(ConfigPropertyKeys.AppLibsCacheDir)
                .StartsWith(BenchRootDir, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Checks whether the cached app resources of this Bench environment
        /// can be transfered in case of an export or cloning.
        /// </summary>
        public bool CanTransferAppResourceCache
            => GetStringValue(ConfigPropertyKeys.AppsCacheDir)
                .StartsWith(BenchRootDir, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Returns an array with relative paths to the directories and files,
        /// relevant for a transfer of the Bench environment.
        /// </summary>
        /// <returns>An array with relative paths.</returns>
        public string[] GetTransferPaths(TransferPaths selection)
        {
            var result = new List<string>();
            result.Add(AUTO_DIR);
            result.Add(RES_DIR);
            var licenseFile = "LICENSE.md";
            if (File.Exists(Path.Combine(BenchRootDir, licenseFile)))
            {
                result.Add(licenseFile);
            }
            var changeLogFile = "CHANGELOG.md";
            if (File.Exists(Path.Combine(BenchRootDir, changeLogFile)))
            {
                result.Add(changeLogFile);
            }
            if ((selection & TransferPaths.UserConfiguration) == TransferPaths.UserConfiguration)
            {
                result.Add(GetStringValue(ConfigPropertyKeys.UserConfigDir));
            }
            if ((selection & TransferPaths.HomeDirectory) == TransferPaths.HomeDirectory &&
                CanTransferHomeDirectory)
            {
                result.Add(GetStringValue(ConfigPropertyKeys.HomeDir));
            }
            if ((selection & TransferPaths.ProjectDirectory) == TransferPaths.ProjectDirectory &&
                CanTransferProjectDirectory)
            {
                result.Add(GetStringValue(ConfigPropertyKeys.ProjectRootDir));
            }
            if ((selection & TransferPaths.AppLibraries) == TransferPaths.AppLibraries)
            {
                if (CanTransferAppLibraryCache)
                {
                    result.Add(GetStringValue(ConfigPropertyKeys.AppLibsCacheDir));
                }
                result.Add(GetStringValue(ConfigPropertyKeys.AppLibsInstallDir));
            }
            if (CanTransferAppResourceCache)
            {
                if ((selection & TransferPaths.AppResourceCache) == TransferPaths.AppResourceCache)
                {
                    result.Add(GetStringValue(ConfigPropertyKeys.AppsCacheDir));
                }
                else if ((selection & TransferPaths.RequiredAppResourceCache) == TransferPaths.RequiredAppResourceCache)
                {
                    foreach (var a in Apps.RequiredApps)
                    {
                        if (a.HasResource && a.IsResourceCached)
                        {
                            result.Add(Path.Combine(
                                GetStringValue(ConfigPropertyKeys.AppsCacheDir),
                                a.ResourceFileName ?? a.ResourceArchiveName));
                        }
                    }
                }
            }
            if ((selection & TransferPaths.Apps) == TransferPaths.Apps)
            {
                result.Add(GetStringValue(ConfigPropertyKeys.AppsInstallDir));
            }
            else if ((selection & TransferPaths.RequiredApps) == TransferPaths.RequiredApps)
            {
                foreach (var a in Apps.RequiredApps)
                {
                    if (a.Typ == AppTyps.Default &&
                        a.CanCheckInstallation &&
                        a.IsInstalled &&
                        !string.IsNullOrEmpty(a.Dir))
                    {
                        result.Add(a.Dir);
                    }
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].StartsWith(BenchRootDir, StringComparison.InvariantCultureIgnoreCase))
                {
                    result[i] = result[i].Substring(BenchRootDir.Length)
                        .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }
            }
            return result.ToArray();
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
            if (!actuallyLoaded || WithUserConfiguration)
            {
                if ((type & ConfigurationFileType.UserConfig) == ConfigurationFileType.UserConfig)
                {

                    var userConfigFile = GetStringValue(ConfigPropertyKeys.UserConfigFile);
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
                                GetStringValue(ConfigPropertyKeys.AppLibIndexFileName))));
                    }
                }
            }
            if (!actuallyLoaded || (WithAppIndex && WithUserConfiguration))
            {
                if ((type & ConfigurationFileType.UserAppLib) == ConfigurationFileType.UserAppLib)
                {
                    var userAppLib = Path.Combine(
                        GetStringValue(ConfigPropertyKeys.UserConfigDir),
                        GetStringValue(ConfigPropertyKeys.AppLibIndexFileName));
                    if (!mustExist || File.Exists(userAppLib))
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.UserAppLib, 999,
                            userAppLib));
                    }
                }
            }
            if (!actuallyLoaded || (WithAppIndex && WithUserConfiguration))
            {
                if ((type & ConfigurationFileType.Activation) == ConfigurationFileType.Activation)
                {
                    var activationFile = GetStringValue(ConfigPropertyKeys.AppActivationFile);
                    if (!mustExist || File.Exists(activationFile))
                    {
                        files.Add(new ConfigurationFile(ConfigurationFileType.Activation, 1000,
                            activationFile));
                    }
                }
                if ((type & ConfigurationFileType.Deactivation) == ConfigurationFileType.Deactivation)
                {
                    var deactivationFile = GetStringValue(ConfigPropertyKeys.AppDeactivationFile);
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
            SetValue(ConfigPropertyKeys.BenchRoot, BenchRootDir);
            SetValue(ConfigPropertyKeys.BenchDrive, Path.GetPathRoot(BenchRootDir));
            SetValue(ConfigPropertyKeys.BenchAuto, Path.Combine(BenchRootDir, AUTO_DIR));
            SetValue(ConfigPropertyKeys.BenchBin, Path.Combine(BenchRootDir, BIN_DIR));
            SetValue(ConfigPropertyKeys.BenchScripts, Path.Combine(BenchRootDir, SCRIPTS_DIR));

            var versionFile = GetValue(ConfigPropertyKeys.VersionFile) as string;
            var version = File.Exists(versionFile) ? File.ReadAllText(versionFile, Encoding.UTF8).Trim() : "0.0.0";
            SetValue(ConfigPropertyKeys.Version, version);

            if (!GetBooleanValue(ConfigPropertyKeys.OverrideHome))
            {
                SetValue(ConfigPropertyKeys.HomeDir, GetVolatileEnvironmentVariable("USERPROFILE"));
                SetValue(ConfigPropertyKeys.AppDataDir, GetVolatileEnvironmentVariable("APPDATA"));
                SetValue(ConfigPropertyKeys.LocalAppDataDir, GetVolatileEnvironmentVariable("LOCALAPPDATA"));
            }
            if (!GetBooleanValue(ConfigPropertyKeys.OverrideTemp))
            {
                SetValue(ConfigPropertyKeys.TempDir, Path.GetTempPath());
            }
        }

        /// <summary>
        /// Copy a couple of temporary properties, needed during the initialization
        /// of a Bench environment, from a source configuration to this configuration.
        /// </summary>
        /// <param name="sourceCfg">The source configuration to copy from.</param>
        public void CopyBenchInitializationPropertiesFrom(BenchConfiguration sourceCfg)
        {
            foreach (var key in new[]
                {
                    ConfigPropertyKeys.UserConfigRepository,
                    ConfigPropertyKeys.WizzardStartAutoSetup,
                    ConfigPropertyKeys.WizzardSelectedApps,
                })
            {
                SetValue(key, sourceCfg.GetValue(key));
            }
            if (appProperties != null && GetValue(ConfigPropertyKeys.UserConfigRepository) != null)
            {
                appProperties.SetGroupCategory(AppKeys.Git, AppIndex.DefaultAppCategory);
                Apps[AppKeys.Git].ActivateAsRequired();
            }
        }

        /// <summary>
        /// The merged definition of the Bench apps as a <see cref="AppIndexFacade"/>.
        /// </summary>
        public AppIndexFacade Apps => appProperties.Facade;

        /// <summary>
        /// The merged definition of the Bench apps.
        /// </summary>
        public AppIndex AppProperties => appProperties;

        /// <summary>
        /// The app libraries defined in the configuration property <c>AppLibs</c>.
        /// </summary>
        public AppLibrary[] AppLibraries
        {
            get
            {
                var result = new List<AppLibrary>();
                foreach (var item in GetStringListValue(ConfigPropertyKeys.AppLibs))
                {
                    var kvp = ValueParser.ParseKeyValuePair(item);
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
                WithAppIndex, WithUserConfiguration, WithSiteConfiguration);
        }
    }
}
