using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Controller
{
    class ConfigController : BaseController
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

        private const string COMMAND_GET = "get";

        private static ArgumentParser InitializeParser()
        {
            var commandGet = new CommandArgument(COMMAND_GET, "g");
            commandGet.Description
                .Text("Reads a configuration value.");
            commandGet.SyntaxInfo
                .Variable("property name");

            return new ArgumentParser(MainController.Parser, MainController.COMMAND_CONFIG,
                commandGet);
        }

        private readonly MainController mainController;

        public ConfigController(MainController mainController, string[] args)
        {
            this.mainController = mainController;
            Verbose = mainController.Verbose;
            NoAssurance = mainController.NoAssurance;
            Arguments = Parser.Parse(args);
        }

        protected override void PrintHelp(DocumentWriter w)
        {
            w.Begin(BlockType.Document);
            w.Title("Bench CLI v{0} - [{1}]", Program.Version(), MainController.COMMAND_CONFIG);
            HelpFormatter.WriteHelp(w, Parser);
            w.End(BlockType.Document);
        }

        protected override bool ExecuteCommand(string command, string[] args)
        {
            WriteDetail("Config Command: " + command);
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

            var cfg = new BenchConfiguration(mainController.RootPath, false, true, true);
            if (!cfg.ContainsValue(propertyName))
            {
                WriteError("Unknown property name: " + propertyName);
                return false;
            }
            WriteDetail("Property: " + propertyName);
            Console.Write(cfg.GetValue(propertyName));
            return true;
        }
    }
}
