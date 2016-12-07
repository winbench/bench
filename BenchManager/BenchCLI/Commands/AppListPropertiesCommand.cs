using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli.Commands
{
    class AppListPropertiesCommand : BenchCommand
    {
        public const string CMD_NAME = "list-properties";

        private const string FLAG_RAW = "raw";
        private const string OPTION_FORMAT = "format";

        private static readonly string[] formats = new[] { "default", "markdown", "json", "xml" };

        public override string Name => CMD_NAME;

        protected override ArgumentParser InitializeArgumentParser(ArgumentParser parent)
        {
            var flagRaw = new FlagArgument(FLAG_RAW, "r");

            return new ArgumentParser(parent, Name,
                flagRaw);
        }

        private bool ShowRaw => Arguments.GetFlag(FLAG_RAW);

        protected override bool ExecuteCommand(string[] args)
        {
            if (args.Length != 1)
            {
                WriteError("Invalid arguments after 'info'.");
                if (ShowRaw)
                    WriteLine("Expected: bench app info --raw <app ID>");
                else
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
            if (ShowRaw)
                PrintRawProperties(cfg, appId);
            else
                PrintProperties(app);
            return true;
        }

        private void PrintProperties(AppFacade app)
        {
            var knownProperties = app.KnownProperties;
            var unknownProperties = app.UnknownProperties;
            var lookup = new Dictionary<string, object>();
            var names = new List<string>();
            foreach (var kvp in knownProperties)
            {
                lookup[kvp.Key] = kvp.Value;
                if (!names.Contains(kvp.Key)) names.Add(kvp.Key);
            }
            foreach (var kvp in unknownProperties)
            {
                lookup[kvp.Key] = kvp.Value;
                if (!names.Contains(kvp.Key)) names.Add(kvp.Key);
            }
            names.Sort();
            //using (var w = new ConsolePropertyWriter())
            using (var w = new MarkdownPropertyWriter(Console.OpenStandardOutput()))
            {
                foreach (var p in names)
                {
                    w.Write(p, lookup[p]);
                }
            }
        }

        private void PrintRawProperties(BenchConfiguration cfg, string appId)
        {
            using (var w = new ConsolePropertyWriter())
            {
                foreach (var name in cfg.PropertyNames(appId))
                {
                    w.Write(name, cfg.GetRawGroupValue(appId, name));
                }
            }
        }

    }
}
