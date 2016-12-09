﻿using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;

namespace Mastersign.Bench.Cli.Commands
{
    class AppListPropertiesCommand : BenchCommand
    {
        public const string CMD_NAME = "list-properties";

        private const string FLAG_RAW = "raw";
        private const string OPTION_FORMAT = "format";
        private const string POSITIONAL_APP_ID = "App ID";

        private const DataOutputFormat DEF_FORMAT = DataOutputFormat.Plain;

        public override string Name => CMD_NAME;

        private bool ShowRaw => Arguments.GetFlag(FLAG_RAW);

        private DataOutputFormat Format = DataOutputFormat.Plain;

        protected override ArgumentParser InitializeArgumentParser()
        {
            var flagRaw = new FlagArgument(FLAG_RAW, "r");
            flagRaw.Description
                .Text("Shows the raw properties without expansion and default values.");

            var optionFormat = new OptionArgument(OPTION_FORMAT, "f",
                v => ArgumentValidation.IsEnumMember(v, typeof(DataOutputFormat)),
                "fmt");
            optionFormat.Description
                .Text("Specify the output format.");
            optionFormat.PossibleValueInfo
                .Syntactic(string.Join(" | ", Enum.GetNames(typeof(DataOutputFormat))));
            optionFormat.DefaultValueInfo
                .Syntactic(DEF_FORMAT.ToString());

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app of which the properties are to be listed.");
            positionalAppId.PossibleValueInfo
                .Text("The apps ID, an alphanumeric string without whitespace.");

            var parser = new ArgumentParser(Name,
                flagRaw,
                optionFormat,
                positionalAppId);

            parser.Description
                .Paragraph("Displays the properties of an app.")
                .Paragraph("This command supports different output formats. "
                          + "And you can choose between the expanded or the raw properties.");

            return parser;
        }

        protected override bool ValidateArguments()
        {
            Format = (DataOutputFormat)Enum.Parse(typeof(DataOutputFormat),
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

            using (var w = PropertyWriterFactory.Create(Format))
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
            using (var w = PropertyWriterFactory.Create(Format))
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
