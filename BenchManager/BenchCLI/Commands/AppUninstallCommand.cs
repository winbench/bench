using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppUninstallCommand : BenchCommand
    {
        private const string POSITIONAL_APP_ID = "App ID";

        public override string Name => "uninstall";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command uninstalles the specified app, regardless of its activation state.")
                .End(BlockType.Paragraph);

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to install.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                positionalAppId);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            return RunManagerTask(mgr => mgr.UninstallApp(
                Arguments.GetPositionalValue(POSITIONAL_APP_ID)));
        }
    }
}
