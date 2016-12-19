using System;
using System.Collections.Generic;
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
            WriteError("Starting the update is not implemented yet.");
            return RunManagerTask(mgr => mgr.DownloadBenchUpdate());
        }
    }
}
