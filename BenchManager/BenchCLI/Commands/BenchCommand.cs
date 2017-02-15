using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;

namespace Mastersign.Bench.Cli.Commands
{
    abstract class BenchCommand : CommandBase
    {
        #region Bubbeling Properties

        private string rootPath;

        public string RootPath
        {
            get { return (Parent as BenchCommand)?.RootPath ?? rootPath; }
            set { rootPath = value; }
        }

        private string logFile;

        public string LogFile
        {
            get { return (Parent as BenchCommand)?.LogFile ?? logFile; }
            set { logFile = value; }
        }

        #endregion

        protected static string BenchBinDirPath()
        {
            return Path.GetDirectoryName(Program.CliExecutable());
        }

        protected static string DefaultRootPath()
        {
            var rootPath = Path.GetFullPath(Path.Combine(Path.Combine(BenchBinDirPath(), ".."), ".."));
            return BenchConfiguration.IsValidBenchRoot(rootPath) ? rootPath : null;
        }

        protected static string DashboardExecutable(string rootDir = null)
        {
            if (!BenchTasks.IsDashboardSupported) return null;
            var path = rootDir != null
                ? Path.Combine(Path.Combine(Path.Combine(rootDir, "auto"), "bin"), "BenchDashboard.exe")
                : Path.Combine(BenchBinDirPath(), "BenchDashboard.exe");
            return File.Exists(path) ? path : null;
        }

        #region Task Helper

        protected BenchConfiguration LoadConfiguration(bool withApps = true)
        {
            var cfg = new BenchConfiguration(RootPath, withApps, true, true);
            if (LogFile != null) cfg.SetValue(ConfigPropertyKeys.LogFile, LogFile);
            return cfg;
        }

        protected DefaultBenchManager CreateManager()
        {
            return new DefaultBenchManager(LoadConfiguration())
            {
                Verbose = Verbose
            };
        }

        protected bool RunManagerTask(ManagerTask task)
        {
            using (var mgr = CreateManager())
            {
                return task(mgr);
            }
        }

        public bool LaunchApp(BenchConfiguration cfg, bool detached, string appId, params string[] args)
        {
            if (!cfg.Apps.Exists(appId))
            {
                WriteError("The app '{0}' was not found.", appId);
                return false;
            }

            var app = cfg.Apps[appId];
            if (app.Exe == null)
            {
                WriteError("The app '{0}' has no main executable.", app.Label);
                return false;
            }
            WriteDetail("Found apps executable: {0}", app.Exe);

            WriteDetail("Starting app '{0}' {1} ...", app.Label, detached ? "detached" : "synchronously");

            var env = new BenchEnvironment(cfg);
            var p = BenchTasks.LaunchApp(cfg, env, appId, args);
            if (detached)
            {
                return true;
            }
            else
            {
                p.WaitForExit();
                return p.ExitCode == 0;
            }
        }

        #endregion
    }

    delegate bool ManagerTask(DefaultBenchManager mgr);
}
