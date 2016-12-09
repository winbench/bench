using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
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

        private BenchConfiguration config;

        protected override ArgumentParser InitializeArgumentParser()
        {
            var optionFormat = new EnumOptionArgument<DocumentOutputFormat>(
                OPTION_FORMAT, "f", DEF_FORMAT, "fmt");
            optionFormat.Description
                .Text("Specify the output format.");

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

            config = LoadConfiguration();
            if (!config.Apps.Exists(appId))
            {
                WriteError("Unknown app ID: " + appId);
                return false;
            }

            var app = config.Apps[appId];
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
            writer.Headline2("Description");
            writer.Begin(BlockType.List);
            WriteProperty(writer, "ID", app.ID, InlineType.Keyword);
            WriteProperty(writer, "Label", app.Label);
            if (app.IsInstalled && app.Launcher != null)
            {
                WriteProperty(writer, "Launcher", app.Launcher);
            }
            WriteProperty(writer, "App Type", app.Typ, InlineType.Keyword);
            WriteProperty(writer, "Version", app.Version, InlineType.Keyword);
            writer.End(BlockType.List);
            writer.Headline2("State");
            writer.Paragraph(app.LongStatus);
            var dependencies = app.Dependencies;
            var responsibilities = app.Responsibilities;
            if (dependencies.Length > 0 || responsibilities.Length > 0)
            {
                writer.Headline2("Relationships");
                writer.Begin(BlockType.List);
                if (dependencies.Length > 0)
                {
                    Array.Sort(dependencies);
                    writer.Begin(BlockType.ListItem)
                        .Text("Dependencies:")
                        .Begin(BlockType.List);
                    foreach (var d in dependencies)
                    {
                        writer.Begin(BlockType.ListItem)
                            .Keyword(d)
                            .End(BlockType.ListItem);
                    }
                    writer.End(BlockType.List);
                    writer.End(BlockType.ListItem);
                }
                if (responsibilities.Length > 0)
                {
                    Array.Sort(responsibilities);
                    writer.Begin(BlockType.ListItem)
                        .Text("Responsibilities:")
                        .Begin(BlockType.List);
                    foreach (var r in app.Responsibilities)
                    {
                        writer.Begin(BlockType.ListItem)
                            .Keyword(r)
                            .End(BlockType.ListItem);
                    }
                    writer.End(BlockType.List);
                    writer.End(BlockType.ListItem);
                }
                writer.End(BlockType.List);
            }
            writer.Headline2("Paths and Resources");
            writer.Begin(BlockType.List);
            if (app.IsInstalled)
            {
                WriteProperty(writer, "Installation Dir", app.Dir, InlineType.Keyword);
                WriteProperty(writer, "Main Executable", app.Exe, InlineType.Keyword);
            }
            if (app.IsResourceCached)
            {
                WriteProperty(writer, "Cached Resource",
                    Path.Combine(config.GetStringValue(PropertyKeys.DownloadDir),
                        app.ResourceArchiveName ?? app.ResourceFileName),
                    InlineType.Keyword);
            }
            if (app.Url != null)
            {
                WriteProperty(writer, "Resource URL", app.Url);
            }
            writer.End(BlockType.List);

            writer.End(BlockType.Document);
        }

        private void WriteProperty(DocumentWriter writer, string key, string value)
        {
            writer
                .Begin(BlockType.ListItem)
                .Text(key).Text(": ").Text(value)
                .End(BlockType.ListItem);
        }
        private void WriteProperty(DocumentWriter writer, string key, string value, InlineType type)
        {
            writer.Begin(BlockType.ListItem);
            writer.Text(key).Text(": ");
            writer.Inline(type, value);
            writer.End(BlockType.ListItem);
        }
    }
}
