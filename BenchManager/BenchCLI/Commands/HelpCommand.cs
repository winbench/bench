using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class HelpCommand : BenchCommand
    {
        private const string FLAG_NO_TITLE = "no-title";
        private const string FLAG_NO_VERSION = "no-version";
        private const string FLAG_NO_INDEX = "no-index";
        private const string FLAG_APPEND = "append";
        private const string OPTION_TARGET_FILE = "target-file";

        public override string Name => "help";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Text("Displays the full help for all commands.");

            var flagNoTitle = new FlagArgument(FLAG_NO_TITLE, 't');
            flagNoTitle.Description
                .Text("Suppress the output of the tool name as the document title.");

            var flagNoVersion = new FlagArgument(FLAG_NO_VERSION, 'v');
            flagNoVersion.Description
                .Text("Suppress the output of the tool version number.");

            var flagNoIndex = new FlagArgument(FLAG_NO_INDEX, 'i');
            flagNoIndex.Description
                .Text("Suppress the index of the commands.");

            var flagAppend = new FlagArgument(FLAG_APPEND, 'a');
            flagAppend.Description
                .Text("Append to an existing file, in case a target file is specified.");

            var optionTargetFile = new OptionArgument(OPTION_TARGET_FILE, 'o',
                ArgumentValidation.IsValidPath,
                "out");
            optionTargetFile.Description
                .Text("Specifies a target file to write the help content to.");
            optionTargetFile.PossibleValueInfo
                .Text("A path to a writable file. The target file will be created or overridden.");
            optionTargetFile.DefaultValueInfo
                .Text("None");

            parser.RegisterArguments(
                flagNoTitle,
                flagNoVersion,
                flagNoIndex,
                flagAppend,
                optionTargetFile);
        }

        private bool NoTitle => Arguments.GetFlag(FLAG_NO_TITLE);
        private bool NoVersion => Arguments.GetFlag(FLAG_NO_VERSION);
        private bool NoIndex => Arguments.GetFlag(FLAG_NO_INDEX);
        private bool Append => Arguments.GetFlag(FLAG_APPEND);
        private string TargetFile => Arguments.GetOptionValue(OPTION_TARGET_FILE);

        protected override bool ExecuteCommand(string[] args)
        {
            var targetFile = TargetFile;
            using (var s = targetFile != null
                ? File.Open(targetFile, Append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None)
                : Console.OpenStandardOutput())
            using (var w = DocumentWriterFactory.Create(HelpFormat, s))
            {
                RootCommand.PrintFullHelp(w, !NoTitle, !NoVersion, !NoIndex);
            }
            return true;
        }
    }
}
