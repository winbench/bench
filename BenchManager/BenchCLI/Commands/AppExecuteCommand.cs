using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppExecuteCommand : BenchCommand
    {
        private const string FLAG_DETACHED = "detached";
        private const string POSITIONAL_APP_ID = "App ID";

        public override string Name => "execute";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command starts the main executable of the specified app.")
                .End(BlockType.Paragraph);

            var flagDetached = new FlagArgument(FLAG_DETACHED, 'd', "async");
            flagDetached.Description
                .Text("Do not wait for the end of the process.");

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to execute.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                flagDetached,
                positionalAppId);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration();
            var appId = Arguments.GetPositionalValue(POSITIONAL_APP_ID);
            var detached = Arguments.GetFlag(FLAG_DETACHED);

            return LaunchApp(cfg, detached, appId, args);
        }
    }
}
