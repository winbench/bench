using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppUpgradeCommand : BenchCommand
    {
        private const string POSITIONAL_APP_ID = "App ID";

        public override string Name => "upgrade";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command upgrades the specified app to the most current release.")
                .End(BlockType.Paragraph)
                .Paragraph("Updates app resources are downloaded automatically.");

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to upgrade.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                positionalAppId);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            return RunManagerTask(mgr => mgr.UpgradeApp(
                Arguments.GetPositionalValue(POSITIONAL_APP_ID)));
        }
    }
}
