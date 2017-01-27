using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AutoSetupCommand : BenchCommand
    {
        public override string Name => "setup";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command runs the auto-setup for the active Bench apps.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
            => RunManagerTask(mgr => mgr.AutoSetup());
    }
}
