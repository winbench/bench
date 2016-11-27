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
        private const string COMMAND_INFO = "info";

        private static ArgumentParser InitializeParser()
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

            return new ArgumentParser(MainController.Parser, MainController.COMMAND_APP,
                commandProperty,
                commandInfo);
        }

        private readonly MainController mainController;

        public AppController(MainController mainController, string[] args)
        {
            this.mainController = mainController;
            Verbose = mainController.Verbose;
            NoAssurance = mainController.NoAssurance;
            Arguments = Parser.Parse(args);
        }

        protected override void PrintHelp(DocumentWriter w)
        {
            w.Begin(BlockType.Document);
            w.Title("Bench CLI v{0} - [{1}]", Program.Version(), MainController.COMMAND_APP);
            HelpFormatter.WriteHelp(w, Parser);
            w.End(BlockType.Document);
        }

        protected override bool ExecuteCommand(string command, string[] args)
        {
            WriteDetail("App Command: " + command);
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
                return false;
            }
            WriteDetail("Property: " + propertyName);
            Console.Write(cfg.GetGroupValue(appId, propertyName));
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

            var cfg = mainController.LoadConfiguration();
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
