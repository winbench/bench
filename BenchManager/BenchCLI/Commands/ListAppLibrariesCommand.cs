using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ListAppLibrariesCommand : BenchCommand
    {
        public override string Name => "applibs";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command lists all loaded app libraries.")
                .End(BlockType.Paragraph);
        }

        private DataOutputFormat Format => ((ListCommand)Parent).Format;

        private bool OutputAsTable => ((ListCommand)Parent).OutputAsTable;

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration(withApps: true);
            var appLibs = cfg.AppLibraries;
            if (OutputAsTable)
            {
                using (var w = TableWriterFactory.Create(Format))
                {
                    w.Initialize(new[] { "Order", "ID", "Path", "URL" });
                    for (int i = 0; i < appLibs.Length; i++)
                    {
                        var l = appLibs[i];
                        w.Write((i + 1).ToString().PadLeft(5), l.ID, l.BaseDir, l.Url.OriginalString);
                    }
                }
            }
            else
            {
                foreach (var l in appLibs)
                {
                    Console.WriteLine("{0}={1}", l.ID, l.Url);
                }
            }
            return true;
        }
    }
}
