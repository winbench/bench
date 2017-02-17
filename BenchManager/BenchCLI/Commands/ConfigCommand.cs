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
        private readonly BenchCommand setCommand = new ConfigSetCommand();
        private readonly BenchCommand editCommand = new ConfigEditCommand();

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

            var commandSet = new CommandArgument(setCommand.Name, 's', "write");
            commandSet.Description
                .Text("Writes a configuration value.");
            commandSet.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, setCommand);

            var commandEdit = new CommandArgument(editCommand.Name, 'e');
            commandEdit.Description
                .Text("Opens the user configuration in the default Markdown editor.");
            commandEdit.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, editCommand);
            
            parser.RegisterArguments(
                commandGet,
                commandSet,
                commandEdit);
        }

        public ConfigCommand()
        {
            RegisterSubCommand(getCommand);
            RegisterSubCommand(setCommand);
            RegisterSubCommand(editCommand);
        }
    }
}
