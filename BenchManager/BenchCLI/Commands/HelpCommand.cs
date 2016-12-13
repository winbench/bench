using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class HelpCommand : BenchCommand
    {
        public override string Name => "help";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Text("Displays the full help for all commands.");
        }

        protected override bool ExecuteCommand(string[] args)
        {
            using (var w = DocumentWriterFactory.Create(HelpFormat))
            {
                RootCommand.PrintFullHelp(w);
            }
            return true;
        }
    }
}
