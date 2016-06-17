using Ionic.Zip;
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
    public static class BenchTasks
    {
        public static bool IsDashboardSupported
        {
            get { return ClrInfo.IsVersionSupported(new Version(4, 5)); }
        }

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

            var resultCfg = new BenchConfiguration(benchRootDir, true, false, true);

            // transfer intermediate results from wizzard to following initialization steps
            foreach (var key in new[]
                {
                    PropertyKeys.CustomConfigRepository,
                    PropertyKeys.WizzardEditCustomConfigBeforeSetup,
                    PropertyKeys.WizzardStartAutoSetup
                })
            {
                resultCfg.SetValue(key, cfg.GetValue(key));
            }

            if (resultCfg.GetValue(PropertyKeys.CustomConfigRepository) != null)
            {
                resultCfg.SetGroupCategory(AppKeys.Git, BenchConfiguration.DefaultAppCategory);
                resultCfg.Apps[AppKeys.Git].ActivateAsRequired();
            }

            return resultCfg;
        }

        /// <remarks>
        /// Precondition: Git must be set up.
        /// </remarks>
        public static BenchConfiguration InitializeCustomConfiguration(IBenchManager man)
        {
            var customConfigDir = man.Config.GetStringValue(PropertyKeys.CustomConfigDir);
            var customConfigFile = man.Config.GetStringValue(PropertyKeys.CustomConfigFile);

            if (!File.Exists(customConfigFile))
            {
                var repo = man.Config.GetStringValue(PropertyKeys.CustomConfigRepository);
                if (repo != null)
                {
                    // assure no config directoryx exist for git clone
                    if (Directory.Exists(customConfigDir))
                    {
                        FileSystem.PurgeDir(customConfigDir);
                    }
                    // assure the parent directory exists
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

                if (man.Config.GetBooleanValue(PropertyKeys.WizzardEditCustomConfigBeforeSetup))
                {
                    Thread.Sleep(500);
                    man.UI.EditTextFile(customConfigDir,
                        "Adapt the custom configuration before starting the final setup.");
                }
            }

            var cfg = new BenchConfiguration(man.Config.BenchRootDir, false, true, true);

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

            var customAppIndexFile = cfg.GetStringValue(PropertyKeys.CustomAppIndexFile);
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

            return new BenchConfiguration(man.Config.BenchRootDir, true, true, true);
        }

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

        public static string RunCustomScript(BenchConfiguration config, IProcessExecutionHost execHost,
            string appId, string path, params string[] args)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Custom script not found.", path);
            }
            var customScriptRunner = Path.Combine(
                config.GetStringValue(PropertyKeys.BenchScripts),
                "Run-CustomScript.ps1");
            var result = PowerShell.RunScript(new BenchEnvironment(config), execHost,
                ProcessMonitoring.ExitCodeAndOutput,
                config.BenchRootDir, customScriptRunner,
                CommandLine.EscapeArgument(path), PowerShell.FormatStringList(args));
            if (result.ExitCode != 0)
            {
                throw new ProcessExecutionFailedException("Executing custom script failed.",
                    path + " " + CommandLine.FormatArgumentList(args),
                    result.ExitCode, result.Output);
            }
            return result.Output;
        }

        public static string RunGlobalCustomScript(BenchConfiguration config, IProcessExecutionHost execHost,
            string path, params string[] args)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Global custom script not found.", path);
            }
            var customScriptRunner = Path.Combine(
                config.GetStringValue(PropertyKeys.BenchScripts),
                "Run-CustomScript.ps1");
            var result = PowerShell.RunScript(new BenchEnvironment(config), execHost,
                ProcessMonitoring.ExitCodeAndOutput,
                config.BenchRootDir, customScriptRunner,
                CommandLine.EscapeArgument(path), PowerShell.FormatStringList(args));
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

        public static Process StartProcess(BenchEnvironment env,
            string cwd, string exe, string arguments)
        {
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("The executable could not be found.", exe);
            }

            var p = new Process();
            var si = new ProcessStartInfo(exe, arguments);
            si.UseShellExecute = false;
            si.WorkingDirectory = cwd;
            env.Load(si.EnvironmentVariables);
            p.StartInfo = si;
            p.Start();
            return p;
        }

        public static Process LaunchApp(BenchConfiguration config, BenchEnvironment env,
            string appId, string[] args)
        {
            var app = config.Apps[appId];
            var exe = app.LauncherExecutable;
            if (app.IsExecutableAdorned(exe)) exe = app.GetLauncherScriptFile();

            if (string.IsNullOrEmpty(exe))
            {
                throw new ArgumentException("The launcher executable is not set.");
            }
            return StartProcess(env, config.GetStringValue(PropertyKeys.HomeDir),
                exe, CommandLine.SubstituteArgumentList(app.LauncherArguments, args));
        }

        private static string GemExe(BenchConfiguration config)
        {
            var rubyExe = config.GetStringGroupValue(AppKeys.Ruby, PropertyKeys.AppExe);
            return rubyExe != null
                ? Path.Combine(Path.GetDirectoryName(rubyExe), "gem.bat")
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

        public static ActionResult DoAutoSetup(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new ICollection<AppFacade>[]
                {
                    man.Config.Apps.InactiveApps,
                    man.Config.Apps.ActiveApps
                },
                notify, cancelation,

                UninstallApps,
                DownloadAppResources,
                InstallApps,
                UpdateEnvironment);
        }

        public static ActionResult DoDownloadAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                man.Config.Apps.ActiveApps,
                notify, cancelation,
                DownloadAppResources);
        }

        public static ActionResult DoDownloadAppResources(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                appId,
                notify, cancelation,
                DownloadAppResources);
        }

        public static ActionResult DoDownloadAllAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                DownloadAppResources);
        }

        public static ActionResult DoDeleteAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                DeleteAppResources);
        }

        public static ActionResult DoDeleteAppResources(IBenchManager man,
            string appId,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                appId,
                notify, cancelation,
                DeleteAppResources);
        }

        public static ActionResult DoCleanUpAppResources(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                CleanUpAppResources);
        }

        public static ActionResult DoInstallApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                man.Config.Apps.ActiveApps,
                notify, cancelation,
                DownloadAppResources,
                InstallApps);
        }

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

        public static ActionResult DoUninstallApps(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new List<AppFacade>(man.Config.Apps),
                notify, cancelation,
                UninstallApps);
        }

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

        public static ActionResult DoUpdateEnvironment(IBenchManager man,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            return RunTasks(man,
                new AppFacade[0],
                notify, cancelation,
                UpdateEnvironment);
        }

        #endregion

        #region Task Composition

        internal static ActionResult RunTasks(IBenchManager man,
            ICollection<AppFacade>[] taskApps,
            Action<TaskInfo> notify, Cancelation cancelation,
            params BenchTask[] tasks)
        {
            if (tasks.Length == 0) return new ActionResult();

            var logLevel = LogLevels.GuessLevel(man.Config.GetStringValue(PropertyKeys.LogLevel));
            TaskInfoLogger logger = null;
            if (logLevel != LogLevels.None)
            {
                logger = new TaskInfoLogger(
                    man.Config.GetStringValue(PropertyKeys.LogDir),
                    logLevel == LogLevels.Error);
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

        internal static ActionResult RunTasks(IBenchManager man,
            ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation,
            params BenchTask[] tasks)
        {
            return RunTasks(man,
                new ICollection<AppFacade>[] { apps },
                notify, cancelation, tasks);
        }

        internal static ActionResult RunTasks(IBenchManager man,
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

        #region Download App Resources

        internal static void DownloadAppResources(IBenchManager man, ICollection<AppFacade> apps,
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

        internal static void DeleteAppResources(IBenchManager man,
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
                    notify(new TaskError(e.Message, app.ID, null, e));
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

        internal static void CleanUpAppResources(IBenchManager man,
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
            foreach(var name in fileNames)
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
                    notify(new TaskError(e.Message, null, null, e));
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
            if (adornedExePaths.Length > 0)
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

        private static void CreateActionLauncher(BenchConfiguration config, string label, string action, string icon,
            string targetDir = null)
        {
            var launcherDir = targetDir ?? config.GetStringValue(PropertyKeys.LauncherDir);
            var actionDir = config.GetStringValue(PropertyKeys.ActionDir);
            var shortcut = Path.Combine(launcherDir, label + ".lnk");
            var target = Path.Combine(actionDir, action + ".cmd");
            FileSystem.CreateShortcut(shortcut, target, null, config.BenchRootDir, icon);
        }

        private static void CreateActionLaunchers(BenchConfiguration config)
        {
            CreateBenchDashboardLauncher(config);

            if (!IsDashboardSupported)
            {
                CreateActionLauncher(config, "Bench Control", "bench-ctl", @"%SystemRoot%\System32\imageres.dll,109");
                CreateActionLauncher(config, "Bench Control", "bench-ctl", @"%SystemRoot%\System32\imageres.dll,109",
                    config.BenchRootDir);
            }
            if (config.GetBooleanValue(PropertyKeys.QuickAccessCmd, true))
            {
                CreateActionLauncher(config, "Command Line", "bench-cmd", @"%SystemRoot%\System32\cmd.exe");
            }
            if (config.GetBooleanValue(PropertyKeys.QuickAccessPowerShell, false))
            {
                CreateActionLauncher(config, "PowerShell", "bench-ps", @"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe");
            }
            if (config.GetBooleanValue(PropertyKeys.QuickAccessBash, false))
            {
                CreateActionLauncher(config, "Bash", "bench-bash", @"%SystemRoot%\System32\imageres.dll,95");
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

            var code = new StringBuilder();
            code.AppendLine("@ECHO OFF");
            code.AppendLine(string.Format("ECHO.Launching {0} in Bench Context ...", label));
            code.AppendLine(string.Format("CALL \"{0}\\env.cmd\"", autoDir));
            if (app.IsExecutableAdorned(executable))
            {
                code.AppendLine(string.Format("\"{0}\\runps.cmd\" Run-Adorned {1} \"{2}\" {3}",
                    autoDir, app.ID, executable, args));
            }
            else
            {
                code.AppendLine(string.Format("START \"{0}\" \"{1}\" {2}", label, executable, args));
            }
            File.WriteAllText(script, code.ToString());

            var shortcut = app.GetLauncherFile();
            FileSystem.CreateShortcut(shortcut, script, null, config.BenchRootDir, app.LauncherIcon,
                FileSystem.ShortcutWindowStyle.Minimized);
        }

        internal static void UpdateEnvironment(IBenchManager man,
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
                    null, null, e));
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
                    null, null, e));
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
                    null, null, e));
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
                    null, null, e));
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
                        app.ID, null, e));
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
                        app.ID, null, e));
                    continue;
                }
                var envScript = app.GetCustomScriptFile("env");
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
                            string.Format("Running custom environment script for {0} failed: {1}", app.ID, e.Message),
                            app.ID, e.ProcessOutput, e));
                        continue;
                    }
                    if (!string.IsNullOrEmpty(scriptOutput))
                    {
                        notify(new TaskInfo(
                            string.Format("Running custom environment script for {0} finished.", app.ID),
                            app.ID, scriptOutput));
                    }
                }
                notify(new TaskProgress(
                    string.Format("Set up environment for {0}.", app.ID),
                    progress, app.ID));
            }

            var globalEnvScript = GetGlobalCustomScriptFile(man.Config, "env");
            if (globalEnvScript != null)
            {
                notify(new TaskProgress("Executing global environment script.", 0.9f));
                string scriptOutput = null;
                try
                {
                    scriptOutput = RunGlobalCustomScript(man.Config, man.ProcessExecutionHost, globalEnvScript).Trim();
                }
                catch (ProcessExecutionFailedException e)
                {
                    notify(new TaskError("Executing global environment script failed.",
                        null, e.ProcessOutput, e));
                }
                if (!string.IsNullOrEmpty(scriptOutput))
                {
                    notify(new TaskInfo("Executing global environment script finished.", null, scriptOutput));
                }
            }

            if (!cancelation.IsCanceled)
            {
                notify(new TaskProgress("Finished updating environment.", 1f));
            }
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
            var customExtractScript = app.GetCustomScriptFile("extract");
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
                FileSystem.PurgeDir(targetDir);
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
            if (app.Version != null) argList.Add(app.Version);
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

        internal static void InstallApps(IBenchManager man,
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
            }
            catch (Exception e)
            {
                notify(new TaskError("Preparing directories failed.", null, null, e));
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
                        default:
                            throw new ArgumentOutOfRangeException("Invalid app typ '" + app.Typ + "' for app " + app.ID + ".");
                    }
                }
                catch (ProcessExecutionFailedException e)
                {
                    notify(new TaskError(
                        string.Format("Installing app {0} failed: {1}", app.ID, e.Message),
                        app.ID, e.ProcessOutput, e));
                    continue;
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Installing app {0} failed: {1}", app.ID, e.Message),
                        app.ID, null, e));
                    continue;
                }

                // 2. Custom Setup-Script
                var customSetupScript = app.GetCustomScriptFile("setup");
                if (customSetupScript != null)
                {
                    notify(new TaskProgress(
                        string.Format("Executing custom setup script for {0}.", app.ID),
                        progress, app.ID));
                    string scriptOutput = null;
                    try
                    {
                        scriptOutput = RunCustomScript(man.Config, man.ProcessExecutionHost, app.ID, customSetupScript).Trim();
                    }
                    catch (ProcessExecutionFailedException e)
                    {
                        notify(new TaskError(
                            string.Format("Execution of custom setup script for {0} failed.", app.ID),
                            app.ID, e.ProcessOutput, e));
                        continue;
                    }
                    if (!string.IsNullOrEmpty(scriptOutput))
                    {
                        notify(new TaskInfo(
                            string.Format("Execution custom setup script for {0} finished.", app.ID),
                            app.ID, scriptOutput));
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
                        app.ID, null, e));
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
                        app.ID, null, e));
                    continue;
                }

                // 5. Run Custom Environment Script
                var envScript = app.GetCustomScriptFile("env");
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
                            app.ID, e.ProcessOutput, e));
                        continue;
                    }
                    if (!string.IsNullOrEmpty(scriptOutput))
                    {
                        notify(new TaskInfo(
                            string.Format("Running custom environment script for {0} finished.", app.ID),
                            app.ID, scriptOutput));
                    }
                }

                notify(new TaskProgress(string.Format("Finished installing app {0}.", app.ID), progress, app.ID));
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
                        null, e.ProcessOutput, e));
                }
                if (!string.IsNullOrEmpty(scriptOutput))
                {
                    notify(new TaskInfo("Executing global custom setup script finished.", null, scriptOutput));
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

        internal static void UninstallApps(IBenchManager man,
            ICollection<AppFacade> apps,
            Action<TaskInfo> notify, Cancelation cancelation)
        {
            var selectedApps = new List<AppFacade>();
            foreach (var app in apps)
            {
                if (app.CanUninstall) selectedApps.Add(app);
            }
            selectedApps.Reverse();

            var cnt = 0;
            foreach (var app in selectedApps)
            {
                if (cancelation.IsCanceled) break;
                cnt++;
                var progress = (float)cnt / selectedApps.Count;

                notify(new TaskProgress(
                    string.Format("Uninstalling app {0}.", app.ID),
                    progress, app.ID));
                var customScript = app.GetCustomScriptFile("remove");
                try
                {
                    if (customScript != null)
                    {
                        notify(new TaskProgress(
                            string.Format("Executing custom uninstall script for {0}.", app.ID),
                            progress, app.ID));
                        var scriptOutput = RunCustomScript(man.Config, man.ProcessExecutionHost, app.ID, customScript).Trim();
                        if (!string.IsNullOrEmpty(scriptOutput))
                        {
                            notify(new TaskInfo(
                                string.Format("Execution of custom uninstall script for {0} finished.", app.ID),
                                app.ID, scriptOutput));
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
                            default:
                                throw new ArgumentOutOfRangeException("Invalid app typ '" + app.Typ + "' for app " + app.ID + ".");
                        }
                    }
                }
                catch (Exception e)
                {
                    notify(new TaskError(
                        string.Format("Uninstalling the app {0} failed.", app.ID),
                        app.ID, null, e));
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

        internal static void UninstallApps(IBenchManager man,
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
                    notify(new TaskError("Uninstalling apps failed.", null, null, e));
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
