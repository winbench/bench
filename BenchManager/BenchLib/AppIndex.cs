using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mastersign.Bench.PropertyCollections;

namespace Mastersign.Bench
{
    /// <summary>
    /// The merged app library for a Bench environment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The app library is merged by loading the following files:
    /// </para>
    /// <list type="bullet">
    ///     <item>
    ///       <term>external</term>
    ///       <description><c>lib\_applibs\*\apps.md</c></description>
    ///     </item>
    ///     <item>
    ///         <term>user</term>
    ///         <description><c>config\apps.md</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public class AppIndex : ResolvingGroupedPropertyCollection
    {
        private readonly BenchConfiguration Config;

        /// <summary>
        /// A flag which indicates if the user configuration was loaded during initialization of the <see cref="BenchConfiguration"/>.
        /// </summary>
        public bool WithUserConfiguration { get; private set; }

        /// <summary>
        /// The merged definition of the Bench apps as a <see cref="AppIndexFacade"/>.
        /// </summary>
        public AppIndexFacade Facade { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AppIndex"/>.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="loadUserConfiguration"><c>true</c> if the user app library is to be loaded.</param>
        public AppIndex(BenchConfiguration config, bool loadUserConfiguration)
        {
            Config = config;
            WithUserConfiguration = loadUserConfiguration;

            AddResolver(new GroupedVariableResolver(this));
            AddResolver(new VariableResolver(config));
            AddResolver(new GroupedPropertyPathResolver(IsPathProperty, GetBaseForPathProperty));
            AddResolver(new DictionaryValueResolver());

            var parser = MarkdownPropertyParserFactory.Create(this);

            foreach (var l in config.AppLibraries)
            {
                var appIndexFile = Path.Combine(l.BaseDir, config.GetStringValue(ConfigPropertyKeys.AppLibIndexFileName));
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

            if (loadUserConfiguration)
            {
                var customAppIndexFile = Path.Combine(
                    config.GetStringValue(ConfigPropertyKeys.UserConfigDir),
                    config.GetStringValue(ConfigPropertyKeys.AppLibIndexFileName));
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

            GroupedDefaultValueSource = new AppIndexDefaultValueSource(config, this);

            Facade = new AppIndexFacade(Config, this);

            LoadAppActivation();
            AutomaticConfiguration();
            ActivateAppDependencies();
            RecordAppResponsibilities();
        }

        private void AutomaticConfiguration()
        {
            foreach (var app in Facade)
            {
                app.SetupAutoConfiguration();
            }
        }

        /// <summary>
        /// The property group category, which contains app definitions of required apps.
        /// </summary>
        public const string DefaultAppCategory = "Required";
        private void RecordAppResponsibilities()
        {
            var apps = new List<AppFacade>(Facade);
            foreach(var app in apps)
            {
                ResetGroupValue(app.Name, AppPropertyKeys.Responsibilities);
            }
            foreach (var app in apps)
            {
                app.TrackResponsibilities();
            }
        }

        private void LoadAppActivation()
        {
            // activate required apps

            foreach (var app in Facade.ByCategory(DefaultAppCategory))
            {
                Debug.WriteLine(string.Format("Activating required app '{0}'", app.ID));
                app.ActivateAsRequired();
            }

            if (WithUserConfiguration)
            {
                // activate manually activated apps

                var activationFile = new ActivationFile(Config.GetStringValue(ConfigPropertyKeys.AppActivationFile));
                foreach (var appName in activationFile)
                {
                    if (Facade.Exists(appName))
                    {
                        Debug.WriteLine(string.Format("Activating app '{0}'", appName));
                        Facade[appName].Activate();
                    }
                }

                // deactivate manually deactivated apps

                var deactivationFile = new ActivationFile(Config.GetStringValue(ConfigPropertyKeys.AppDeactivationFile));
                foreach (var appName in deactivationFile)
                {
                    if (Facade.Exists(appName))
                    {
                        Debug.WriteLine(string.Format("Deactivating app '{0}'", appName));
                        Facade[appName].Deactivate();
                    }
                }
            }
        }

        private void ActivateAppDependencies()
        {
            var apps = new List<AppFacade>(Facade);
            foreach (var app in apps)
            {
                if (app.IsActive) app.ActivateDependencies();
            }
        }

        private void ResetAppActivation()
        {
            foreach (var app in Facade)
            {
                app.ResetActivation();
                app.ResetAutoDependency();
            }
        }

        /// <summary>
        /// Reads the activation and deactivation files,
        /// and reloads the app activation and dependency structure.
        /// </summary>
        public void ReloadAppActivation()
        {
            ResetAppActivation();
            LoadAppActivation();
            AutomaticConfiguration();
            ActivateAppDependencies();
        }

        private bool IsPathProperty(string app, string property)
        {
            return property == AppPropertyKeys.Dir
                || property == AppPropertyKeys.Path
                || property == AppPropertyKeys.Exe
                || property == AppPropertyKeys.SetupTestFile
                || property == AppPropertyKeys.AdornedExecutables
                || property == AppPropertyKeys.LauncherExecutable
                || property == AppPropertyKeys.LauncherIcon;
        }

        private string GetBaseForPathProperty(string app, string property)
        {
            if (property == AppPropertyKeys.Dir)
            {
                return Config.GetStringValue(ConfigPropertyKeys.AppsInstallDir);
            }
            else
            {
                return GetStringGroupValue(app, AppPropertyKeys.Dir);
            }
        }
    }
}
