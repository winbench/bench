using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;

namespace Mastersign.Bench.Cli.Commands
{
    class AppCommand : BenchCommand
    {
        private readonly BenchCommand appInfoCommand = new AppInfoCommand();
        private readonly BenchCommand appPropertyCommand = new AppPropertyCommand();
        private readonly BenchCommand appListPropertiesCommand = new AppListPropertiesCommand();

        public override string Name => "app";

        public AppCommand()
        {
            RegisterSubCommand(appInfoCommand);
            RegisterSubCommand(appPropertyCommand);
            RegisterSubCommand(appListPropertiesCommand);
        }

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(Docs.BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command allows interacting with Bench apps.")
                .End(Docs.BlockType.Paragraph)
                .Paragraph("Use the sub-commands to select the kind of interaction.");

            var commandProperty = new CommandArgument(appPropertyCommand.Name, "p", "prop");
            commandProperty.Description
                .Text("Reads an app property value.");
            commandProperty.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appPropertyCommand);

            var commandInfo = new CommandArgument(appInfoCommand.Name, "i");
            commandInfo.Description
                .Text("Shows a detailed, human readable info of an app.");
            commandInfo.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appInfoCommand);

            var commandListProperties = new CommandArgument(appListPropertiesCommand.Name, "lp", "lst-p");
            commandListProperties.Description
                .Text("Lists the properties of an app.");
            commandListProperties.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appListPropertiesCommand);

            parser.RegisterArguments(
                commandProperty,
                commandInfo,
                commandListProperties);
        }
    }
}
