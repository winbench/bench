using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class TransferCommand : BenchCommand
    {
        public override string Name => "transfer";

        private readonly BenchCommand exportCommand = new TransferExportCommand();
        private readonly BenchCommand installCommand = new TransferInstallCommand();
        private readonly BenchCommand cloneCommand = new TransferCloneCommand();

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command supports different kinds of a copying the whole Bench environment.")
                .End(BlockType.Paragraph);

            var commandExport = new CommandArgument(exportCommand.Name, 'e');
            commandExport.Description
                .Text("Create a transfer package of this Bench environment");
            commandExport.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, exportCommand);

            var commandInstall = new CommandArgument(installCommand.Name, 'i');
            commandInstall.Description
                .Text("Install a Bench environment from an extracted Bench transfer package");
            commandInstall.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, installCommand);

            var commandClone = new CommandArgument(cloneCommand.Name, 'c');
            commandClone.Description
                .Text("Copy this Bench environment to a different place.");

            parser.RegisterArguments(
                commandExport,
                commandInstall,
                commandClone);
        }

        public TransferCommand()
        {
            RegisterSubCommand(exportCommand);
            RegisterSubCommand(installCommand);
            RegisterSubCommand(cloneCommand);
        }
    }
}
