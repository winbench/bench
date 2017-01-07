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
            BenchConfiguration cfgWithSite, cfgWithCoreApps, cfgWithCustom;

            // 1. Initialize the site configuration, possibly with HTTP(S) proxy
            cfgWithSite = BenchTasks.InitializeSiteConfiguration(RootPath);
            if (cfgWithSite == null)
            {
                WriteInfo("Initialization canceled.");
                return false;
            }

            // Create a manager object to get a download manager
            using (var mgrWithSite = new DefaultBenchManager(cfgWithSite))
            {
                mgrWithSite.Verbose = Verbose;
                // 2. Download the app libraries, listed in the Bench system and site configuration
                if (!mgrWithSite.LoadAppLibraries())
                {
                    WriteError("Loading the core app libraries failed.");
                    return false;
                }
            } // dispose the manager object

            // Reload the configuration with the core app libraries
            cfgWithCoreApps = new BenchConfiguration(RootPath, true, false, true);
            cfgWithSite.InjectBenchInitializationProperties(cfgWithCoreApps);
            cfgWithSite = null;

            // Create a manager object to get an execution host
            using (var mgrWithCoreApps = new DefaultBenchManager(cfgWithCoreApps))
            {
                cfgWithCoreApps = null;
                mgrWithCoreApps.Verbose = Verbose;
                // 3. Download and install required apps from the core app library
                if (!mgrWithCoreApps.SetupRequiredApps())
                {
                    WriteError("Initial app setup failed.");
                    return false;
                }

                // 4. Initialize the user configuration and reload the Bench configuration
                cfgWithCustom = BenchTasks.InitializeCustomConfiguration(mgrWithCoreApps);
                if (cfgWithCustom == null)
                {
                    WriteInfo("Initialization canceled.");
                    return false;
                }
            } // dispose the manager object

            // Create a manager object to get a download manager
            using (var mgrWithCustom = new DefaultBenchManager(cfgWithCustom))
            {
                mgrWithCustom.Verbose = Verbose;
                // 5. Download the app libraries, listed in the custom configuration
                if (!mgrWithCustom.LoadAppLibraries())
                {
                    WriteError("Loading the app libraries failed.");
                    return false;
                }
            }

            // Check if the auto setup should be started right now
            var autoSetup = cfgWithCustom.GetBooleanValue(PropertyKeys.WizzardStartAutoSetup, true);

            var dashboardPath = DashboardExecutable();
            if (dashboardPath != null)
            {
                // Kick-off the auto setup with the GUI
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
                // Kick-off the auto setup with the CLI
                return RunManagerTask(m => m.AutoSetup());
            }
            else
                return true;
        }
    }
}
