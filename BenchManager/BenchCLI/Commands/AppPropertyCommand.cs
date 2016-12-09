using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;

namespace Mastersign.Bench.Cli.Commands
{
    class AppPropertyCommand : BenchCommand
    {
        public const string CMD_NAME = "property";

        private const string POSITIONAL_APP_ID = "App ID";
        private const string POSITIONAL_PROPERTY_NAME = "Property Name";

        public override string Name => CMD_NAME;

        protected override ArgumentParser InitializeArgumentParser()
        {
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

            var parser = new ArgumentParser(CMD_NAME,
                positionalAppId,
                positionalPropertyName);

            parser.Description
                .Paragraph("Reads the value of an app property.");

            return parser;
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
            PropertyWriter.WritePropertyValue(cfg.GetGroupValue(appId, propertyName));
            return true;
        }
    }
}
