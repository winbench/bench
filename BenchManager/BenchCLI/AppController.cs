using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    class AppController : Controller
    {
        private const string COMMAND_PROPERTY = "property";

        private readonly ArgumentParser parser =
            new ArgumentParser(
                new CommandArgument(COMMAND_PROPERTY, "p",
                    "Reads an app property value.",
                    "prop"));

        private readonly MainController mainController;

        public AppController(MainController mainController, string[] args)
        {
            this.mainController = mainController;
            Verbose = mainController.Verbose;
            Arguments = parser.Parse(args);
        }

        protected override void PrintHelp()
        {
            WriteLine("Bench CLI v" + Program.Version() + " [app]");
            WriteLine("----------------------------------------");
            WriteLine("");
            WriteLine("Usage:");
            WriteLine("");
            WriteLine(HelpFormatter.INDENT + "bench app <command> arg*");
            WriteLine(HelpFormatter.INDENT + "bench app (/? | -? | -h | --help)");
            WriteLine(HelpFormatter.GenerateHelp(parser));
        }

        protected override void PrintHelpHint()
        {
            WriteLine("Use 'bench app -?' to display the help.");
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
