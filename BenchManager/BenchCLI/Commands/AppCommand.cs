using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;

namespace Mastersign.Bench.Cli.Commands
{
    class AppCommand : BenchCommand
    {
        public const string CMD_NAME = "app";

        private readonly BenchCommand appInfoCommand = new AppInfoCommand();
        private readonly BenchCommand appPropertyCommand = new AppPropertyCommand();
        private readonly BenchCommand appListPropertiesCommand = new AppListPropertiesCommand();

        public override string Name => CMD_NAME;

        public AppCommand()
        {
            RegisterSubCommand(appInfoCommand);
            RegisterSubCommand(appPropertyCommand);
            RegisterSubCommand(appListPropertiesCommand);
        }

        protected override ArgumentParser InitializeArgumentParser()
        {
            var commandProperty = new CommandArgument(AppPropertyCommand.CMD_NAME, "p", "prop");
            commandProperty.Description
                .Text("Reads an app property value.");
            commandProperty.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appPropertyCommand);

            var commandInfo = new CommandArgument(AppInfoCommand.CMD_NAME, "n");
            commandInfo.Description
                .Text("Shows a detailed, human readable info of an app.");
            commandInfo.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appInfoCommand);

            var commandListProperties = new CommandArgument(AppListPropertiesCommand.CMD_NAME, "lp", "lst-p");
            commandListProperties.Description
                .Text("Lists the properties of an app.");
            commandListProperties.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appListPropertiesCommand);

            var parser = new ArgumentParser(Name,
                commandProperty,
                commandInfo,
                commandListProperties);
            parser.Description
                .Paragraph("The app command allows interacting with Bench apps.")
                .Paragraph("Use the sub-commands to select the kind of interaction.");

            return parser;
        }
    }
}
