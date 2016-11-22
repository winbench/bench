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
            ProcessExecutionHost = new DefaultExecutionHost();
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

                if (info.DetailedMessage != null)
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
        {
            return action(this, NotificationHandler, new Cancelation()).Success;
        }
        
        /// <summary>
        /// Sets up only the apps required by Bench.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool SetupRequiredApps() { return RunAction(BenchTasks.DoSetupRequiredApps); }

        /// <summary>
        /// Runs a full automatic setup, including app resource download, app installation of all active apps
        /// and setup of the environment.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool AutoSetup() { return RunAction(BenchTasks.DoAutoSetup); }

        /// <summary>
        /// Sets up the environment, including the <c>env.cmd</c>, the launcher scripts and launcher shortcuts.
        /// It also runs all custom environment scripts.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UpdateEnvironment() { return RunAction(BenchTasks.DoUpdateEnvironment); }

        /// <summary>
        /// Downloads the app resources for all active apps, in case they are not already cached.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool DownloadAppResources() { return RunAction(BenchTasks.DoDownloadAppResources); }

        /// <summary>
        /// Deletes all cached app resources.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool DeleteAppResources() { return RunAction(BenchTasks.DoDeleteAppResources); }

        /// <summary>
        /// Installs all active apps, in case they are not already installed.
        /// This also downloads missing app resources.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool InstallApps() { return RunAction(BenchTasks.DoInstallApps); }

        /// <summary>
        /// Uninstalls all installed apps.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UninstallApps() { return RunAction(BenchTasks.DoUninstallApps); }

        /// <summary>
        /// Uninstalls all installed apps, and then installs all active apps again.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool ReinstallApps() { return RunAction(BenchTasks.DoReinstallApps); }

        /// <summary>
        /// Upgrades all active apps, which can be upgraded.
        /// </summary>
        /// <returns><c>true</c> if the execution of the task was successful; otherwise <c>false</c>.</returns>
        public bool UpgradeApps() { return RunAction(BenchTasks.DoUpgradeApps); }
    }
}
