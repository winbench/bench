using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    class AppController : Controller
    {
        private const string COMMAND_PROPERTY = "property";

        public readonly ArgumentParser parser =
            new ArgumentParser(MainController.Parser, MainController.COMMAND_APP,
                new CommandArgument(COMMAND_PROPERTY, "p",
                    "Reads an app property value.",
                    "<app ID> <property name>",
                    "prop"));

        private readonly MainController mainController;

        public AppController(MainController mainController, string[] args)
        {
            this.mainController = mainController;
            Verbose = mainController.Verbose;
            Arguments = parser.Parse(args);
        }

        protected override void PrintHelp(IDocumentWriter w)
        {
            w.StartDocument();
            w.Title("Bench CLI v{0} - [{1}]", Program.Version(), "app");
            HelpFormatter.WriteHelp(w, parser);
            w.EndDocument();
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
