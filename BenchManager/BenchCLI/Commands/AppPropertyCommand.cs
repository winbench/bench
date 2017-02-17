using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppPropertyCommand : BenchCommand
    {
        private const string POSITIONAL_APP_ID = "App ID";
        private const string POSITIONAL_PROPERTY_NAME = "Property Name";

        public override string Name => "property";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command reads the value of an app property.")
                .End(BlockType.Paragraph);

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to get the property from.");
            positionalAppId.PossibleValueInfo
                .Text("The apps ID, an alphanumeric string without whitespace.");

            var positionalPropertyName = new PositionalArgument(POSITIONAL_PROPERTY_NAME,
                ArgumentValidation.IsIdString,
                2);
            positionalPropertyName.Description
                .Text("Specifies the property to read.");
            positionalPropertyName.PossibleValueInfo
                .Text("The property name, an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                positionalAppId,
                positionalPropertyName);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var appId = Arguments.GetPositionalValue(POSITIONAL_APP_ID);
            var propertyName = Arguments.GetPositionalValue(POSITIONAL_PROPERTY_NAME);

            var cfg = LoadConfiguration();
            if (!cfg.Apps.Exists(appId))
            {
                WriteError("Unknown app ID: " + appId);
                return false;
            }
            WriteDetail("App ID: " + appId);
            WriteDetail("Property: " + propertyName);
            PropertyWriter.WritePropertyValue(cfg.AppProperties.GetGroupValue(appId, propertyName));
            return true;
        }
    }
}
