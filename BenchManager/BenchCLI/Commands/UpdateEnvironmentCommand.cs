using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class UpdateEnvironmentCommand : BenchCommand
    {
        public override string Name => "update-env";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command updates the paths in the Bench environment.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
            => RunManagerTask(mgr => mgr.UpdateEnvironment());
    }
}
