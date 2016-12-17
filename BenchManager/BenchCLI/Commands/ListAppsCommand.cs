using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ListAppsCommand : BenchCommand
    {
        public override string Name => "apps";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command lists apps from the app library.")
                .End(BlockType.Paragraph)
                .Paragraph("You can specify the base set of apps and filter the apps to list.");
        }

        private DataOutputFormat Format { get { return ((ListCommand)Parent).Format; } }

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration();
            using (var w = TableWriterFactory.Create(Format))
            {
                w.Initialize("ID", "Version", "Typ", "Active", "Label");
                foreach (var app in cfg.Apps)
                {
                    w.Write(app.ID, app.Version, app.Typ, app.IsActive, app.Label);
                }
            }
            return true;
        }
    }
}
