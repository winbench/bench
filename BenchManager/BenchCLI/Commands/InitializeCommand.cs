using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class InitializeCommand : BenchCommand
    {
        public override string Name => "initialize";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command initializes the Bench configuration and starts the setup process.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = BenchTasks.InitializeSiteConfiguration(RootPath);
            if (cfg == null)
            {
                WriteInfo("Initialization canceled.");
                return false;
            }

            var autoSetup = cfg.GetBooleanValue(PropertyKeys.WizzardStartAutoSetup, true);
            var mgr = new DefaultBenchManager(cfg);
            mgr.Verbose = Verbose;
            cfg = null;

            var success = mgr.SetupRequiredApps();
            if (!success)
            {
                WriteError("Initial app setup failed.");
                return false;
            }

            cfg = BenchTasks.InitializeCustomConfiguration(mgr);
            if (cfg == null)
            {
                WriteInfo("Initialization canceled.");
                return false;
            }
            mgr.Dispose();

            var dashboardPath = DashboardExecutable();
            if (dashboardPath != null)
            {
                var arguments = string.Format("-root \"{0}\"", RootPath);
                if (autoSetup)
                {
                    arguments += " -setup";
                }
                var pi = new ProcessStartInfo()
                {
                    FileName = dashboardPath,
                    Arguments = arguments,
                    UseShellExecute = false
                };
                System.Diagnostics.Process.Start(pi);
                return true;
            }
            else if (autoSetup)
            {
                return RunManagerTask(m => m.AutoSetup());
            }
            else
                return true;
        }
    }
}
