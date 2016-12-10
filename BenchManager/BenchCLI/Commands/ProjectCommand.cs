using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ProjectCommand : BenchCommand
    {
        public override string Name => "project";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword("project").Text(" command allows to you")
                .Text(" to perform certain tasks on projects in the Bench environment.")
                .End(BlockType.Paragraph)
                .Paragraph("WARNING: This command is not implemented yet.");
        }
    }
}
