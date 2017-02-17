using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class TransferCloneCommand : TransferCommandBase
    {
        private const string OPTION_INCLUDE = "include";
        private const string POSITIONAL_TARGET_DIR = "target-dir";

        private const string DEF_OPTION_INCLUDE = "Config,AppLibs,Cache";

        public override string Name => "clone";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command creates and initializes a clone of this Bench environment in a different location.")
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

            var positionalTargetDir = new PositionalArgument(POSITIONAL_TARGET_DIR,
                ArgumentValidation.IsValidPath,
                1);
            positionalTargetDir.Description
                .Text("The target directory for the clone.");
            positionalTargetDir.PossibleValueInfo
                .Text("A path to a directory. The directory must not exist yet.");

            parser.RegisterArguments(
                optionSelection,
                positionalTargetDir);
        }

        private TransferPaths SelectedPaths
            => ParseTransferPaths(Arguments.GetOptionValue(OPTION_INCLUDE, DEF_OPTION_INCLUDE));

        private string TargetDir => Arguments.GetPositionalValue(POSITIONAL_TARGET_DIR);

        protected override bool ExecuteCommand(string[] args)
        {
            WriteDetail("Exporting the Bench environment...");
            try
            {
                RunManagerTask(man => BenchTasks.CloneBenchEnvironment(man, TargetDir, SelectedPaths));
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
                WriteDetail(ex.ToString());
                return false;
            }
            WriteDetail("Finished exporting the Bench environmment.");
            return true;
        }
    }
}
