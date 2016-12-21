using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class UpgradeCommand : BenchCommand
    {
        public override string Name => "upgrade";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command checks if a new version of Bench is available, and incase installs it.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Strong("WARNING: This command ist not implemented yet.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration();
            var version = cfg.GetStringValue(PropertyKeys.Version);

            WriteDetail("Retrieving the latest version number...");
            var latestVersion = BenchTasks.GetLatestVersion(cfg);
            if (latestVersion == null)
            {
                WriteError("Contacting the distribution site failed.");
                return false;
            }

            WriteLine("Current version: " + version);
            if (string.Equals(version, latestVersion))
            {
                WriteLine("No update available.");
                if (!AskForAssurance("Do you want to reinstall the Bench system?")) return false;
            }
            else
            {
                WriteLine("Latest version:  " + latestVersion);
                if (!AskForAssurance("Are you sure, you want to upgrade the Bench system?")) return false;
            }

            if (!RunManagerTask(mgr => mgr.DownloadBenchUpdate())) return false;

            BenchTasks.InitiateInstallationBootstrap(cfg);
            return true;
        }
    }
}
