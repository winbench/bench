using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppReinstallCommand : BenchCommand
    {
        private const string POSITIONAL_APP_ID = "App ID";

        public override string Name => "reinstall";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command reinstalles the specified app.")
                .End(BlockType.Paragraph);

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to reinstall.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                positionalAppId);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            return RunManagerTask(mgr => mgr.ReinstallApp(
                Arguments.GetPositionalValue(POSITIONAL_APP_ID)));
        }
    }
}
