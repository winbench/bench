using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ConfigCommand : BenchCommand
    {
        private const string COMMAND_GET = "get";

        public override string Name => "config";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command gives access to the Bench user configuration.")
                .End(BlockType.Paragraph);

            var commandGet = new CommandArgument(COMMAND_GET, "g");
            commandGet.Description
                .Text("Reads a configuration value.");
            commandGet.SyntaxInfo
                .Variable("property name");

            parser.RegisterArguments(
                commandGet);
        }

        protected override bool ExecuteUnknownSubCommand(string command, string[] args)
        {
            switch (command)
            {
                case COMMAND_GET:
                    return TaskGetConfigValue(args);

                default:
                    WriteError("Unsupported command: " + command + ".");
                    return false;
            }
        }

        private bool TaskGetConfigValue(string[] args)
        {
            if (args.Length != 1)
            {
                WriteError("Invalid arguments after 'get'.");
                WriteError("Expected: bench config get <property name>");
                return false;
            }

            var propertyName = args[0];

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
