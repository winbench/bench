using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ConfigGetCommand : BenchCommand
    {
        private static string POSITIONAL_PROPERTY_NAME = "property-name";

        public override string Name => "get";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command reads a configuration value.")
                .End(BlockType.Paragraph);

            var positionalPropertyName = new PositionalArgument(POSITIONAL_PROPERTY_NAME,
                null, 1);
            positionalPropertyName.Description
                .Text("The name of the configuration property to read.");

            parser.RegisterArguments(
                positionalPropertyName);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var propertyName = Arguments.GetPositionalValue(POSITIONAL_PROPERTY_NAME);

            var cfg = LoadConfiguration(false);
            if (!cfg.ContainsValue(propertyName))
            {
                WriteError("Unknown property name: " + propertyName);
                return false;
            }
            WriteDetail("Property: " + propertyName);
            PropertyWriter.WritePropertyValue(cfg.GetValue(propertyName));
            return true;
        }
    }
}
