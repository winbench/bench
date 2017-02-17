using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Bench.Markdown;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ConfigSetCommand : BenchCommand
    {
        private static string POSITIONAL_PROPERTY_NAME = "Property Name";
        private static string POSITIONAL_PROPERTY_VALUE = "New Value";

        public override string Name => "set";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command writes a configuration value to the user configuration file.")
                .End(BlockType.Paragraph);

            var positionalPropertyName = new PositionalArgument(POSITIONAL_PROPERTY_NAME,
                null, 1);
            positionalPropertyName.Description
                .Text("The name of the configuration property to write.");

            var positionalPropertyValue = new PositionalArgument(POSITIONAL_PROPERTY_VALUE,
                null, 2);
            positionalPropertyValue.Description
                .Text("The new value for the configuration property.");

            parser.RegisterArguments(
                positionalPropertyName,
                positionalPropertyValue);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var propertyName = Arguments.GetPositionalValue(POSITIONAL_PROPERTY_NAME);
            var propertyValue = Arguments.GetPositionalValue(POSITIONAL_PROPERTY_VALUE);

            var cfg = LoadConfiguration(false);

            WriteDetail("Property: " + propertyName);
            WriteDetail("New Value: " + propertyValue);

            var userConfigFile = cfg.GetStringValue(ConfigPropertyKeys.UserConfigFile);
            WriteDetail("Configuration File: " + userConfigFile);

            try
            {
                MarkdownPropertyEditor.UpdateFile(userConfigFile,
                    new Dictionary<string, string> { { propertyName, propertyValue } });
            }
            catch (Exception e)
            {
                WriteError("Writing the new value to the user configuration failed.");
                WriteDetail(e.ToString());
                return false;
            }
            return true;
        }
    }
}
