using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class TransferExportCommand : TransferCommandBase
    {
        private const string OPTION_INCLUDE = "include";
        private const string POSITIONAL_TARGET_FILE = "target-file";

        private const string DEF_OPTION_INCLUDE = "Config,AppLibs";

        public override string Name => "export";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command creates a transfer package of this Bench environment.")
                .End(BlockType.Paragraph);

            var optionSelection = new OptionArgument(OPTION_INCLUDE, 'i',
                IsTransferSelection);
            optionSelection.Description
                .Text("Specifies the content included in the export.");
            optionSelection.DefaultValueInfo
                .Text(DEF_OPTION_INCLUDE);
            optionSelection.PossibleValueInfo
                .Text("A comma separated list of the following keywords: ")
                .AppendList(", ", (w, v) => w.Keyword(v), SELECT_OPTIONS);

            var positionalTargetFile = new PositionalArgument(POSITIONAL_TARGET_FILE,
                ArgumentValidation.IsValidPath, 
                1);
            positionalTargetFile.Description
                .Text("A path to the output file.");
            positionalTargetFile.PossibleValueInfo
                .Text("The filename can have one of the following extensions: ")
                .AppendList(", ", (w, e) => w.Keyword(e), ".zip", ".7z", ".exe");

            parser.RegisterArguments(
                optionSelection,
                positionalTargetFile);
        }

        private TransferPaths SelectedPaths
            => ParseTransferPaths(Arguments.GetOptionValue(OPTION_INCLUDE, DEF_OPTION_INCLUDE));

        private string TargetFile
            => Arguments.GetPositionalValue(POSITIONAL_TARGET_FILE);

        protected override bool ExecuteCommand(string[] args)
        {
            WriteDetail("Exporting the Bench environment...");
            return RunManagerTask(mgr => mgr.ExportBenchEnvironment(TargetFile, SelectedPaths));
        }
    }
}
