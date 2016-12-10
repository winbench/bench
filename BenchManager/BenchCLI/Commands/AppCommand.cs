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
        private readonly BenchCommand appDownloadCommand = new AppDownloadCommand();
        private readonly BenchCommand appInstallCommand = new AppInstallCommand();
        private readonly BenchCommand appReinstallCommand = new AppReinstallCommand();
        private readonly BenchCommand appUpgradeCommand = new AppUpgradeCommand();
        private readonly BenchCommand appUninstallCommand = new AppUninstallCommand();
        private readonly BenchCommand appExecuteCommand = new AppExecuteCommand();

        public override string Name => "app";

        public AppCommand()
        {
            RegisterSubCommand(appInfoCommand);
            RegisterSubCommand(appPropertyCommand);
            RegisterSubCommand(appListPropertiesCommand);
            RegisterSubCommand(appDownloadCommand);
            RegisterSubCommand(appInstallCommand);
            RegisterSubCommand(appReinstallCommand);
            RegisterSubCommand(appUpgradeCommand);
            RegisterSubCommand(appUninstallCommand);
            RegisterSubCommand(appExecuteCommand);
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

            var commandListProperties = new CommandArgument(appListPropertiesCommand.Name, "l", "list");
            commandListProperties.Description
                .Text("Lists the properties of an app.");
            commandListProperties.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appListPropertiesCommand);

            var commandDownload = new CommandArgument(appDownloadCommand.Name, "c", "cache");
            commandDownload.Description
                .Text("Downloads an apps resource.");
            commandDownload.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appDownloadCommand);

            var commandInstall = new CommandArgument(appInstallCommand.Name, "s", "setup");
            commandInstall.Description
                .Text("Installs an app, regardless of its activation state.");
            commandInstall.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appInstallCommand);

            var commandReinstall = new CommandArgument(appReinstallCommand.Name, "r");
            commandReinstall.Description
                .Text("Reinstalls an app.");
            commandReinstall.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appReinstallCommand);

            var commandUpgrade = new CommandArgument(appUpgradeCommand.Name, "u");
            commandUpgrade.Description
                .Text("Upgrades an app.");
            commandUpgrade.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appUpgradeCommand);

            var commandUninstall = new CommandArgument(appUninstallCommand.Name, "x", "remove");
            commandUninstall.Description
                .Text("Uninstalls an app, regardless of its activation state.");
            commandUninstall.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appUninstallCommand);

            var commandExecute = new CommandArgument(appExecuteCommand.Name, "e", "exec", "run");
            commandExecute.Description
                .Text("Starts an apps main executable.");
            commandExecute.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appExecuteCommand);

            parser.RegisterArguments(
                commandProperty,
                commandInfo,
                commandListProperties,
                commandDownload,
                commandInstall,
                commandReinstall,
                commandUpgrade,
                commandUninstall,
                commandExecute);
        }
    }
}
