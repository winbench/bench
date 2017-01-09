using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ManageCommand : BenchCommand
    {
        public override string Name => "manage";

        private readonly BenchCommand configCommand = new ConfigCommand();
        private readonly BenchCommand initializeCommand = new InitializeCommand();
        private readonly BenchCommand loadAppLibsCommand = new LoadAppLibraries();
        private readonly BenchCommand autoSetupCommand = new AutoSetupCommand();
        private readonly BenchCommand updateEnvCommand = new UpdateEnvironmentCommand();
        private readonly BenchCommand downloadsCommand = new DownloadsCommand();
        private readonly BenchCommand reinstallCommand = new ReinstallCommand();
        private readonly BenchCommand renewCommand = new RenewCommand();
        private readonly BenchCommand updateCommand = new UpdateCommand();
        private readonly BenchCommand upgradeCommand = new UpgradeCommand();

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command manages the Bench environment via a number of sub-commands.")
                .End(BlockType.Paragraph);

            var commandConfig = new CommandArgument(configCommand.Name, 'c', "cfg");
            commandConfig.Description
                .Text("Read or write values from the user configuration.");
            commandConfig.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, configCommand);

            var commandInitialize = new CommandArgument(initializeCommand.Name, 'i', "init");
            commandInitialize.Description
                .Text("Initialize the Bench configuration and start the setup process.");

            var commandLoadAppLibs = new CommandArgument(loadAppLibsCommand.Name, 'l');
            commandLoadAppLibs.Description
                .Text("Load the latest app libraries.");

            var commandSetup = new CommandArgument(autoSetupCommand.Name, 's');
            commandSetup.Description
                .Text("Run the auto-setup for the active Bench apps.");

            var commandUpdateEnv = new CommandArgument(updateEnvCommand.Name, 'e');
            commandUpdateEnv.Description
                .Text("Update the paths in the Bench environment.");

            var commandDownloads = new CommandArgument(downloadsCommand.Name, 'd', "cache", "dl");
            commandDownloads.Description
                .Text("Manage the app resource cache.");
            commandDownloads.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, downloadsCommand);

            var commandReinstall = new CommandArgument(reinstallCommand.Name, 'r');
            commandReinstall.Description
                .Text("Remove all installed apps, then install all active apps.");

            var commandRenew = new CommandArgument(renewCommand.Name, 'n');
            commandRenew.Description
                .Text("Redownload all app resources, remove all installed apps, then install all active apps.");

            var commandUpdate = new CommandArgument(updateCommand.Name, 'u');
            commandUpdate.Description
                .Text("Update the app libraries and upgrades all apps.");

            var commandUpgrade = new CommandArgument(upgradeCommand.Name, 'g');
            commandUpgrade.Description
                .Text("Download and extract the latest Bench release, then run the auto-setup.");

            parser.RegisterArguments(
                commandConfig,
                commandInitialize,
                commandSetup,
                commandLoadAppLibs,
                commandUpdateEnv,
                commandReinstall,
                commandRenew,
                commandUpdate,
                commandUpgrade);
        }

        public ManageCommand()
        {
            RegisterSubCommand(configCommand);
            RegisterSubCommand(initializeCommand);
            RegisterSubCommand(loadAppLibsCommand);
            RegisterSubCommand(autoSetupCommand);
            RegisterSubCommand(updateEnvCommand);
            RegisterSubCommand(downloadsCommand);
            RegisterSubCommand(reinstallCommand);
            RegisterSubCommand(renewCommand);
            RegisterSubCommand(updateCommand);
            RegisterSubCommand(upgradeCommand);
        }
    }
}
