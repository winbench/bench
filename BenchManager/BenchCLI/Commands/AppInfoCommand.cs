using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppInfoCommand : BenchCommand
    {
        public const string CMD_NAME = "info";

        private const string OPTION_FORMAT = "format";
        private const string POSITIONAL_APP_ID = "App ID";

        private const DocumentOutputFormat DEF_FORMAT = DocumentOutputFormat.Plain;

        public override string Name => CMD_NAME;

        private DocumentOutputFormat Format = DEF_FORMAT;

        protected override ArgumentParser InitializeArgumentParser()
        {
            var optionFormat = new OptionArgument(OPTION_FORMAT, "f",
                v => ArgumentValidation.IsEnumMember(v, typeof(DocumentOutputFormat)),
                "fmt");
            optionFormat.Description
                .Text("Specify the output format.");
            optionFormat.PossibleValueInfo
                .Syntactic(string.Join(" | ", Enum.GetNames(typeof(DocumentOutputFormat))));
            optionFormat.DefaultValueInfo
                .Syntactic(DEF_FORMAT.ToString());

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to display the description for.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            var parser = new ArgumentParser(Name,
                optionFormat,
                positionalAppId);
            parser.Description
                .Paragraph("Displays a human readable description of an app.");

            return parser;
        }

        protected override bool ValidateArguments()
        {
            Format = (DocumentOutputFormat)Enum.Parse(typeof(DocumentOutputFormat),
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
            using (var w = DocumentWriterFactory.Create(Format, Console.OpenStandardOutput()))
            {
                WriteAppInfo(app, w);
            }

            return true;
        }

        private void WriteAppInfo(AppFacade app, DocumentWriter writer)
        {
            writer.Begin(BlockType.Document);
            writer.Title(app.Label);

            writer.Begin(BlockType.List);
            WriteProperty(writer, "ID", app.ID);
            writer.End(BlockType.List);

            writer.End(BlockType.Document);
        }

        private void WriteProperty(DocumentWriter writer, string key, string value)
        {
            writer
                .Begin(BlockType.ListItem)
                .Keyword(key).Text(": ").Syntactic(value)
                .End(BlockType.ListItem);
        }
    }
}
