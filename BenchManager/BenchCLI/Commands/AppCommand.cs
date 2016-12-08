using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppCommand : BenchCommand
    {
        public const string CMD_NAME = "app";

        private const string COMMAND_PROPERTY = "property";
        private const string COMMAND_INFO = "info";
        private const string COMMAND_LIST_PROPERTIES = "list-properties";

        private readonly BenchCommand appInfoCommand = new AppInfoCommand();
        private readonly BenchCommand appListPropertiesCommand = new AppListPropertiesCommand();

        public override string Name => CMD_NAME;

        public AppCommand()
        {
            RegisterSubCommand(appInfoCommand);
            RegisterSubCommand(appListPropertiesCommand);
        }

        protected override ArgumentParser InitializeArgumentParser()
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
                .Text("Shows a detailed, human readable info of an app.");
            commandInfo.SyntaxInfo
                .Variable("app ID");

            var commandListProperties = new CommandArgument(AppListPropertiesCommand.CMD_NAME, "lp", "lst-p");
            commandListProperties.Description
                .Text("Lists the properties of an app.");
            commandListProperties.SyntaxInfo
                .Append(HelpFormatter.CommandChain, appListPropertiesCommand);

            return new ArgumentParser(Name,
                commandProperty,
                commandInfo,
                commandListProperties);
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

            var app = cfg.Apps[appId];
            var w = new PlainTextDocumentWriter(Console.Out);
            WriteAppInfo(app, w);

            return true;
        }

        private void WriteAppInfo(AppFacade app, DocumentWriter writer)
        {
            writer.Begin(BlockType.Document);
            writer.Title(app.Label);

            writer.Begin(BlockType.List);
            WriteProperty(writer, "ID", app.ID);
            writer.End(BlockType.List);

            writer.End(BlockType.Document);
        }

        private void WriteProperty(DocumentWriter writer, string key, string value)
        {
            writer
                .Begin(BlockType.ListItem)
                .Keyword(key).Text(": ").Syntactic(value)
                .End(BlockType.ListItem);
        }
    }
}
