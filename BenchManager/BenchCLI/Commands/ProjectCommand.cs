using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ProjectCommand : BenchCommand
    {
        public const string CMD_NAME = "project";

        public override string Name => CMD_NAME;

        protected override ArgumentParser InitializeArgumentParser()
        {
            var parser = new ArgumentParser(Name);

            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword("project").Text(" command allows to you")
                .Text(" to perform certain tasks on projects in the Bench environment.")
                .End(BlockType.Paragraph)
                .Paragraph("WARNING: This command is not implemented yet.");

            return parser;
        }
    }
}
