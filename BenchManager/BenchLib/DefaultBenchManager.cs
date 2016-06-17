using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class DefaultBenchManager : IBenchManager
    {
        public BenchConfiguration Config { get; private set; }

        public Downloader Downloader { get; private set; }

        public BenchEnvironment Env { get; private set; }

        public IProcessExecutionHost ProcessExecutionHost { get; private set; }

        public IUserInterface UI { get; private set; }

        public bool Verbose { get; set; }

        private static readonly object consoleSyncHandle = new object();

        public DefaultBenchManager(BenchConfiguration config)
        {
            Config = config;
            Downloader = BenchTasks.InitializeDownloader(config);
            Env = new BenchEnvironment(Config);
            ProcessExecutionHost = new DefaultExecutionHost();
            UI = new ConsoleUserInterface();
        }

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

        public bool SetupRequiredApps() { return RunAction(BenchTasks.DoSetupRequiredApps); }

        public bool AutoSetup() { return RunAction(BenchTasks.DoAutoSetup); }

        public bool UpdateEnvironment() { return RunAction(BenchTasks.DoUpdateEnvironment); }

        public bool DownloadAppResources() { return RunAction(BenchTasks.DoDownloadAppResources); }

        public bool DeleteAppResources() { return RunAction(BenchTasks.DoDeleteAppResources); }

        public bool InstallApps() { return RunAction(BenchTasks.DoInstallApps); }

        public bool UninstallApps() { return RunAction(BenchTasks.DoUninstallApps); }

        public bool ReinstallApps() { return RunAction(BenchTasks.DoReinstallApps); }

        public bool UpgradeApps() { return RunAction(BenchTasks.DoUpgradeApps); }
    }
}
