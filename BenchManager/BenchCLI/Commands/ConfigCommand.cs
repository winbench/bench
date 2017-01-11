using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ConfigCommand : BenchCommand
    {
        public override string Name => "config";

        private readonly BenchCommand getCommand = new ConfigGetCommand();

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command gives access to the Bench user configuration.")
                .End(BlockType.Paragraph);

            var commandGet = new CommandArgument(getCommand.Name, 'g', "read");
            commandGet.Description
                .Text("Reads a configuration value.");
            commandGet.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, getCommand);

            parser.RegisterArguments(
                commandGet);
        }

        public ConfigCommand()
        {
            RegisterSubCommand(getCommand);
        }
    }
}
