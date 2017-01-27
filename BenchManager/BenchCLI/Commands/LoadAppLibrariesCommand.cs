using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class LoadAppLibraries : BenchCommand
    {
        private const string FLAG_UPDATE = "update";

        public override string Name => "load-app-libs";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command loads missing app libraries.")
                .End(BlockType.Paragraph);

            var updateFlag = new FlagArgument(FLAG_UPDATE, 'u');
            updateFlag.Description
                .Text("Clears the cache and loads the latest version of all app libraries.");

            parser.RegisterArguments(
                updateFlag);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            using (var mgr = CreateManager())
            {
                BenchTasks.DeleteAppLibraries(mgr.Config);
                return mgr.LoadAppLibraries();
            }
        }
    }
}
