﻿using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Mastersign.Bench.UI;
using Mastersign.Bench.Windows;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class implements the core logic of Bench tasks.
    /// It is designed as a static class and acts hereby as kind of function library.
    /// </summary>
    public static class BenchTasks
    {
        /// <summary>
        /// A list with app IDs which will not be uninstalled during Bench tasks.
        /// </summary>
        public static List<string> UninstallBlacklist = new List<string>();

        /// <summary>
        /// Checks, whether the installed .NET framework has at least the version 4.5,
        /// and therefore supports the BenchDashboard user interface.
        /// </summary>
        public static bool IsDashboardSupported
        {
            get { return ClrInfo.IsVersionSupported(new Version(4, 5)); }
        }

        /// <summary>
        /// This method is the first step for initializing or upgrading a Bench installation.
        /// </summary>
        /// <remarks>
        /// It looks for a site configuration file. If it does not find a matching file,
        /// it starts a graphical wizzard to request basic configuration parameters from the user.
        /// Then it creates a initial site configuration.
        /// </remarks>
        /// <param name="benchRootDir">The root directory for the Bench installation.</param>
        /// <returns>A <see cref="BenchConfiguration"/> object, initialized with the default configuration,
        /// the default app library and the site configuration.</returns>
        public static BenchConfiguration InitializeSiteConfiguration(string benchRootDir)
        {
            var cfg = new BenchConfiguration(benchRootDir, false, false, false);

            var siteConfigFiles = cfg.FindSiteConfigFiles();
            var customConfigFile = cfg.GetStringValue(PropertyKeys.CustomConfigFile);

            var initSiteConfig = siteConfigFiles.Length == 0;
            var initCustomConfig = !File.Exists(customConfigFile);

            if (initSiteConfig || initCustomConfig)
            {
                var wizzardTask = new InitializeConfigTask(cfg,
                    initSiteConfig, initCustomConfig);
                if (!WizzardForm.ShowWizzard(wizzardTask))
                {
                    return null;
                }
            }

            var resultCfg = new BenchConfiguration(benchRootDir, false, false, true);
            cfg.InjectBenchInitializationProperties(resultCfg);

            return resultCfg;
        }

        /// <summary>
        /// This method is the last fourth for initializing or upgrading a Bench installation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method looks for the user configuration file.
        /// If it does not find a matching file, it creates it and other user configuration
        /// files like app activation lists from templates.
        /// Then it creates all missing directories, that are missing for the Bench installation.
        /// </para>
        /// <para>
        /// Precondition: For cloning an existing Bench configuration, Git must be set up.
        /// </para>
        /// </remarks>
        /// <param name="man">A <see cref="IBenchManager"/> object to use when running initializations.</param>
        /// <returns>A <see cref="BenchConfiguration"/> object, fully initialized
        /// with all configuration files and app libraries.</returns>
        public static BenchConfiguration InitializeCustomConfiguration(IBenchManager man)
        {
            var customConfigDir = man.Config.GetStringValue(PropertyKeys.CustomConfigDir);
            var customConfigFile = man.Config.GetStringValue(PropertyKeys.CustomConfigFile);

            if (!File.Exists(customConfigFile))
            {
                var repo = man.Config.GetStringValue(PropertyKeys.CustomConfigRepository);
                if (repo != null)
                {
                    // asure no config directory exist for git clone
                    if (Directory.Exists(customConfigDir))
                    {
                        FileSystem.PurgeDir(customConfigDir);
                    }
                    // asure the parent directory exists
                    if (!Directory.Exists(Path.GetDirectoryName(customConfigDir)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(customConfigDir));
                    }
                    // clone the existing config
                    var result = man.ProcessExecutionHost.RunProcess(man.Env, man.Config.BenchRootDir,
                        man.Config.Apps[AppKeys.Git].Exe,
                        CommandLine.FormatArgumentList("clone", repo, customConfigDir),
                        ProcessMonitoring.ExitCodeAndOutput);
                    if (result.ExitCode != 0)
                    {
                        man.UI.ShowError("Cloning Custom Configuration",
                            "Executing Git failed: "
                            + Environment.NewLine + Environment.NewLine
                            + result.Output);
                        return null;
                    }
                }
                else
                {
                    if (!Directory.Exists(customConfigDir))
                    {
                        Directory.CreateDirectory(customConfigDir);
                    }

                    var customConfigTemplateFile = man.Config.GetStringValue(PropertyKeys.CustomConfigTemplateFile);
                    File.Copy(customConfigTemplateFile, customConfigFile, false);
                }
            }

            var cfg = new BenchConfiguration(man.Config.BenchRootDir, false, true, true);
            man.Config.InjectBenchInitializationProperties(cfg);

            var homeDir = cfg.GetStringValue(PropertyKeys.HomeDir);
            FileSystem.AsureDir(homeDir);
            FileSystem.AsureDir(Path.Combine(homeDir, "Desktop"));
            FileSystem.AsureDir(Path.Combine(homeDir, "Documents"));
            FileSystem.AsureDir(cfg.GetStringValue(PropertyKeys.AppDataDir));
            FileSystem.AsureDir(cfg.GetStringValue(PropertyKeys.LocalAppDataDir));
            FileSystem.AsureDir(cfg.GetStringValue(PropertyKeys.TempDir));
            FileSystem.AsureDir(cfg.GetStringValue(PropertyKeys.DownloadDir));
            FileSystem.AsureDir(cfg.GetStringValue(PropertyKeys.LibDir));
            FileSystem.AsureDir(cfg.GetStringValue(PropertyKeys.ProjectRootDir));

            var customAppIndexFile = Path.Combine(
                cfg.GetStringValue(PropertyKeys.CustomConfigDir),
                cfg.GetStringValue(PropertyKeys.AppLibIndexFileName));
            if (!File.Exists(customAppIndexFile))
            {
                var customAppIndexTemplateFile = cfg.GetStringValue(PropertyKeys.CustomAppIndexTemplateFile);
                File.Copy(customAppIndexTemplateFile, customAppIndexFile, false);
            }
            var activationFile = cfg.GetStringValue(PropertyKeys.AppActivationFile);
            if (!File.Exists(activationFile))
            {
                var activationTemplateFile = cfg.GetStringValue(PropertyKeys.AppActivationTemplateFile);
                File.Copy(activationTemplateFile, activationFile, false);
            }
            var deactivationFile = cfg.GetStringValue(PropertyKeys.AppDeactivationFile);
            if (!File.Exists(deactivationFile))
            {
                var deactivationTemplateFile = cfg.GetStringValue(PropertyKeys.AppDeactivationTemplateFile);
                File.Copy(deactivationTemplateFile, deactivationFile, false);
            }
            var conEmuConfigFile = cfg.GetStringValue(PropertyKeys.ConEmuConfigFile);
            if (!File.Exists(conEmuConfigFile))
            {
                var conEmuConfigTemplateFile = cfg.GetStringValue(PropertyKeys.ConEmuConfigTemplateFile);
                File.Copy(conEmuConfigTemplateFile, conEmuConfigFile, false);
            }

            var resultCfg = new BenchConfiguration(man.Config.BenchRootDir, true, true, true);
            cfg.InjectBenchInitializationProperties(resultCfg);
            return resultCfg;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Downloader"/> properly configured,
        /// according to the given Bench configuration.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <returns>The created downloader instance.</returns>
        public static Downloader InitializeDownloader(BenchConfiguration config)
        {
            var parallelDownloads = config.GetInt32Value(PropertyKeys.ParallelDownloads, 1);
            var downloadAttempts = config.GetInt32Value(PropertyKeys.DownloadAttempts, 1);
            var useProxy = config.GetBooleanValue(PropertyKeys.UseProxy);
            var httpProxy = config.GetStringValue(PropertyKeys.HttpProxy);
            var httpsProxy = config.GetStringValue(PropertyKeys.HttpsProxy);
            var proxyBypass = config.GetStringListValue(PropertyKeys.ProxyBypass);
            var downloader = new Downloader(parallelDownloads);
            downloader.DownloadAttempts = downloadAttempts;
            if (useProxy)
            {
                downloader.HttpProxy = new WebProxy(httpProxy, true, proxyBypass);
                downloader.HttpsProxy = new WebProxy(httpsProxy, true, proxyBypass);
            }
            downloader.UrlResolver.Clear();
            downloader.UrlResolver.Add(EclipseMirrorResolver);
            downloader.UrlResolver.Add(EclipseDownloadLinkResolver);
            return downloader;
        }

        private static IUrlResolver EclipseDownloadLinkResolver = new HtmlLinkUrlResolver(
            new UrlPattern(
                new Regex(@"^www\.eclipse\.org$"),
                new Regex(@"^/downloads/download\.php"),
                new Dictionary<string, Regex> { { "file", null } }),
            new UrlPattern(
                null,
                new Regex(@"download\.php$"),
                new Dictionary<string, Regex> {
                    { "file", null },
                    { "mirror_id", new Regex(@"^\d+$") }
                }));

        private static IUrlResolver EclipseMirrorResolver = new SurroundedHtmlLinkUrlResolver(
            new UrlPattern(
                new Regex(@"^www\.eclipse\.org$"),
                new Regex(@"^/downloads/download\.php"),
                new Dictionary<string, Regex>
                {
                    {"file", null },
                    {"mirror_id", new Regex(@"^\d+$") }
                }),
                new Regex(@"\<span\s[^\>]*class=""direct-link""[^\>]*\>(.*?)\</span\>"));

        private static WebClient InitializeWebClient(BenchConfiguration config)
        {
            var useProxy = config.GetBooleanValue(PropertyKeys.UseProxy);
            var httpProxy = config.GetStringValue(PropertyKeys.HttpProxy);
            var httpsProxy = config.GetStringValue(PropertyKeys.HttpsProxy);
            var proxyBypass = config.GetStringListValue(PropertyKeys.ProxyBypass);
            var webClient = new WebClient();
            webClient.Proxy = useProxy
                ? new SchemeDispatchProxy(new Dictionary<string, IWebProxy>
                    {
                        {"http", new WebProxy(httpProxy, true, proxyBypass)},
                        {"https", new WebProxy(httpsProxy, true, proxyBypass)}
                    })
                : null;
            return webClient;
        }

        /// <summary>
        /// Downloads a string via HTTP(S) asynchronously.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="url">The URL of the HTTP resource.</param>
        /// <param name="resultHandler">The handler to process the download result.</param>
        public static void DownloadStringAsync(BenchConfiguration config, Uri url, StringDownloadResultHandler resultHandler)
        {
            var wc = InitializeWebClient(config);
            wc.DownloadStringCompleted += (sender, eventArgs) =>
            {
                resultHandler(eventArgs.Error == null && !eventArgs.Cancelled, eventArgs.Result);
                wc.Dispose();
            };
            wc.DownloadStringAsync(url);
        }


        /// <summary>
        /// Downloads a string via HTTP(S).
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="url">The URL of the HTTP resource.</param>
        /// <returns>The resources content or <c>null</c> if the download failed.</returns>
        public static string DownloadString(BenchConfiguration config, Uri url)
        {
            using (var wc = InitializeWebClient(config))
            {
                try
                {
                    return wc.DownloadString(url);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Downloads a file via HTTP(S) asynchronously.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="url">The URL of the HTTP resource.</param>
        /// <param name="targetFile">A path to the target file.</param>
        /// <param name="resultHandler">The handler to process the download result.</param>
        public static void DownloadFileAsync(BenchConfiguration config, Uri url, string targetFile,
            FileDownloadResultHandler resultHandler)
        {
            var wc = InitializeWebClient(config);
            wc.DownloadFileCompleted += (sender, eventArgs) =>
            {
                resultHandler(eventArgs.Error == null && !eventArgs.Cancelled);
                wc.Dispose();
            };
            wc.DownloadStringAsync(url);
        }

        /// <summary>
        /// Downloads a file via HTTP(S).
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="url">The URL of the HTTP resource.</param>
        /// <param name="targetFile">A path to the target file.</param>
        /// <returns><c>true</c> if the download was successful; otherwise <c>false</c>.</returns>
        public static bool DownloadFile(BenchConfiguration config, Uri url, string targetFile)
        {
            using (var wc = InitializeWebClient(config))
            {
                try
                {
                    wc.DownloadFile(url, targetFile);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Downloads the version number of the latest Bench release asynchronously.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="resultHandler">The handler for the download result.</param>
        public static void GetLatestVersionAsync(BenchConfiguration config, StringDownloadResultHandler resultHandler)
        {
            var uri = new Uri(config.GetStringValue(PropertyKeys.VersionUrl));
            DownloadStringAsync(config, uri,
                (success, content) => resultHandler(success, content != null ? content.Trim() : null));
        }

        /// <summary>
        /// Downloads the version number of the latest Bench release.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <returns>The version number or <c>null</c> if the download failed.</returns>
        public static string GetLatestVersion(BenchConfiguration config)
        {
            var uri = new Uri(config.GetStringValue(PropertyKeys.VersionUrl));
            var result = DownloadString(config, uri);
            return result != null ? result.Trim() : null;
        }

        private static string RunCustomScript(BenchConfiguration config, IProcessExecutionHost execHost,
            string appId, string path, params string[] args)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Custom script not found.", path);
            }
            var customScriptRunner = Path.Combine(
                config.GetStringValue(PropertyKeys.BenchScripts),
                "Run-CustomScript.ps1");
            var result = execHost.RunProcess(new BenchEnvironment(config),
                config.BenchRootDir, customScriptRunner,
                $"@(\"{path}\", {PowerShell.FormatStringList(args)})",
                ProcessMonitoring.ExitCodeAndOutput);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException("Executing custom script failed.",
                    path + " " + CommandLine.FormatArgumentList(args),
                    result.ExitCode, result.Output);
            }
            return result.Output;
        }

        private static string RunGlobalCustomScript(BenchConfiguration config, IProcessExecutionHost execHost,
            string path, params string[] args)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Global custom script not found.", path);
            }
            var customScriptRunner = Path.Combine(
                config.GetStringValue(PropertyKeys.BenchScripts),
                "Run-CustomScript.ps1");
            var result = execHost.RunProcess(new BenchEnvironment(config),
                config.BenchRootDir, customScriptRunner,
                $"@(\"{path}\", {PowerShell.FormatStringList(args)})",
                ProcessMonitoring.ExitCodeAndOutput);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException("Executing global custom script failed.",
                    path + " " + CommandLine.FormatArgumentList(args),
                    result.ExitCode, result.Output);
            }
            return result.Output;
        }

        private static string CustomScriptDir(BenchConfiguration config)
        {
            return Path.Combine(config.GetStringValue(PropertyKeys.BenchAuto), "apps");
        }

        private static string GetGlobalCustomScriptFile(BenchConfiguration config, string typ)
        {
            var path = Path.Combine(config.GetStringValue(PropertyKeys.CustomConfigDir), typ + ".ps1");
            return File.Exists(path) ? path : null;
        }

        private static Process StartProcess(BenchEnvironment env,
            string cwd, string exe, string arguments)
        {
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("The executable could not be found.", exe);
            }
            var si = new ProcessStartInfo(exe, arguments);
            si.UseShellExecute = false;
            si.WorkingDirectory = cwd;
            env.Load(si.EnvironmentVariables);
            return Process.Start(si);
        }

        private static Process StartProcessViaShell(BenchEnvironment env,
            string cwd, string exe, string arguments,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("The executable could not be found.", exe);
            }
            var si = new ProcessStartInfo(exe, arguments);
            si.UseShellExecute = true;
            si.WindowStyle = windowStyle;
            si.WorkingDirectory = cwd;
            return Process.Start(si);
        }

        /// <summary>
        /// Launches the launcher executable of the specified app.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="env">The environment variables of the Bench system.</param>
        /// <param name="appId">The ID of the app to launch.</param>
        /// <param name="args">An array with additional command line arguments, to pass to the launcher executable.</param>
        /// <returns>The <see cref="Process"/> object of the started executable.</returns>
        public static Process LaunchApp(BenchConfiguration config, BenchEnvironment env,
                    string appId, string[] args)
        {
            var app = config.Apps[appId];
            var exe = app.LauncherExecutable;
            var isAdorned = app.IsExecutableAdorned(exe) && app.IsAdornmentRequired;
            if (isAdorned) exe = app.GetLauncherScriptFile();

            if (string.IsNullOrEmpty(exe))
            {
                throw new ArgumentException("The launcher executable is not set.");
            }
            if (isAdorned)
            {
                return StartProcessViaShell(env, config.GetStringValue(PropertyKeys.HomeDir),
                    exe, CommandLine.SubstituteArgumentList(app.LauncherArguments, args),
                    ProcessWindowStyle.Minimized);
            }
            else
            {
                return StartProcess(env, config.GetStringValue(PropertyKeys.HomeDir),
                    exe, CommandLine.SubstituteArgumentList(app.LauncherArguments, args));
            }
        }

        private static string GemExe(BenchConfiguration config)
        {
            var rubyExe = config.GetStringGroupValue(AppKeys.Ruby, PropertyKeys.AppExe);
            return rubyExe != null
                ? Path.Combine(Path.GetDirectoryName(rubyExe), "gem.cmd")
                : null;
        }

        private static string PipExe(BenchConfiguration config, PythonVersion pyVer)
        {
            switch (pyVer)
            {
                case PythonVersion.Python2:
                    return Path.Combine(
                        Path.Combine(config.GetStringValue(PropertyKeys.LibDir), config.Apps[AppKeys.Python2].Dir),
                        @"Scripts\pip2.exe");
                case PythonVersion.Python3:
                    return Path.Combine(
                        Path.Combine(config.GetStringValue(PropertyKeys.LibDir), config.Apps[AppKeys.Python3].Dir),
                        @"Scripts\pip3.exe");
                default:
                    throw new NotSupportedException();
            }
        }

        #region Higher Order Actions

        /// <summary>
        /// Runs the Bench task of downloading and extracting the app libraries.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoLoadAppLibraries(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new AppFacade[0],
                notify, cancelation,
                LoadAppLibraries);
        }

        /// <summary>
        /// Runs the Bench task of setting up only the apps, required of the Bench system itself.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoSetupRequiredApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new ICollection<AppFacade>[]
                {
                    man.Config.Apps.RequiredApps,
                    man.Config.Apps.RequiredApps
                },
                notify, cancelation,
                UninstallApps,
                DownloadAppResources,
                InstallApps,
                UpdateEnvironment);
        }

        private static ICollection<AppFacade> AutoUninstallApps(AppIndexFacade apps)
        {
            var uninstallApps = new List<AppFacade>(apps.InactiveApps);
            foreach (var app in apps.ActiveApps)
            {
                if (!app.IsVersionUpToDate)
                {
                    uninstallApps.Add(app);
                }
            }
            return uninstallApps;
        }

        /// <summary>
        /// Runs the Bench task of automatically setting up all active apps including downloading missing resources,
        /// and setting up the environment afterwards.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoAutoSetup(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {

            return RunTasks(man,
                new ICollection<AppFacade>[]
                {
                    AutoUninstallApps(man.Config.Apps),
                    man.Config.Apps.ActiveApps
                },
                notify, cancelation,
                UninstallApps,
                DownloadAppResources,
                InstallApps,
                UpdateEnvironment);
        }

        /// <summary>
        /// Runs the Bench task of downloading the missing app resources of all active apps.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoDownloadAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                man.Config.Apps.ActiveApps,
                notify, cancelation,
                DownloadAppResources);
        }

        /// <summary>
        /// Runs the Bench task of downloading the missing app resource of a specific app.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="appId">The ID of the targeted app.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoDownloadAppResources(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                appId,
                notify, cancelation,
                DownloadAppResources);
        }

        /// <summary>
        /// Runs the Bench task of downloading the missing app resources of all apps known to Bench,
        /// whether tey are active or not.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoDownloadAllAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                DownloadAppResources);
        }

        /// <summary>
        /// Runs the Bench task of deleting all cached app resources.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoDeleteAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                DeleteAppResources);
        }

        /// <summary>
        /// Runs the Bench task of deleting the cached app resource of a specific app.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="appId">The ID of the targeted app.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoDeleteAppResources(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                appId,
                notify, cancelation,
                DeleteAppResources);
        }

        /// <summary>
        /// Runs the Bench task of deleting all cached app resources, not referenced by any app anymore.
        /// These app resources where typically downloaded before the definition of some apps where updated,
        /// now referencing to newer release versions.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoCleanUpAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                CleanUpAppResources);
        }

        /// <summary>
        /// Runs the Bench task of installing all active apps, not installed already.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoInstallApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                man.Config.Apps.ActiveApps,
                notify, cancelation,
                DownloadAppResources,
                InstallApps);
        }

        /// <summary>
        /// Runs the Bench task of installing a specific app, including all of its dependencies.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="appId">The ID of the targeted app.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoInstallApps(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var app = man.Config.Apps[appId];
            if (app == null) throw new ArgumentException("App not found: " + appId, "appId");
            var dependencies = man.Config.Apps.GetApps(app.FindAllDependencies());
            return RunTasks(man,
                dependencies,
                notify, cancelation,
                DownloadAppResources,
                InstallApps);
        }

        /// <summary>
        /// Runs the Bench task of uninstalling all installed apps.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoUninstallApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                UninstallApps);
        }

        /// <summary>
        /// Runs the Bench task of uninstalling a specific app, including all apps, depending on the specified one.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="appId">The ID of the targeted app.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoUninstallApps(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var app = man.Config.Apps[appId];
            if (app == null) throw new ArgumentException("App not found: " + appId, "appId");
            var responsibilities = man.Config.Apps.GetApps(app.FindAllResponsibilities());
            return RunTasks(man,
                responsibilities,
                notify, cancelation,
                UninstallApps);
        }

        /// <summary>
        /// Runs the Bench task of uninstalling all installed apps and then installing all active apps.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoReinstallApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                man.Config.Apps.ActiveApps,
                notify, cancelation,
                DownloadAppResources,
                UninstallApps,
                InstallApps);
        }

        /// <summary>
        /// Runs the Bench task of uninstalling a specific app and all apps, depending on it,
        /// and then installing the specified app.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="appId">The ID of the targeted app.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoReinstallApps(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var app = man.Config.Apps[appId];
            if (app == null) throw new ArgumentException("App not found: " + appId, "appId");
            var dependencies = man.Config.Apps.GetApps(app.FindAllDependencies());
            var responsibilities = man.Config.Apps.GetApps(app.FindAllResponsibilities());
            return RunTasks(man,
                new ICollection<AppFacade>[]
                {
                    dependencies,
                    responsibilities,
                    dependencies
                },
                notify, cancelation,
                DownloadAppResources,
                UninstallApps,
                InstallApps);
        }

        /// <summary>
        /// Runs the Bench task of upgrading all upgradable apps.
        /// An app is upgradable if <see cref="AppFacade.CanUpgrade"/> is <c>true</c>.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoUpgradeApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var upgradable = new List<AppFacade>();
            foreach (var app in man.Config.Apps) if (app.CanUpgrade) upgradable.Add(app);

            var activeApps = man.Config.Apps.ActiveApps;
            return RunTasks(man,
                new ICollection<AppFacade>[]
                {
                    upgradable,
                    activeApps,
                    upgradable,
                    activeApps
                },
                notify, cancelation,
                DeleteAppResources,
                DownloadAppResources,
                UninstallApps,
                InstallApps);
        }

        /// <summary>
        /// Runs the Bench task of upgrading a specific app.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="appId">The ID of the targeted app.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoUpgradeApps(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var app = man.Config.Apps[appId];
            if (app == null) throw new ArgumentException("App not found: " + appId, "appId");
            var dependencies = man.Config.Apps.GetApps(app.FindAllDependencies());
            var responsibilities = man.Config.Apps.GetApps(app.FindAllResponsibilities());
            return RunTasks(man,
                new ICollection<AppFacade>[]
                {
                    new [] { app },
                    dependencies,
                    responsibilities,
                    dependencies
                },
                notify, cancelation,
                DeleteAppResources,
                DownloadAppResources,
                UninstallApps,
                InstallApps);
        }

        /// <summary>
        /// Runs the Bench task of setting up the environment, including the generation of <c>env.cmd</c>,
        /// (re)creating all launcher scripts and shortcuts, and running the custom environment scripts
        /// of all active apps.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoUpdateEnvironment(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new AppFacade[0],
                notify, cancelation,
                UpdateEnvironment);
        }

        /// <summary>
        /// Runs the Bench task of downloading the latest Bench binary and bootstrap file.
        /// </summary>
        /// <param name="man">The Bench manager.</param>
        /// <param name="notify">The notification handler.</param>
        /// <param name="cancelation">A cancelation token.</param>
        /// <returns>The result of running the task, in shape of an <see cref="ActionResult"/> object.</returns>
        public static ActionResult DoDownloadBenchUpdate(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new AppFacade[0],
                notify, cancelation,
                DownloadBenchUpdate);
        }

        #endregion

        #region Task Composition

        private static ActionResult RunTasks(IBenchManager man,
            ICollection<AppFacade>[] taskApps,
            Action<TaskInfo> notify, Cancelation cancelation,
            params BenchTask[] tasks)
        {
            if (tasks.Length == 0) return new ActionResult();

            var logLevel = LogLevels.GuessLevel(man.Config.GetStringValue(PropertyKeys.LogLevel));
            TaskInfoLogger logger = null;
            if (logLevel != LogLevels.None)
            {
                var file = man.Config.GetStringValue(PropertyKeys.LogFile,
                    DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_setup.txt");
                if (!Path.IsPathRooted(file))
                {
                    var logDir = man.Config.GetStringValue(PropertyKeys.LogDir);
                    FileSystem.AsureDir(logDir);
                    file = Path.Combine(logDir, file);
                }
                logger = new TaskInfoLogger(file, logLevel == LogLevels.Error);
            }

            var infos = new List<TaskInfo>();
            var errorIds = new List<string>();
            var taskProgress = 0f;

            Action<TaskInfo> myNotify = info =>
            {
                if (info is TaskProgress)
                {
                    info = ((TaskProgress)info).ScaleProgress(taskProgress, 1f / tasks.Length);
                }
                if (info is TaskError && info.AppId != null && !errorIds.Contains(info.AppId))
                {
                    errorIds.Add(info.AppId);
                }
                infos.Add(info);
                if (logger != null)
                {
                    logger.Log(info);
                }
                notify(info);
            };

            for (int i = 0; i < tasks.Length; i++)
            {
                taskProgress = (float)i / tasks.Length;
                if (cancelation.IsCanceled) break;
                var apps = new List<AppFacade>(FindLastNotNull(taskApps, i));
                foreach (var err in errorIds)
                {
                    apps.RemoveAll(app => app.ID == err);
                }
                tasks[i](man, apps, myNotify, cancelation);
            }
            notify(new TaskProgress("Finished.", 1f));

            if (logger != null) logger.Dispose();

            return new ActionResult(infos, cancelation.IsCanceled);
        }

        private static ActionResult RunTasks(IBenchManager man,
            ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation,
            params BenchTask[] tasks)
        {
            return RunTasks(man,
                new ICollection<AppFacade>[] { apps },
                notify, cancelation, tasks);
        }

        private static ActionResult RunTasks(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation,
            params BenchTask[] tasks)
        {
            return RunTasks(man,
                new[] { man.Config.Apps[appId] },
                notify, cancelation, tasks);
        }

        private static T FindLastNotNull<T>(T[] a, int p)
        {
            p = Math.Min(a.Length - 1, p);
            while (p > 0 && a[p] == null) p--;
            return a[p];
        }

        #endregion

        #region Load App Libraries

        private static void UnwrapSubDir(string targetDir)
        {
            var content = Directory.GetFileSystemEntries(targetDir);
            // Check if the only content of the source folder is one directory
            if (content != null && content.Length == 1 && Directory.Exists(content[0]))
            {
                // Then move all further content one directory up
                // (helps with ZIP files which contain the whole app library in a sub-dir)
                FileSystem.MoveContent(content[0], targetDir);
                Directory.Delete(content[0]);
            }
        }

        private static bool IsAppLibrary(BenchConfiguration config, string directory)
        {
            var appIndexFileName = config.GetStringValue(PropertyKeys.AppLibIndexFileName);
            return File.Exists(Path.Combine(directory, appIndexFileName));
        }

        private static void ExtractAppLibrary(BenchConfiguration config, string source, string targetDir)
        {
            if (!ZipFile.IsZipFile(source)) throw new InvalidOperationException(
                "The app library does not appear to be a ZIP file: " + source);
            FileSystem.EmptyDir(targetDir);
            using (var zf = new ZipFile(source))
            {
                zf.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                zf.FlattenFoldersOnExtract = false;
                zf.ExtractAll(targetDir);
            }
            if (!IsAppLibrary(config, targetDir)) UnwrapSubDir(targetDir);
        }

        private static void CopyAppLibrary(BenchConfiguration config, string source, string targetDir)
        {
            if (File.Exists(source))
            {
                ExtractAppLibrary(config, source, targetDir);
            }
            else if (Directory.Exists(source))
            {
                FileSystem.CopyDir(source, targetDir, true);
                if (!IsAppLibrary(config, targetDir)) UnwrapSubDir(targetDir);
            }
            else
            {
                throw new ArgumentException("Source of app library not found: " + source);
            }
        }

        private static string AppLibDirectory(BenchConfiguration config, string appLibId)
        {
            return Path.Combine(config.GetStringValue(PropertyKeys.AppLibsDir), appLibId);
        }

        /// <summary>
        /// Deletes the loaded and cached app libraries
        /// in preparation for re-loading them for an update or a repair.
        /// </summary>
        /// <param name="cfg">The Bench configuration</param>
        public static void DeleteAppLibraries(BenchConfiguration cfg)
        {
            var appLibDir = cfg.GetStringValue(PropertyKeys.AppLibsDir);
            FileSystem.EmptyDir(appLibDir);
            var cacheDir = cfg.GetStringValue(PropertyKeys.AppLibsDownloadDir);
            FileSystem.EmptyDir(cacheDir);
        }

        private static void LoadAppLibraries(IBenchManager man,
            ICollection<AppFacade> _,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var appLibsDir = man.Config.GetStringValue(PropertyKeys.AppLibsDir);
            FileSystem.AsureDir(appLibsDir);
            var cacheDir = man.Config.GetStringValue(PropertyKeys.AppLibsDownloadDir);
            FileSystem.AsureDir(cacheDir);

            var appLibs = man.Config.AppLibraries;

            // Clean unconfigured app library directories
            foreach (var d in Directory.GetDirectories(appLibsDir))
            {
                var n = Path.GetFileName(d);
                var found = false;
                foreach (var l in appLibs)
                {
                    if (string.Equals(l.ID, n, StringComparison.InvariantCultureIgnoreCase))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    FileSystem.PurgeDir(d);
                    notify(new TaskInfo(string.Format("Deleted unknown app library '{0}'.", n)));
                }
            }

            var finished = 0;
            var errorCnt = 0;
            var taskCnt = appLibs.Length;
            var tasks = new List<DownloadTask>();
            var endEvent = new ManualResetEvent(false);

            EventHandler<DownloadEventArgs> downloadStartedHandler = (o, e) =>
            {
                notify(new TaskProgress(
                    string.Format("Started download for app library '{0}' ...", e.Task.Id),
                    progress: (float)finished / taskCnt,
                    detailedMessage: e.Task.Url.ToString()));
            };

            EventHandler<DownloadEventArgs> downloadEndedHandler = (o, e) =>
            {
                finished++;
                if (!e.Task.Success)
                {
                    errorCnt++;
                    notify(new TaskError(e.Task.ErrorMessage));
                }
                else
                {
                    try
                    {
                        ExtractAppLibrary(man.Config, e.Task.TargetFile, AppLibDirectory(man.Config, e.Task.Id));
                        notify(new TaskProgress(
                            string.Format("Finished download for app library '{0}'.", e.Task.Id),
                            progress: (float)finished / taskCnt));
                    }
                    catch (Exception exc)
                    {
                        errorCnt++;
                        notify(new TaskError(
                            string.Format("Extracting the archive of app library '{0}' failed.", e.Task.Id),
                            exception: exc));
                    }
                }
            };

            EventHandler workFinishedHandler = null;
            workFinishedHandler = (o, e) =>
            {
                man.Downloader.DownloadEnded -= downloadEndedHandler;
                man.Downloader.WorkFinished -= workFinishedHandler;
                endEvent.Set();
            };
            man.Downloader.DownloadStarted += downloadStartedHandler;
            man.Downloader.DownloadEnded += downloadEndedHandler;
            man.Downloader.WorkFinished += workFinishedHandler;

            cancelation.Canceled += (s, e) => man.Downloader.CancelAll();

            notify(new TaskProgress("Loading app libraries...",
                progress: 0f));

            foreach (var l in appLibs)
            {
                var appLibDir = AppLibDirectory(man.Config, l.ID);

                // Skip app library if is already loaded
                if (File.Exists(Path.Combine(appLibDir,
                    man.Config.GetStringValue(PropertyKeys.AppLibIndexFileName))))
                {
                    notify(new TaskProgress(
                        string.Format("App library '{0}' already loaded.", l.ID),
                        progress: (float)finished / taskCnt));
                    continue;
                }
                else
                {
                    // Clean the app library directory, if the directory is not valid
                    notify(new TaskInfo(
                        string.Format("Cleaning invalid app library '{0}'.", l.ID)));
                    FileSystem.PurgeDir(appLibDir);
                }

                var appLibArchive = l.ID + ".zip";

                if ("file".Equals(l.Url.Scheme.ToLowerInvariant()))
                {
                    var sourcePath = l.Url.LocalPath;
                    notify(new TaskInfo(
                        string.Format("Loading app libary '{0}' from file system...", l.ID)));
                    finished++;
                    try
                    {
                        CopyAppLibrary(man.Config, sourcePath, appLibDir);
                        notify(new TaskProgress(
                            string.Format("Successfully loaded app library '{0}'.", l.ID),
                            progress: (float)finished / taskCnt));
                    }
                    catch (Exception e)
                    {
                        errorCnt++;
                        notify(new TaskError(
                            string.Format("Loading app library '{0}' failed.", l.ID),
                            exception: e));
                    }
                }
                else
                {
                    var appLibArchivePath = Path.Combine(cacheDir, appLibArchive);
                    // Check if app library is cached
                    if (File.Exists(appLibArchivePath))
                    {
                        finished++;
                        // Extract if it is cached
                        try
                        {
                            ExtractAppLibrary(man.Config, appLibArchivePath, appLibDir);
                            notify(new TaskProgress(
                                string.Format("Extracted app library '{0}' from cache.", l.ID),
                                progress: (float)finished / taskCnt));
                        }
                        catch (Exception exc)
                        {
                            errorCnt++;
                            notify(new TaskError(
                                string.Format("Extracting the archive of app library '{0}' failed.", l.ID),
                                exception: exc));
                        }
                    }
                    else
                    {
                        // Queue download task if not
                        var task = new DownloadTask(l.ID, l.Url, appLibArchivePath);
                        tasks.Add(task);
                        man.Downloader.Enqueue(task);
                    }
                }
            }

            if (tasks.Count == 0)
            {
                man.Downloader.DownloadEnded -= downloadEndedHandler;
                man.Downloader.WorkFinished -= workFinishedHandler;
                notify(new TaskProgress("Nothing to download.",
                    progress: 1f));
            }
            else
            {
                notify(new TaskProgress(string.Format("Queued {0} downloads.", tasks.Count),
                    progress: 0f));
                endEvent.WaitOne();
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished loading app libraries.",
                    progress: 1f));
            }
        }

        #endregion

        #region Download App Resources

        private static void DownloadAppResources(IBenchManager man, ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var targetDir = man.Config.GetStringValue(PropertyKeys.DownloadDir);
            FileSystem.AsureDir(targetDir);

            var tasks = new List<DownloadTask>();
            var finished = 0;
            var errorCnt = 0;
            var endEvent = new ManualResetEvent(false);

            EventHandler<DownloadEventArgs> downloadStartedHandler = (o, e) =>
            {
                notify(new TaskProgress(
                    string.Format("Started download for {0} ...", e.Task.Id),
                    (float)finished / tasks.Count, e.Task.Id, e.Task.Url.ToString()));
            };

            EventHandler<DownloadEventArgs> downloadEndedHandler = (o, e) =>
            {
                finished++;
                if (!e.Task.Success)
                {
                    errorCnt++;
                    notify(new TaskError(e.Task.ErrorMessage, e.Task.Id));
                }
                else
                {
                    notify(new TaskProgress(
                        string.Format("Finished download for {0}.", e.Task.Id),
                        (float)finished / tasks.Count, e.Task.Id));
                }
            };

            EventHandler workFinishedHandler = null;
            workFinishedHandler = (EventHandler)((o, e) =>
            {
                man.Downloader.DownloadEnded -= downloadEndedHandler;
                man.Downloader.WorkFinished -= workFinishedHandler;
                foreach (var app in apps)
                {
                    app.DiscardCachedValues();
                }
                endEvent.Set();
            });
            man.Downloader.DownloadStarted += downloadStartedHandler;
            man.Downloader.DownloadEnded += downloadEndedHandler;
            man.Downloader.WorkFinished += workFinishedHandler;

            cancelation.Canceled += (s, e) =>
            {
                man.Downloader.CancelAll();
            };

            notify(new TaskProgress("Downloading app resources...", 0f));

            var selectedApps = new List<AppFacade>();
            foreach (var app in apps)
            {
                if (app.CanDownloadResource) selectedApps.Add(app);
            }

            foreach (var app in selectedApps)
            {
                var targetFile = app.ResourceFileName ?? app.ResourceArchiveName;
                var targetPath = Path.Combine(targetDir, targetFile);

                var task = new DownloadTask(app.ID, new Uri(app.Url), targetPath);
                task.Headers = app.DownloadHeaders;
                task.Cookies = app.DownloadCookies;
                tasks.Add(task);

                man.Downloader.Enqueue(task);
            }

            if (tasks.Count == 0)
            {
                man.Downloader.DownloadEnded -= downloadEndedHandler;
                man.Downloader.WorkFinished -= workFinishedHandler;
                notify(new TaskProgress("Nothing to download.", 1f));
            }
            else
            {
                notify(new TaskProgress(
                    string.Format("Queued {0} downloads.", tasks.Count),
                    0f));
                endEvent.WaitOne();
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished downloading resources.", 1f));
            }
        }

        #endregion

        #region Delete App Resources

        private static void DeleteAppResources(IBenchManager man,
            ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var downloadDir = man.Config.GetStringValue(PropertyKeys.DownloadDir);

            notify(new TaskProgress("Deleting app resources", 0));

            var selectedApps = new List<AppFacade>();
            foreach (var app in apps)
            {
                if (app.CanDeleteResource) selectedApps.Add(app);
            }

            var cnt = 0;
            foreach (var app in selectedApps)
            {
                if (cancelation.IsCanceled) break;

                cnt++;
                var progress = (float)cnt / selectedApps.Count;

                var resourceFile = app.ResourceFileName ?? app.ResourceArchiveName;
                var resourcePath = Path.Combine(downloadDir, resourceFile);
                try
                {
                    File.Delete(resourcePath);
                }
                catch (Exception e)
                {
                    notify(new TaskError(e.Message, appId: app.ID, exception: e));
                    continue;
                }
                notify(new TaskProgress(
                    string.Format("Deleted app resource for {0}.", app.ID),
                    progress, app.ID));
                app.DiscardCachedValues();
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished deleting app resources.", 1f));
            }
        }

        private static void CleanUpAppResources(IBenchManager man,
            ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var downloadDir = man.Config.GetStringValue(PropertyKeys.DownloadDir);

            notify(new TaskProgress("Deleting obsolete app resources", 0));

            var preservedFileNames = new List<string>();
            foreach (var app in apps)
            {
                if (!app.HasResource) continue;
                var resourceName = (app.ResourceFileName ?? app.ResourceArchiveName)
                    .ToLowerInvariant();
                preservedFileNames.Add(resourceName);
            }

            var fileNames = new List<string>();
            foreach (var path in Directory.GetFiles(downloadDir))
            {
                var name = Path.GetFileName(path).ToLowerInvariant();
                if (!preservedFileNames.Contains(name))
                {
                    fileNames.Add(name);
                }
            }

            var cnt = 0;
            foreach (var name in fileNames)
            {
                if (cancelation.IsCanceled) break;
                var progress = (float)cnt / fileNames.Count;

                var resourcePath = Path.Combine(downloadDir, name);
                try
                {
                    File.Delete(resourcePath);
                }
                catch (Exception e)
                {
                    notify(new TaskError(e.Message, exception: e));
                    continue;
                }
                notify(new TaskProgress(
                    string.Format("Deleted obsolete app resource {0}.", name),
                    progress));
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished deleting app resources.", 1f));
            }
        }

        #endregion

        #region Setup Environment

        private static void CleanExecutionProxies(BenchConfiguration config)
        {
            FileSystem.EmptyDir(config.GetStringValue(PropertyKeys.AppAdornmentBaseDir));
        }

        private static void CreateExecutionProxies(BenchConfiguration config, AppFacade app)
        {
            var adornedExePaths = app.AdornedExecutables;
            if (adornedExePaths.Length > 0 && app.IsAdornmentRequired)
            {
                var proxyBaseDir = FileSystem.EmptyDir(app.AdornmentProxyBasePath);
                foreach (var exePath in adornedExePaths)
                {
                    var proxyPath = app.GetExecutableProxy(exePath);
                    var code = new StringBuilder();
                    code.AppendLine("@ECHO OFF");
                    code.AppendLine(string.Format("runps Run-Adorned \"{0}\" \"{1}\" %*", app.ID, exePath));
                    File.WriteAllText(proxyPath, code.ToString());
                }
            }
        }

        private static void CleanLaunchers(BenchConfiguration config)
        {
            FileSystem.EmptyDir(config.GetStringValue(PropertyKeys.LauncherDir));
            FileSystem.EmptyDir(config.GetStringValue(PropertyKeys.LauncherScriptDir));

            var benchControlRootLink = Path.Combine(config.BenchRootDir, "Bench Control.lnk");
            var benchDashboardRootLink = Path.Combine(config.BenchRootDir, "Bench Dashboard.lnk");
            if (File.Exists(benchControlRootLink)) File.Delete(benchControlRootLink);
            if (File.Exists(benchDashboardRootLink)) File.Delete(benchDashboardRootLink);
        }

        private static void CreateBenchDashboardLauncher(BenchConfiguration config)
        {
            if (!IsDashboardSupported) return;
            var benchDashboard = Path.Combine(config.GetStringValue(PropertyKeys.BenchAuto), @"bin\BenchDashboard.exe");
            var benchDashboardShortcut = Path.Combine(config.GetStringValue(PropertyKeys.LauncherDir), "Bench Dashboard.lnk");
            FileSystem.CreateShortcut(benchDashboardShortcut, benchDashboard,
                string.Format("-root \"{0}\"", config.BenchRootDir), config.BenchRootDir,
                benchDashboard);
            File.Copy(benchDashboardShortcut, Path.Combine(config.BenchRootDir, Path.GetFileName(benchDashboardShortcut)), true);
        }

        private static void CreateActionLauncher(BenchConfiguration config, string label, string binFile, string icon = null,
            string targetDir = null)
        {
            var launcherDir = targetDir ?? config.GetStringValue(PropertyKeys.LauncherDir);
            var binDir = config.GetStringValue(PropertyKeys.BenchBin);
            var shortcut = Path.Combine(launcherDir, label + ".lnk");
            var target = Path.Combine(binDir, binFile);
            FileSystem.CreateShortcut(shortcut, target, null, config.BenchRootDir, icon ?? target);
        }

        private static void CreateActionLaunchers(BenchConfiguration config)
        {
            CreateBenchDashboardLauncher(config);

            if (!IsDashboardSupported)
            {
                CreateActionLauncher(config, "Bench CLI", "bench.exe");
                CreateActionLauncher(config, "Bench CLI", "bench.exe", null, config.BenchRootDir);
            }
            if (config.GetBooleanValue(PropertyKeys.QuickAccessCmd, true))
            {
                CreateActionLauncher(config, "Command Line", "bench-cmd.cmd", @"%SystemRoot%\System32\cmd.exe");
            }
            if (config.GetBooleanValue(PropertyKeys.QuickAccessPowerShell, false))
            {
                CreateActionLauncher(config, "PowerShell", "bench-ps.cmd", @"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe");
            }
            if (config.GetBooleanValue(PropertyKeys.QuickAccessBash, false))
            {
                CreateActionLauncher(config, "Bash", "bench-bash.cmd", @"%SystemRoot%\System32\imageres.dll,95");
            }
        }

        private static void CreateLauncher(BenchConfiguration config, AppFacade app)
        {
            var label = app.Launcher;
            if (label == null) return;

            var executable = app.LauncherExecutable;
            var args = CommandLine.FormatArgumentList(app.LauncherArguments);
            var script = app.GetLauncherScriptFile();
            var autoDir = config.GetStringValue(PropertyKeys.BenchAuto);
            var rootDir = config.BenchRootDir;

            var code = new StringBuilder();
            code.AppendLine("@ECHO OFF");
            code.AppendLine($"ECHO.Launching {label} in Bench Context ...");
            code.AppendLine($"CALL \"{rootDir}\\env.cmd\"");
            if (app.IsExecutableAdorned(executable) && app.IsAdornmentRequired)
            {
                code.AppendLine($"\"{autoDir}\\runps.cmd\" Run-Adorned {app.ID} \"{executable}\" {args}");
            }
            else
            {
                code.AppendLine($"START \"{label}\" \"{executable}\" {args}");
            }
            File.WriteAllText(script, code.ToString());

            var shortcut = app.GetLauncherFile();
            FileSystem.CreateShortcut(shortcut, script, null, config.BenchRootDir, app.LauncherIcon,
                FileSystem.ShortcutWindowStyle.Minimized);
        }

        private static void UpdateEnvironment(IBenchManager man,
            ICollection<AppFacade> _,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            try
            {
                man.Env.WriteEnvironmentFile();
            }
            catch (Exception e)
            {
                notify(new TaskError(
                    string.Format("Writing the environment file failed: {0}", e.Message),
                    exception: e));
                return;
            }
            try
            {
                if (man.Config.GetBooleanValue(PropertyKeys.RegisterInUserProfile))
                {
                    man.Env.RegisterInUserProfile();
                }
                else
                {
                    man.Env.UnregisterFromUserProfile();
                }
            }
            catch (Exception e)
            {
                notify(new TaskError(
                    string.Format("Registering the environment in the user profile failed: {0}", e.Message),
                    exception: e));
                return;
            }
            try
            {
                CleanExecutionProxies(man.Config);
            }
            catch (Exception e)
            {
                notify(new TaskError(
                    string.Format("Cleaning execution proxies failed: {0}", e.Message),
                    exception: e));
                return;
            }
            try
            {
                CleanLaunchers(man.Config);
            }
            catch (Exception e)
            {
                notify(new TaskError(
                    string.Format("Cleaning launchers failed: {0}", e.Message),
                    exception: e));
                return;
            }
            try
            {
                CreateActionLaunchers(man.Config);
            }
            catch (Exception e)
            {
                notify(new TaskError(
                    string.Format("Creating bench action launchers failed: {0}", e.Message),
                    exception: e));
                return;
            }
            var selectedApps = man.Config.Apps.ActiveApps;
            var cnt = 0;
            foreach (var app in selectedApps)
            {
                if (cancelation.IsCanceled) break;

                cnt++;
                var progress = 0.9f * cnt / selectedApps.Length;

                try
                {
                    CreateExecutionProxies(man.Config, app);
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Creating execution proxy for {0} failed: {1}", app.ID, e.Message),
                        appId: app.ID, exception: e));
                    continue;
                }
                try
                {
                    CreateLauncher(man.Config, app);
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Creating launcher for {0} failed: {1}", app.ID, e.Message),
                        appId: app.ID, exception: e));
                    continue;
                }
                var envScript = app.GetCustomScript("env");
                if (envScript != null)
                {
                    notify(new TaskProgress(
                       string.Format("Running custom environment script for {0}.", app.ID), progress, 
                       appId: app.ID));
                    string scriptOutput = null;
                    try
                    {
                        scriptOutput = RunCustomScript(man.Config, man.ProcessExecutionHost, app.ID, envScript).Trim();
                    }
                    catch (ProcessExecutionFailedException e)
                    {
                        notify(new TaskError(
                            string.Format("Running custom environment script for {0} failed: {1}", app.ID, e.Message),
                            appId: app.ID, exception: e));
                        continue;
                    }
                    if (!string.IsNullOrEmpty(scriptOutput))
                    {
                        notify(new TaskInfo(
                            string.Format("Running custom environment script for {0} finished.", app.ID),
                            appId: app.ID, consoleOutput: scriptOutput));
                    }
                }
                notify(new TaskProgress(
                    string.Format("Set up environment for {0}.", app.ID),
                    progress, app.ID));
            }

            var globalEnvScript = GetGlobalCustomScriptFile(man.Config, "env");
            if (globalEnvScript != null)
            {
                notify(new TaskProgress("Running global environment script.", 0.9f));
                string scriptOutput = null;
                try
                {
                    scriptOutput = RunGlobalCustomScript(man.Config, man.ProcessExecutionHost, globalEnvScript).Trim();
                }
                catch (ProcessExecutionFailedException e)
                {
                    notify(new TaskError("Running global environment script failed.",
                        consoleOutput: e.ProcessOutput, exception: e));
                }
                if (!string.IsNullOrEmpty(scriptOutput))
                {
                    notify(new TaskInfo(
                        "Running global environment script finished.",
                        consoleOutput: scriptOutput));
                }
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished updating environment.", 1f));
            }
        }

        #endregion

        #region Bench Update

        private static void DownloadBenchUpdate(IBenchManager man,
            ICollection<AppFacade> _,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            notify(new TaskInfo("Retrieving latest Bench version number..."));

            var version = GetLatestVersion(man.Config);
            if (version == null)
            {
                notify(new TaskError("Retrieving the latest Bench version number failed."));
                return;
            }
            notify(new TaskInfo("Found Bench v" + version));

            var taskCount = 2;
            var finished = 0;
            var errorCnt = 0;
            var endEvent = new ManualResetEvent(false);

            EventHandler<DownloadEventArgs> downloadStartedHandler = (o, e) =>
            {
                notify(new TaskProgress(
                    string.Format("Started download for {0} ...", e.Task.Id),
                    (float)finished / taskCount, e.Task.Id, e.Task.Url.ToString()));
            };

            EventHandler<DownloadEventArgs> downloadEndedHandler = (o, e) =>
            {
                finished++;
                if (!e.Task.Success)
                {
                    errorCnt++;
                    notify(new TaskError(e.Task.ErrorMessage, e.Task.Id));
                }
                else
                {
                    notify(new TaskProgress(
                        string.Format("Finished download for {0}.", e.Task.Id),
                        (float)finished / taskCount, e.Task.Id));
                }
            };

            EventHandler workFinishedHandler = null;
            workFinishedHandler = (EventHandler)((o, e) =>
            {
                man.Downloader.DownloadEnded -= downloadEndedHandler;
                man.Downloader.WorkFinished -= workFinishedHandler;
                endEvent.Set();
            });
            man.Downloader.DownloadStarted += downloadStartedHandler;
            man.Downloader.DownloadEnded += downloadEndedHandler;
            man.Downloader.WorkFinished += workFinishedHandler;

            cancelation.Canceled += (s, e) =>
            {
                man.Downloader.CancelAll();
            };

            notify(new TaskProgress("Downloading Bench update...", 0f));

            var binaryUrl = man.Config.GetStringValue(PropertyKeys.UpdateUrlTemplate, string.Empty)
                .Replace("#VERSION#", version);
            var binaryFile = Path.Combine(man.Config.BenchRootDir, "Bench.zip");
            var bootstrapUrl = man.Config.GetStringValue(PropertyKeys.BootstrapUrlTemplate, string.Empty)
                .Replace("#VERSION#", version);
            var bootstrapFile = Path.Combine(man.Config.BenchRootDir, "bench-install.bat");

            man.Downloader.Enqueue(new DownloadTask("Bench Binary", new Uri(binaryUrl), binaryFile));
            man.Downloader.Enqueue(new DownloadTask("Bootstrap File", new Uri(bootstrapUrl), bootstrapFile));
            endEvent.WaitOne();

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished downloading the Bench update.", 1f));
            }
        }

        /// <summary>
        /// Starts the boostrap script for (re)installing the Bench system.
        /// This requires the <c>bench-install.bat</c> and the <c>bench.zip</c>
        /// to be stored in the Bench root directory.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        public static void InitiateInstallationBootstrap(BenchConfiguration config)
        {
            var rootPath = config.BenchRootDir;
            var si = new ProcessStartInfo("cmd",
                "/D \"@ECHO.Starting Bench Installation... && @ECHO. && @ECHO.Make sure, all programs in the Bench environment are closed. && @PAUSE && CALL ^\""
                    + Path.Combine(rootPath, "bench-install.bat") + "^\"\"");
            si.UseShellExecute = true;
            si.WorkingDirectory = rootPath;
            Process.Start(si);
        }

        #endregion

        #region Install Apps

        private static void CopyAppResourceFile(BenchConfiguration config, AppFacade app)
        {
            var resourceFile = Path.Combine(config.GetStringValue(PropertyKeys.DownloadDir), app.ResourceFileName);
            if (!File.Exists(resourceFile))
            {
                throw new FileNotFoundException("Application resource not found.", resourceFile);
            }
            var targetDir = Path.Combine(config.GetStringValue(PropertyKeys.LibDir), app.Dir);

            FileSystem.AsureDir(targetDir);
            File.Copy(resourceFile, Path.Combine(targetDir, app.ResourceFileName), true);
        }

        private static void ExtractAppArchive(BenchConfiguration config, IProcessExecutionHost execHost, AppFacade app)
        {
            var tmpDir = Path.Combine(config.GetStringValue(PropertyKeys.TempDir), app.ID + "_extract");
            var archiveFile = Path.Combine(config.GetStringValue(PropertyKeys.DownloadDir), app.ResourceArchiveName);
            if (!File.Exists(archiveFile))
            {
                throw new FileNotFoundException("Application resource not found.", app.ResourceArchiveName);
            }
            var targetDir = Path.Combine(config.GetStringValue(PropertyKeys.LibDir), app.Dir);
            var extractDir = app.ResourceArchivePath != null ? tmpDir : targetDir;
            FileSystem.AsureDir(extractDir);
            var customExtractScript = app.GetCustomScript("extract");
            switch (app.ResourceArchiveTyp)
            {
                case AppArchiveTyps.Auto:
                    if (customExtractScript != null)
                    {
                        RunCustomScript(config, execHost, app.ID, customExtractScript, archiveFile, extractDir);
                    }
                    else if (archiveFile.EndsWith(".msi", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ExtractMsiPackage(config, execHost, app.ID, archiveFile, extractDir);
                    }
                    else if (archiveFile.EndsWith(".0"))
                    {
                        ExtractInnoSetup(config, execHost, app.ID, archiveFile, extractDir);
                    }
                    else
                    {
                        ExtractArchiveGeneric(config, execHost, app.ID, archiveFile, extractDir);
                    }
                    break;
                case AppArchiveTyps.Generic:
                    ExtractArchiveGeneric(config, execHost, app.ID, archiveFile, extractDir);
                    break;
                case AppArchiveTyps.Msi:
                    ExtractMsiPackage(config, execHost, app.ID, archiveFile, extractDir);
                    break;
                case AppArchiveTyps.InnoSetup:
                    ExtractInnoSetup(config, execHost, app.ID, archiveFile, extractDir);
                    break;
                case AppArchiveTyps.Custom:
                    if (customExtractScript != null)
                    {
                        RunCustomScript(config, execHost, app.ID, customExtractScript, archiveFile, extractDir);
                    }
                    break;
            }

            if (app.ResourceArchivePath != null)
            {
                FileSystem.MoveContent(Path.Combine(extractDir, app.ResourceArchivePath), targetDir);
                FileSystem.PurgeDir(extractDir);
            }
        }

        private static void ExtractArchiveGeneric(BenchConfiguration config, IProcessExecutionHost execHost, string id,
            string archiveFile, string targetDir)
        {
            var sevenZipExe = config.Apps[AppKeys.SevenZip].Exe;
            if (sevenZipExe != null && File.Exists(sevenZipExe))
            {
                ExtractArchive7z(config, execHost, id, archiveFile, targetDir);
            }
            else
            {
                ExtractArchiveClr(archiveFile, targetDir);
            }
        }

        private static void ExtractArchiveClr(string archiveFile, string targetDir)
        {
            using (var zip = new ZipFile(archiveFile))
            {
                zip.ExtractAll(targetDir, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        private static void ExtractArchive7z(BenchConfiguration config, IProcessExecutionHost execHost, string id,
            string archiveFile, string targetDir)
        {
            if (Regex.IsMatch(Path.GetFileName(archiveFile), @"\.tar\.\w+$",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                var tmpDir = Path.Combine(config.GetStringValue(PropertyKeys.TempDir), id + "_tar");
                FileSystem.EmptyDir(tmpDir);
                try
                {
                    Run7zExtract(config, execHost, archiveFile, tmpDir);
                }
                catch (ProcessExecutionFailedException)
                {
                    // ignore warnings during tar extraction
                }
                try
                {
                    var tarFile = Path.Combine(tmpDir, Path.GetFileNameWithoutExtension(archiveFile));
                    Run7zExtract(config, execHost, tarFile, targetDir);
                }
                finally
                {
                    // extracting the compressed tar file failed
                    FileSystem.PurgeDir(tmpDir);
                }
            }
            else
            {
                Run7zExtract(config, execHost, archiveFile, targetDir);
            }
        }

        private static void Run7zExtract(BenchConfiguration config, IProcessExecutionHost execHost,
            string archiveFile, string targetDir)
        {
            var svnZipExe = config.Apps[AppKeys.SevenZip].Exe;
            if (svnZipExe == null || !File.Exists(svnZipExe))
            {
                throw new FileNotFoundException("Could not find the executable of 7z.");
            }
            var env = new BenchEnvironment(config);
            var args = CommandLine.FormatArgumentList("x", "-y", "-bd", "-o" + targetDir, archiveFile);
            var result = execHost.RunProcess(env, targetDir, svnZipExe, args,
                    ProcessMonitoring.ExitCodeAndOutput);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException("Running 7z failed.",
                    svnZipExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void ExtractMsiPackage(BenchConfiguration config, IProcessExecutionHost execHost,
            string id, string archiveFile, string targetDir)
        {
            var lessMsiExe = config.Apps[AppKeys.LessMSI].Exe;
            if (lessMsiExe == null || !File.Exists(lessMsiExe))
            {
                throw new FileNotFoundException("Could not find the executable of LessMSI.");
            }
            var env = new BenchEnvironment(config);
            var args = CommandLine.FormatArgumentList("x", archiveFile, @".\");
            var result = execHost.RunProcess(env, targetDir, lessMsiExe, args,
                ProcessMonitoring.ExitCodeAndOutput);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException("Running LessMSI failed.",
                    lessMsiExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void ExtractInnoSetup(BenchConfiguration config, IProcessExecutionHost execHost,
            string id, string archiveFile, string targetDir)
        {
            var innoUnpExe = config.Apps[AppKeys.InnoSetupUnpacker].Exe;
            if (innoUnpExe == null || !File.Exists(innoUnpExe))
            {
                throw new FileNotFoundException("Could not find the executable of InnoUnp.");
            }
            var env = new BenchEnvironment(config);
            var args = CommandLine.FormatArgumentList("-q", "-x", archiveFile);
            var result = execHost.RunProcess(env, targetDir, innoUnpExe, args,
                ProcessMonitoring.ExitCodeAndOutput);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException("Running InnoUnp failed.",
                    innoUnpExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void InstallNodePackage(BenchConfiguration config, IProcessExecutionHost execHost, AppFacade app)
        {
            var npmExe = config.Apps[AppKeys.Npm].Exe;
            if (npmExe == null || !File.Exists(npmExe))
            {
                throw new FileNotFoundException("The NodeJS package manager was not found.");
            }
            var packageName = app.Version != null
                ? string.Format("{0}@{1}", app.PackageName, app.Version)
                : app.PackageName;
            var args = CommandLine.FormatArgumentList("install", "--global", packageName);
            var result = execHost.RunProcess(new BenchEnvironment(config), config.BenchRootDir, npmExe, args,
                ProcessMonitoring.ExitCodeAndOutput);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException(
                    string.Format("Installing the NodeJS package {0} failed.", app.PackageName),
                    npmExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void InstallRubyPackage(BenchConfiguration config, IProcessExecutionHost execHost, AppFacade app)
        {
            var gemExe = GemExe(config);
            if (gemExe == null || !File.Exists(gemExe))
            {
                throw new FileNotFoundException("The Ruby package manager was not found.");
            }
            var argList = new List<string>();
            argList.Add("install");
            argList.Add(app.PackageName);
            if (app.Version != null)
            {
                argList.Add("--version");
                argList.Add(app.Version);
            }
            var args = CommandLine.FormatArgumentList(argList.ToArray());
            var result = execHost.RunProcess(new BenchEnvironment(config), config.BenchRootDir, gemExe, args,
                ProcessMonitoring.ExitCodeAndOutput);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException(
                    string.Format("Installing the Ruby package {0} failed.", app.PackageName),
                    gemExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void InstallPythonPackage(BenchConfiguration config, IProcessExecutionHost execHost, PythonVersion pyVer, AppFacade app)
        {
            var pipExe = PipExe(config, pyVer);
            if (pipExe == null)
            {
                throw new FileNotFoundException("The " + pyVer + " package manager PIP was not found.");
            }
            var argList = new List<string>();
            argList.Add("install");
            argList.Add(app.PackageName);
            if (app.IsVersioned) argList.Add(app.Version);
            if (app.IsInstalled) argList.Add("--upgrade");
            argList.Add("--quiet");
            var args = CommandLine.FormatArgumentList(argList.ToArray());
            var result = execHost.RunProcess(new BenchEnvironment(config), config.BenchRootDir, pipExe, args,
                    ProcessMonitoring.ExitCodeAndOutput);

            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException(
                    string.Format("Installing the {0} package {1} failed.", pyVer, app.PackageName),
                    pipExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void InstallNuGetPackage(BenchConfiguration config, IProcessExecutionHost execHost, AppFacade app)
        {
            var nugetExe = config.Apps[AppKeys.NuGet].Exe;
            if (nugetExe == null || !File.Exists(nugetExe))
            {
                throw new FileNotFoundException("The NuGet executable was not found.");
            }
            var argList = new List<string>();
            argList.Add("install");
            argList.Add(app.PackageName);
            if (app.IsVersioned)
            {
                argList.Add("-Version");
                argList.Add(app.Version);
            }
            argList.Add("-OutputDirectory");
            argList.Add(app.Dir);
            argList.Add("-ExcludeVersion");
            argList.Add("-NoCache");
            argList.Add("-Verbosity");
            argList.Add("normal");
            argList.Add("-NonInteractive");
            var args = CommandLine.FormatArgumentList(argList.ToArray());
            FileSystem.AsureDir(app.Dir);
            var result = execHost.RunProcess(new BenchEnvironment(config), config.BenchRootDir, nugetExe, args,
                ProcessMonitoring.ExitCodeAndOutput);

            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException(
                    string.Format("Installing the NuGet package {1} failed.", app.PackageName),
                    nugetExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void InstallApps(IBenchManager man,
            ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var selectedApps = new List<AppFacade>();
            foreach (var app in apps)
            {
                if (app.CanInstall) selectedApps.Add(app);
            }

            try
            {
                FileSystem.AsureDir(
                    man.Config.GetStringValue(PropertyKeys.LauncherScriptDir));
                FileSystem.AsureDir(
                    man.Config.GetStringValue(PropertyKeys.LauncherDir));
                FileSystem.AsureDir(
                    man.Config.GetStringValue(PropertyKeys.AppAdornmentBaseDir));
                FileSystem.AsureDir(
                    man.Config.GetStringValue(PropertyKeys.AppVersionIndexDir));
            }
            catch (Exception e)
            {
                notify(new TaskError("Preparing directories failed.", 
                    exception: e));
                return;
            }

            var cnt = 0;
            foreach (var app in selectedApps)
            {
                if (cancelation.IsCanceled) break;
                cnt++;
                var progress = 0.9f * cnt / selectedApps.Count;

                // 1. Extraction / Installation
                notify(new TaskProgress(string.Format("Installing app {0}.", app.ID), progress, app.ID));
                try
                {
                    switch (app.Typ)
                    {
                        case AppTyps.Meta:
                            // no resource extraction
                            break;
                        case AppTyps.Default:
                            if (app.ResourceFileName != null)
                            {
                                CopyAppResourceFile(man.Config, app);
                            }
                            else if (app.ResourceArchiveName != null)
                            {
                                ExtractAppArchive(man.Config, man.ProcessExecutionHost, app);
                            }
                            break;
                        case AppTyps.NodePackage:
                            InstallNodePackage(man.Config, man.ProcessExecutionHost, app);
                            break;
                        case AppTyps.RubyPackage:
                            InstallRubyPackage(man.Config, man.ProcessExecutionHost, app);
                            break;
                        case AppTyps.Python2Package:
                            InstallPythonPackage(man.Config, man.ProcessExecutionHost, PythonVersion.Python2, app);
                            break;
                        case AppTyps.Python3Package:
                            InstallPythonPackage(man.Config, man.ProcessExecutionHost, PythonVersion.Python3, app);
                            break;
                        case AppTyps.NuGetPackage:
                            InstallNuGetPackage(man.Config, man.ProcessExecutionHost, app);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Invalid app typ '" + app.Typ + "' for app " + app.ID + ".");
                    }
                }
                catch (ProcessExecutionFailedException e)
                {
                    notify(new TaskError(
                        string.Format("Installing app {0} failed: {1}", app.ID, e.Message),
                        appId: app.ID, consoleOutput: e.ProcessOutput, exception: e));
                    continue;
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Installing app {0} failed: {1}", app.ID, e.Message),
                        appId: app.ID, exception: e));
                    continue;
                }

                // 2. Custom Setup-Script
                var customSetupScript = app.GetCustomScript("setup");
                if (customSetupScript != null)
                {
                    notify(new TaskProgress(
                        string.Format("Executing custom setup script for {0}.", app.ID), progress, 
                        appId: app.ID));
                    string scriptOutput = null;
                    try
                    {
                        scriptOutput = RunCustomScript(man.Config, man.ProcessExecutionHost, app.ID, customSetupScript).Trim();
                    }
                    catch (ProcessExecutionFailedException e)
                    {
                        notify(new TaskError(
                            string.Format("Execution of custom setup script for {0} failed.", app.ID),
                            appId: app.ID, consoleOutput: e.ProcessOutput, exception: e));
                        continue;
                    }
                    if (!string.IsNullOrEmpty(scriptOutput))
                    {
                        notify(new TaskInfo(
                            string.Format("Execution custom setup script for {0} finished.", app.ID),
                            appId: app.ID, consoleOutput: scriptOutput));
                    }
                }

                // 3. Create Execution Proxy
                try
                {
                    CreateExecutionProxies(man.Config, app);
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Creating the execution proxy for {0} failed: {1}", app.ID, e.Message),
                        appId: app.ID, exception: e));
                    continue;
                }

                // 4. Create Launcher
                try
                {
                    CreateLauncher(man.Config, app);
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Creating the launcher for {0} failed: {1}", app.ID, e.Message),
                        appId: app.ID, exception: e));
                    continue;
                }

                // 5. Run Custom Environment Script
                var envScript = app.GetCustomScript("env");
                if (envScript != null)
                {
                    notify(new TaskProgress(
                       string.Format("Running custom environment script for {0}.", app.ID),
                       progress, app.ID));
                    string scriptOutput = null;
                    try
                    {
                        scriptOutput = RunCustomScript(man.Config, man.ProcessExecutionHost, app.ID, envScript).Trim();
                    }
                    catch (ProcessExecutionFailedException e)
                    {
                        notify(new TaskError(
                            string.Format("Running the custom environment script for {0} failed.", app.ID),
                            appId: app.ID, consoleOutput: e.ProcessOutput, exception: e));
                        continue;
                    }
                    if (!string.IsNullOrEmpty(scriptOutput))
                    {
                        notify(new TaskInfo(
                            string.Format("Running custom environment script for {0} finished.", app.ID),
                            appId: app.ID, consoleOutput: scriptOutput));
                    }
                }

                // 6. Store installed version
                app.InstalledVersion = app.Version;

                notify(new TaskProgress(string.Format("Finished installing app {0}.", app.ID), progress, 
                    appId: app.ID));
                app.DiscardCachedValues();
            }

            var globalCustomSetupScript = GetGlobalCustomScriptFile(man.Config, "setup");
            if (globalCustomSetupScript != null)
            {
                notify(new TaskProgress("Executing global custom setup script.", 0.95f));
                string scriptOutput = null;
                try
                {
                    scriptOutput = RunGlobalCustomScript(man.Config, man.ProcessExecutionHost, globalCustomSetupScript).Trim();
                }
                catch (ProcessExecutionFailedException e)
                {
                    notify(new TaskError(
                        "Execution of global custom setup script failed.",
                        consoleOutput: e.ProcessOutput, exception: e));
                }
                if (!string.IsNullOrEmpty(scriptOutput))
                {
                    notify(new TaskInfo("Executing global custom setup script finished.", 
                        consoleOutput: scriptOutput));
                }
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished installing apps.", 1f));
            }
        }

        #endregion

        #region Uninstall App

        private static void UninstallGeneric(BenchConfiguration config, AppFacade app)
        {
            var appDir = app.Dir;
            if (appDir != null)
            {
                FileSystem.PurgeDir(appDir);
            }
        }

        private static void UninstallNodePackage(BenchConfiguration config, IProcessExecutionHost execHost,
            AppFacade app)
        {
            var npmExe = config.Apps[AppKeys.Npm].Exe;
            if (npmExe == null || !File.Exists(npmExe))
            {
                throw new FileNotFoundException("The NodeJS package manager was not found.");
            }
            var args = CommandLine.FormatArgumentList("uninstall", "--global", app.PackageName);
            var result = execHost.RunProcess(new BenchEnvironment(config), config.BenchRootDir, npmExe, args,
                ProcessMonitoring.ExitCode);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException(
                    string.Format("Uninstalling the NodeJS package {0} failed.", app.PackageName),
                    npmExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void UninstallRubyPackage(BenchConfiguration config, IProcessExecutionHost execHost,
            AppFacade app)
        {
            var gemExe = GemExe(config);
            if (gemExe == null || !File.Exists(gemExe))
            {
                throw new FileNotFoundException("The Ruby package manager was not found.");
            }
            var args = CommandLine.FormatArgumentList("uninstall", app.PackageName, "-x", "-a");
            var result = execHost.RunProcess(new BenchEnvironment(config), config.BenchRootDir, gemExe, args,
                ProcessMonitoring.ExitCode);
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException(
                    string.Format("Uninstalling the Ruby package {0} failed.", app.PackageName),
                    gemExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static void UninstallPythonPackage(BenchConfiguration config, IProcessExecutionHost execHost,
            PythonVersion pyVer, AppFacade app)
        {
            var pipExe = PipExe(config, pyVer);
            if (pipExe == null)
            {
                throw new FileNotFoundException("The " + pyVer + " package manager PIP was not found.");
            }
            var args = CommandLine.FormatArgumentList("uninstall", app.PackageName, "--yes", "--quiet");
            var result = execHost.RunProcess(new BenchEnvironment(config), config.BenchRootDir, pipExe, args,
                    ProcessMonitoring.ExitCode);

            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException(
                    string.Format("Uninstalling the {0} package {1} failed.", pyVer, app.PackageName),
                    pipExe + " " + args, result.ExitCode, result.Output);
            }
        }

        private static bool CanOmitUninstall(ICollection<AppFacade> selectedApps, AppFacade app)
        {
            var parentAppId = default(string);
            switch (app.Typ)
            {
                case AppTyps.NodePackage:
                    parentAppId = AppKeys.NodeJS;
                    break;
                case AppTyps.Python2Package:
                    parentAppId = AppKeys.Python2;
                    break;
                case AppTyps.Python3Package:
                    parentAppId = AppKeys.Python3;
                    break;
                case AppTyps.RubyPackage:
                    parentAppId = AppKeys.Ruby;
                    break;
            }
            if (parentAppId != null)
            {
                foreach (var selectedApp in selectedApps)
                {
                    if (selectedApp.ID == parentAppId)
                    {
                        return app.GetCustomScript("remove") == null;
                    }
                }
            }
            return false;
        }

        private static void UninstallApps(IBenchManager man,
            ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var selectedApps = new List<AppFacade>();
            foreach (var app in apps)
            {
                if (app.CanUninstall && !CanOmitUninstall(apps, app))
                {
                    selectedApps.Add(app);
                }
            }
            selectedApps.Reverse();

            var cnt = 0;
            foreach (var app in selectedApps)
            {
                if (cancelation.IsCanceled) break;
                cnt++;
                var progress = (float)cnt / selectedApps.Count;

                if (UninstallBlacklist.Contains(app.ID)) continue;

                notify(new TaskProgress(
                    string.Format("Uninstalling app {0}.", app.ID),
                    progress, app.ID));

                var customScript = app.GetCustomScript("remove");
                try
                {
                    if (customScript != null)
                    {
                        notify(new TaskProgress(
                            string.Format("Running custom uninstall script for {0}.", app.ID), progress, 
                            appId: app.ID));
                        var scriptOutput = RunCustomScript(man.Config, man.ProcessExecutionHost, app.ID, customScript).Trim();
                        if (!string.IsNullOrEmpty(scriptOutput))
                        {
                            notify(new TaskInfo(
                                string.Format("Running of custom uninstall script for {0} finished.", app.ID),
                                appId: app.ID, consoleOutput: scriptOutput));
                        }
                    }
                    else
                    {
                        switch (app.Typ)
                        {
                            case AppTyps.Meta:
                                UninstallGeneric(man.Config, app);
                                break;
                            case AppTyps.Default:
                                UninstallGeneric(man.Config, app);
                                break;
                            case AppTyps.NodePackage:
                                UninstallNodePackage(man.Config, man.ProcessExecutionHost, app);
                                break;
                            case AppTyps.RubyPackage:
                                UninstallRubyPackage(man.Config, man.ProcessExecutionHost, app);
                                break;
                            case AppTyps.Python2Package:
                                UninstallPythonPackage(man.Config, man.ProcessExecutionHost, PythonVersion.Python2, app);
                                break;
                            case AppTyps.Python3Package:
                                UninstallPythonPackage(man.Config, man.ProcessExecutionHost, PythonVersion.Python3, app);
                                break;
                            case AppTyps.NuGetPackage:
                                UninstallGeneric(man.Config, app);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("Invalid app typ '" + app.Typ + "' for app " + app.ID + ".");
                        }
                    }

                    app.InstalledVersion = null;
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Uninstalling the app {0} failed.", app.ID),
                        appId: app.ID, exception: e));
                    continue;
                }

                notify(new TaskProgress(string.Format("Finished uninstalling app {0}.", app.ID), progress, app.ID));
                app.DiscardCachedValues();
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished uninstalling the apps.", 1f));
            }
        }

        private static void UninstallApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            notify(new TaskProgress("Uninstalling alls apps.", 0f));
            var success = false;
            var libDir = man.Config.GetStringValue(PropertyKeys.LibDir);
            if (libDir != null && Directory.Exists(libDir))
            {
                try
                {
                    FileSystem.EmptyDir(libDir);
                    success = true;
                }
                catch (Exception e)
                {
                    notify(new TaskError("Uninstalling apps failed.", exception: e));
                }
                if (success)
                {
                    notify(new TaskProgress("Finished uninstalling alls apps.", 1f));
                }
            }
        }

        #endregion
    }
}
