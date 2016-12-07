using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli.Controller
{
    class AppCommand : BenchCommand
    {
        public const string CMD_NAME = "app";

        private const string COMMAND_PROPERTY = "property";
        private const string COMMAND_INFO = "info";

        public override string Name => CMD_NAME;

        protected override ArgumentParser InitializeArgumentParser(ArgumentParser parent)
        {
            var commandProperty = new CommandArgument(COMMAND_PROPERTY, "p", "prop");
            commandProperty.Description
                .Text("Reads an app property value.");
            commandProperty.SyntaxInfo
                .Variable("app ID")
                .Syntactic(" ")
                .Variable("property name");

            var commandInfo = new CommandArgument(COMMAND_INFO, "n");
            commandInfo.Description
                .Text("Shows a detailed info of an app.");
            commandInfo.SyntaxInfo
                .Variable("app ID");

            return new ArgumentParser(parent, Name,
                commandProperty,
                commandInfo);
        }

        protected override bool ExecuteUnknownSubCommand(string command, string[] args)
        {
            switch (command)
            {
                case COMMAND_PROPERTY:
                    return TaskReadProperty(args);
                case COMMAND_INFO:
                    return TaskInfo(args);

                default:
                    WriteError("Unsupported command: " + command + ".");
                    return false;
            }
        }

        private bool TaskReadProperty(string[] args)
        {
            if (args.Length != 2)
            {
                WriteError("Invalid arguments after 'property'.");
                WriteLine("Expected: bench app property <app ID> <property name>");
                return false;
            }
            var appId = args[0];
            var propertyName = args[1];

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

        private bool TaskInfo(string[] args)
        {
            if (args.Length != 1)
            {
                WriteError("Invalid arguments after 'info'.");
                WriteLine("Expected: bench app info <app ID>");
                return false;
            }

            var appId = args[0];

            var cfg = LoadConfiguration();
            if (!cfg.Apps.Exists(appId))
            {
                WriteError("Unknown app ID: " + appId);
                return false;
            }

            var properties = new List<string>(cfg.PropertyNames(appId));
            properties.Sort();
            WriteLine("[" + appId + "]");
            foreach (var p in properties)
            {
                WriteLine(string.Format("{0} = {1}", p, cfg.GetGroupValue(appId, p)));
            }
            return true;
        }
    }
}
