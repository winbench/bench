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
        private const string POSITIONAL_APP_ID = "App ID";

        private enum OutputFormat
        {
            Plain,
            Markdown,
            //JSON,
            //XML,
        };

        private const OutputFormat DEF_FORMAT = OutputFormat.Plain;

        public override string Name => CMD_NAME;

        private OutputFormat Format = OutputFormat.Plain;

        protected override ArgumentParser InitializeArgumentParser()
        {
            var flagRaw = new FlagArgument(FLAG_RAW, "r");
            flagRaw.Description
                .Text("Shows the raw properties without expansion and default values.");

            var optionFormat = new OptionArgument(OPTION_FORMAT, "f",
                v => ArgumentValidation.IsEnumMember(v, typeof(OutputFormat)),
                "fmt");
            optionFormat.Description
                .Text("Specify the output format.");
            optionFormat.PossibleValueInfo
                .Syntactic(string.Join(" | ", Enum.GetNames(typeof(OutputFormat))));
            optionFormat.DefaultValueInfo
                .Syntactic(DEF_FORMAT.ToString());

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                v => !string.IsNullOrEmpty(v) && !v.Contains(" "),
                1);
            positionalAppId.Description
                .Text("Specifies the app of which the properties are to be listed.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            return new ArgumentParser(Name,
                flagRaw,
                optionFormat,
                positionalAppId);
        }

        private bool ShowRaw => Arguments.GetFlag(FLAG_RAW);

        protected override bool ValidateArguments()
        {
            Format = (OutputFormat)Enum.Parse(typeof(OutputFormat),
                Arguments.GetOptionValue(OPTION_FORMAT, DEF_FORMAT.ToString()), true);

            return true;
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var appId = Arguments.GetPositionalValue(POSITIONAL_APP_ID);

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

        private IPropertyWriter CreatePropertyWriter()
        {
            switch (Format)
            {
                case OutputFormat.Plain:
                    return new ConsolePropertyWriter();
                case OutputFormat.Markdown:
                    return new MarkdownPropertyWriter(Console.OpenStandardOutput());
                //case OutputFormat.JSON:
                //    return new JsonPropertyWriter(Console.OpenStandardOutput());
                //case OutputFormat.XML:
                //    return new XmlPropertyWriter(Console.OpenStandardOutput());
                default:
                    throw new NotSupportedException();
            }
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

            using (var w = CreatePropertyWriter())
            {
                w.Write("ID", app.ID);
                foreach (var p in names)
                {
                    w.Write(p, lookup[p]);
                }
            }
        }

        private void PrintRawProperties(BenchConfiguration cfg, string appId)
        {
            using (var w = CreatePropertyWriter())
            {
                w.Write("ID", appId);
                foreach (var name in cfg.PropertyNames(appId))
                {
                    w.Write(name, cfg.GetRawGroupValue(appId, name));
                }
            }
        }

    }
}
