using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class UpdateCommand : BenchCommand
    {
        public override string Name => "update";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command updates the app libraries and upgrades all apps.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            bool success;
            var oldCfg = LoadConfiguration(false);

            // Delete app libraries
            BenchTasks.DeleteAppLibraries(oldCfg);

            // Load app libraries
            using (var man = new DefaultBenchManager(oldCfg))
            {
                man.Verbose = Verbose;
                success = man.LoadAppLibraries();
                if (!success)
                {
                    WriteError("Failed to load the latest app libraries.");
                    return false;
                }
            }

            // Upgrade apps
            var newCfg = LoadConfiguration(true);
            using (var man = new DefaultBenchManager(newCfg))
            {
                man.Verbose = Verbose;
                success = man.UpgradeApps();
                if (!success)
                {
                    WriteError("Failed to upgrade the apps.");
                    return false;
                }
            }
            return true;
        }
    }
}
