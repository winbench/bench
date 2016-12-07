using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli.Commands
{
    class AppInfoCommand : BenchCommand
    {
        public const string CMD_NAME = "info";

        private const string FLAG_RAW = "raw";

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
            foreach (var p in names)
            {
                PrintProperty(p, lookup[p]);
            }
        }

        private void PrintRawProperties(BenchConfiguration cfg, string appId)
        {
            foreach (var name in cfg.PropertyNames(appId))
            {
                PrintProperty(name, cfg.GetRawGroupValue(appId, name));
            }
        }

        private void PrintProperty(string name, object value)
        {
            if (value is bool)
            {
                value = (bool)value ? "True" : "False";
            }
            else if (value is string[])
            {
                var items = new List<string>();
                foreach (var item in (string[])value)
                {
                    items.Add(EscapeString(item));
                }
                value = "[" + string.Join(", ", items.ToArray()) + "]";
            }
            else if (value is Dictionary<string, string>)
            {
                var pairs = new List<string>();
                foreach (var kvp in (Dictionary<string, string>)value)
                {
                    pairs.Add(string.Format("{0}: {1}",
                        EscapeString(kvp.Key), EscapeString(kvp.Value)));
                }
                value = "{" + string.Join(", ", pairs.ToArray()) + "}";
            }
            else if (value is string)
            {
                value = EscapeString(value.ToString());
            }
            else if (value == null)
            {
                value = "Null";
            }
            else
            {
                value = "Unsupported";
            }
            WriteLine(string.Format("{0} = {1}", name, value));
        }

        private string EscapeString(string value)
        {
            return value != null
                ? "\"" + value.Replace(@"\", @"\\").Replace("\"", "\\\"") + "\""
                : null;
        }
    }
}
