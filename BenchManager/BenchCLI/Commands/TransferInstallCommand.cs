﻿using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class TransferInstallCommand : BenchCommand
    {
        private const string OPTION_TARGET_DIR = "target-dir";
        private const string FLAG_EXTRACT_ONLY = "extract-only";

        public override string Name => "install";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command installs a Bench environment from an extracted Bench transfer package.")
                .End(BlockType.Paragraph);

            var optionTargetDir = new OptionArgument(OPTION_TARGET_DIR, 'd',
                ArgumentValidation.IsValidPath,
                "target", "dir");
            optionTargetDir.Description
                .Text("Specifies the target directory for the installation.")
                .LineBreak()
                .Text("If left empty, a directory browser will be displayed to choose the target directory.");
            optionTargetDir.PossibleValueInfo
                .Text("A path to a directory. The directory must not exist yet.");

            var flagExtractOnly = new FlagArgument(FLAG_EXTRACT_ONLY, 'e',
                "extract", "no-init");
            flagExtractOnly.Description
                .Text("Deactivates automatic initialization and setup after the transfer.");

            parser.RegisterArguments(
                optionTargetDir,
                flagExtractOnly);
        }

        private string TargetDir => Arguments.GetOptionValue(OPTION_TARGET_DIR);

        private bool ExtractOnly => Arguments.GetFlag(FLAG_EXTRACT_ONLY);

        protected override bool ExecuteCommand(string[] args)
        {
            var targetDir = TargetDir;
            if (targetDir == null)
            {
                targetDir = BenchTasks.AskForBenchCloneTargetDirectory();
            }
            if (targetDir == null)
            {
                return false;
            }
            var extractOnly = ExtractOnly;
            WriteDetail("Installing a new Bench environment to: " + targetDir);
            try
            {
                BenchTasks.InstallBenchEnvironment(RootPath, targetDir, startInitialization: !extractOnly);
            }
            catch (Exception e)
            {
                WriteError("Failed to install the new Bench environment: " + e.Message);
                WriteDetail(e.ToString());
                return false;
            }
            WriteDetail("Finished installing a new Bench environment.");
            return true;
        }
    }
}
