using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    class AppController : Controller
    {
        private readonly ArgumentParser parser =
            new ArgumentParser(
                new Argument(ArgumentType.Command, "property", "p",
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
            Console.WriteLine("Bench CLI v" + Program.Version());
            Console.WriteLine("Command: app");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  bench app <command> arg*");
            Console.WriteLine("  bench app (/? | -? | -h | --help)");
            Console.WriteLine(HelpFormatter.GenerateHelp(parser));
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
                case "property":
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
