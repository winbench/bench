using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli.Commands
{
    class ConfigCommand : BenchCommand
    {
        public const string CMD_NAME = "config";

        private const string COMMAND_GET = "get";

        public override string Name => CMD_NAME;

        protected override ArgumentParser InitializeArgumentParser()
        {
            var commandGet = new CommandArgument(COMMAND_GET, "g");
            commandGet.Description
                .Text("Reads a configuration value.");
            commandGet.SyntaxInfo
                .Variable("property name");

            var parser = new ArgumentParser(Name,
                commandGet);

            parser.Description
                .Paragraph("Read and write the Bench configuration.");

            return parser;
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
