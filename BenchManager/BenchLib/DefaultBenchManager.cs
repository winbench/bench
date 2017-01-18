using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This is the default implementation of a <see cref="IBenchManager"/>.
    /// </summary>
    /// <remarks>
    /// This implementation currently also acts as a facade for <see cref="BenchTasks"/>
    /// to be used by <c>auto\lib\Setup-Bench.ps1</c>, which is certainly an indicator
    /// for a bad design decision, and should be refactored.
    /// </remarks>
    public class DefaultBenchManager : IBenchManager
    {
        /// <summary>
        /// The configuration of the Bench system.
        /// </summary>
        public BenchConfiguration Config { get; private set; }

        /// <summary>
        /// The downloader for downloading app resources.
        /// </summary>
        public Downloader Downloader { get; private set; }

        /// <summary>
        /// The environment variables of the Bench system.
        /// </summary>
        public BenchEnvironment Env { get; private set; }

        /// <summary>
        /// The host for starting and running Windows processes.
        /// </summary>
        public IProcessExecutionHost ProcessExecutionHost { get; private set; }

        /// <summary>
        /// The user interface to communicate with the user.
        /// </summary>
        public IUserInterface UI { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultBenchManager"/>
        /// with a <see cref="DefaultExecutionHost"/> and a <see cref="ConsoleUserInterface"/>.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        public DefaultBenchManager(BenchConfiguration config)
        {
            Config = config;
            Downloader = BenchTasks.InitializeDownloader(config);
            Env = new BenchEnvironment(Config);
            //ProcessExecutionHost = new DefaultExecutionHost();
            var execHost = new PowerShellExecutionHost(config);
            execHost.StartHost();
            ProcessExecutionHost = execHost;
            UI = new ConsoleUserInterface();
        }

        /// <summary>
        /// Returns a value, indicating of this instance was already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes all disposable child objects.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            var d = Downloader;
            if (d != null)
            {
                Downloader = null;
                d.Dispose();
            }
            var peh = ProcessExecutionHost;
            if (peh != null)
            {
                ProcessExecutionHost = null;
                peh.Dispose();
            }
            Config = null;
            Env = null;
            UI = null;
        }

        /// <summary>
        /// A flag, controlling if non error messages should be displayed to the user.
        /// If it is set to <c>true</c>, all messages are displayed; otherwise only
        /// error messages are displayed.
        /// </summary>
        public bool Verbose { get; set; }

        private static readonly object consoleSyncHandle = new object();

        private void NotificationHandler(TaskInfo info)
        {
            var err = info as TaskError;

            if (!Verbose && err == null) return;

            lock (consoleSyncHandle)
            {
                if (err != null) Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(
                    "[{0}] ({1}) {2}",
                    info is TaskError ? "ERROR" : "INFO",
                    info.AppId ?? "global",
                    info.Message);

                if (!string.IsNullOrEmpty(info.DetailedMessage))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(info.DetailedMessage);
                }
                if (err != null && err.Exception != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(err.Exception.ToString());
                }
                Console.ResetColor();
            }
        }

        private bool RunAction(BenchTaskForAll action)
            => action(this, NotificationHandler, new Cancelation()).Success;

        private bool RunAction(BenchTaskForOne action, string appId)
            => action(this, appId, NotificationHandler, new Cancelation()).Success;

        /// <summary>
        /// Loads the app libraries, configured in the configuration.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool LoadAppLibraries() => RunAction(BenchTasks.DoLoadAppLibraries);

        /// <summary>
        /// Sets up only the apps required by Bench.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool SetupRequiredApps() => RunAction(BenchTasks.DoSetupRequiredApps);

        /// <summary>
        /// Runs a full automatic setup, including app resource download, app installation of all active apps
        /// and setup of the environment.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool AutoSetup() => RunAction(BenchTasks.DoAutoSetup);

        /// <summary>
        /// Sets up the environment, including the <c>env.cmd</c>, the launcher scripts and launcher shortcuts.
        /// It also runs all custom environment scripts.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UpdateEnvironment() => RunAction(BenchTasks.DoUpdateEnvironment);

        /// <summary>
        /// Downloads the app resources for all active apps, in case they are not already cached.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool DownloadAppResources() => RunAction(BenchTasks.DoDownloadAppResources);

        /// <summary>
        /// Downloads the resource for the specified app, in case it is not already cached.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool DownloadAppResource(string appId) => RunAction(BenchTasks.DoDownloadAppResources, appId);

        /// <summary>
        /// Deletes all cached app resources.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool DeleteAppResources() => RunAction(BenchTasks.DoDeleteAppResources);

        /// <summary>
        /// Deletes the cached resource of the specified app.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool DeleteAppResource(string appId) => RunAction(BenchTasks.DoDeleteAppResources, appId);

        /// <summary>
        /// Deletes all obsolete app resources from the cache.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool CleanUpAppResources() => RunAction(BenchTasks.DoCleanUpAppResources);

        /// <summary>
        /// Installs all active apps, in case they are not already installed.
        /// This also downloads missing app resources.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool InstallApps() => RunAction(BenchTasks.DoInstallApps);

        /// <summary>
        /// Installs the specified app, in case it is not already installed.
        /// This also downloads missing app resources.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool InstallApp(string appId) => RunAction(BenchTasks.DoInstallApps, appId);

        /// <summary>
        /// Uninstalls all installed apps.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UninstallApps() => RunAction(BenchTasks.DoUninstallApps);

        /// <summary>
        /// Uninstalls the specified app, in case it is installed.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UninstallApp(string appId) => RunAction(BenchTasks.DoUninstallApps, appId);

        /// <summary>
        /// Uninstalls all installed apps, and then installs all active apps again.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool ReinstallApps() => RunAction(BenchTasks.DoReinstallApps);

        /// <summary>
        /// Uninstalls the specified app, and then installs it again.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool ReinstallApp(string appId) => RunAction(BenchTasks.DoReinstallApps, appId);

        /// <summary>
        /// Upgrades all active apps, which can be upgraded.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UpgradeApps() => RunAction(BenchTasks.DoUpgradeApps);

        /// <summary>
        /// Upgrades the specified app, if it can be upgraded.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UpgradeApp(string appId) => RunAction(BenchTasks.DoUpgradeApps, appId);

        /// <summary>
        /// Downloads the latest Bench binary and bootstrap file.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool DownloadBenchUpdate() => RunAction(BenchTasks.DoDownloadBenchUpdate);
    }
}
