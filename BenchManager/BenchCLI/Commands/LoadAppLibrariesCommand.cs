using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class LoadAppLibraries : BenchCommand
    {
        public override string Name => "load-app-libs";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command loads the latest app libraries.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
            => RunManagerTask(mgr => mgr.LoadAppLibraries());
    }
}
