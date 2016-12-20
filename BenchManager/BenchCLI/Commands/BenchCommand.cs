using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

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

        #endregion

        protected static string BenchBinDirPath()
        {
            return Path.GetDirectoryName(Program.CliExecutable());
        }

        protected static string DefaultRootPath()
        {
            var rootPath = Path.GetFullPath(Path.Combine(Path.Combine(BenchBinDirPath(), ".."), ".."));
            return File.Exists(Path.Combine(rootPath, @"res\apps.md")) ? rootPath : null;
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
            return new BenchConfiguration(RootPath, withApps, true, true);
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

        #endregion
    }

    delegate bool ManagerTask(DefaultBenchManager mgr);
}
