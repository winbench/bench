using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Mastersign.Bench.Markdown;

namespace Mastersign.Bench
{
    public class BenchConfiguration : ResolvingPropertyCollection
    {
        private const string AutoDir = @"auto";
        private const string ScriptsDir = @"auto\lib";
        private const string ConfigFile = @"res\config.md";
        public const string DefaultAppCategory = "Required";

        private readonly AppIndexFacade appIndexFacade;

        public string BenchRootDir { get; private set; }
        private string siteConfigFileName; // cached to prevent overriding by custom configuration

        public bool WithAppIndex { get; private set; }
        public bool WithCustomConfiguration { get; private set; }
        public bool WithSiteConfiguration { get; private set; }

        public BenchConfiguration(string benchRootDir)
            : this(benchRootDir, true, true, true)
        {
        }

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
            };

            var configFile = Path.Combine(benchRootDir, ConfigFile);
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
                foreach(var file in siteConfigFiles)
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
                var appIndexFile = GetStringValue(PropertyKeys.AppIndexFile);
                Debug.WriteLine("Looking for application index: " + appIndexFile);
                if (!File.Exists(appIndexFile))
                {
                    throw new FileNotFoundException("The default app index for Bench was not found.", appIndexFile);
                }
                using (var appIndexStream = File.OpenRead(appIndexFile))
                {
                    Debug.WriteLine("Reading default application index ...");
                    parser.Parse(appIndexStream);
                }

                if (loadCustomConfiguration)
                {
                    var customAppIndexFile = GetStringValue(PropertyKeys.CustomAppIndexFile);
                    Debug.WriteLine("Looking for custom application index: " + customAppIndexFile);
                    if (File.Exists(customAppIndexFile))
                    {
                        using (var customAppIndexStream = File.OpenRead(customAppIndexFile))
                        {
                            Debug.WriteLine("Reading custom application index ...");
                            parser.Parse(customAppIndexStream);
                        }
                    }
                }
            }

            AddResolver(new AppIndexValueResolver(this));
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

        public string[] FindSiteConfigFiles()
        {
            return FindSiteConfigFiles(BenchRootDir, siteConfigFileName);
        }

        private void AutomaticConfiguration()
        {
            foreach (var app in Apps)
            {
                app.SetupAutoConfiguration();
            }
            SetValue(PropertyKeys.BenchRoot, BenchRootDir);
            SetValue(PropertyKeys.BenchDrive, Path.GetPathRoot(BenchRootDir));
            SetValue(PropertyKeys.BenchAuto, Path.Combine(BenchRootDir, AutoDir));
            SetValue(PropertyKeys.BenchScripts, Path.Combine(BenchRootDir, ScriptsDir));

            var versionFile = GetValue(PropertyKeys.VersionFile) as string;
            var version = File.Exists(versionFile) ? File.ReadAllText(versionFile, Encoding.UTF8) : "0.0.0";
            SetValue(PropertyKeys.Version, version);
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
            foreach(var app in new List<AppFacade>(Apps))
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

        public AppIndexFacade Apps { get { return appIndexFacade; } }

        public BenchConfiguration Reload()
        {
            return new BenchConfiguration(BenchRootDir,
                WithAppIndex, WithCustomConfiguration, WithSiteConfiguration);
        }
    }
}
