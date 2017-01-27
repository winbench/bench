using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ReinstallCommand : BenchCommand
    {
        public override string Name => "reinstall";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command removes all installed apps, then installs all active apps.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
            => RunManagerTask(mgr => mgr.ReinstallApps());
    }
}
