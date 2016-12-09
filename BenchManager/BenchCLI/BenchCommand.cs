using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;

namespace Mastersign.Bench.Cli
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
