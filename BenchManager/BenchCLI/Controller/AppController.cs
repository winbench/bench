using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Controller
{
    class AppController : BaseController
    {
        private static ArgumentParser parser;

        public static ArgumentParser Parser
        {
            get
            {
                if (parser == null) { parser = InitializeParser(); }
                return parser;
            }
        }

        private const string COMMAND_PROPERTY = "property";

        private static ArgumentParser InitializeParser()
        {
            var commandProperty = new CommandArgument(COMMAND_PROPERTY, "p", "prop");
            commandProperty.Description
                .Text("Reads an app property value.");
            commandProperty.SyntaxInfo
                .Variable("app ID")
                .Syntactic(" ")
                .Variable("property name");

            return new ArgumentParser(MainController.Parser, MainController.COMMAND_APP,
                commandProperty);
        }

        private readonly MainController mainController;

        public AppController(MainController mainController, string[] args)
        {
            this.mainController = mainController;
            Verbose = mainController.Verbose;
            Arguments = Parser.Parse(args);
        }

        protected override void PrintHelp(DocumentWriter w)
        {
            w.Begin(BlockType.Document);
            w.Title("Bench CLI v{0} - [{1}]", Program.Version(), "app");
            HelpFormatter.WriteHelp(w, Parser);
            w.End(BlockType.Document);
        }

        protected override bool ExecuteCommand(string command, string[] args)
        {
            WriteDetail("App Command: " + command);
            WriteDetail("");
            switch (command)
            {
                case COMMAND_PROPERTY:
                    return TaskReadProperty(args);

                default:
                    WriteError("Unsupported command: " + command + ".");
                    return false;
            }
        }

        private bool TaskReadProperty(string[] args)
        {
            if (args.Length != 2)
            {
                WriteError("Invalid arguments.");
                WriteError("Expected: bench app property <app ID> <property name>");
                return false;
            }
            var appId = args[0];
            var propertyName = args[1];

            var cfg = mainController.LoadConfiguration();
            if (!cfg.Apps.Exists(appId))
            {
                WriteError("Unknown app ID: " + appId);
                return false;
            }
            WriteDetail("App ID: " + appId);
            if (!cfg.ContainsGroupValue(appId, propertyName))
            {
                WriteError("Unknown property: " + propertyName);
            }
            WriteDetail("Property: " + propertyName);
            Console.Write(cfg.GetGroupValue(appId, propertyName));
            return true;
        }
    }
}
