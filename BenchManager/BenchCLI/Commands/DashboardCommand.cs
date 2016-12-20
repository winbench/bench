using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class DashboardCommand : BenchCommand
    {
        public override string Name => "dashboard";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command starts the graphical user interface ")
                .Emph("Bench Dashboard").Text(".")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var path = DashboardExecutable(RootPath);
            if (path == null || !File.Exists(path))
            {
                WriteError("Could not find the executable of the Bench Dashboard.");
                return false;
            }

            System.Diagnostics.Process.Start(path);
            return true;
        }
    }
}
