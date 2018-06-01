using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class SearchCommand : BenchCommand
    {
        public override string Name => "search";

        private const string FLAG_TABLE = "table";
        private const string OPTION_FORMAT = "format";
        private const string OPTION_LIMIT = "limit";
        private const string POSITIONAL_KEYWORDS = "keywords";

        private const DataOutputFormat DEF_FORMAT = DataOutputFormat.Plain;
        private const int DEF_LIMIT = 50;

        public DataOutputFormat Format
            => (DataOutputFormat)Enum.Parse(
                typeof(DataOutputFormat),
                Arguments.GetOptionValue(OPTION_FORMAT, DEF_FORMAT.ToString()),
                ignoreCase: true);

        public bool OutputAsTable => Arguments.GetFlag(FLAG_TABLE);

        public int Limit
            => int.Parse(
               Arguments.GetOptionValue(OPTION_LIMIT, DEF_LIMIT.ToString(CultureInfo.InvariantCulture)),
               CultureInfo.InvariantCulture);

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command searches for apps in the Bench environment.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("Choose a sub-command to specify the kind of object, you want to list.")
                .End(BlockType.Paragraph);

            var flagTable = new FlagArgument(FLAG_TABLE, 't');
            flagTable.Description
                .Text("Prints the search result as a table.")
                .Text(" Otherwise only the IDs of found apps are printed.");

            var optionFormat = new EnumOptionArgument<DataOutputFormat>(OPTION_FORMAT, 'f', DEF_FORMAT);
            optionFormat.Description
                .Text("Specifies the output format of the listed data.");

            parser.AcceptsAdditionalArguments = true;
            parser.AdditionalArgumentsDescription
                .Text("All additional arguments are used as search keywords.");

            parser.RegisterArguments(
                flagTable,
                optionFormat);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration();
            var matches = cfg.Apps.Search(args);
            WriteDetail($"Found {matches.Length} apps");
            if (matches.Length > 0)
            {
                WriteDetail("Max score: " + matches.First().Score.ToString(CultureInfo.InvariantCulture));
                WriteDetail("Min score: " + matches.Last().Score.ToString(CultureInfo.InvariantCulture));
            }
            if (OutputAsTable)
            {
                var sortedMatches = matches
                    .OrderBy(m => m.App.Label)
                    .OrderByDescending(m => m.Score) // stable sort
                    .Take(Limit)
                    .ToList();
                using (var w = TableWriterFactory.Create(Format))
                {
                    w.Initialize("Score", "ID", "Label", "Installed");
                    foreach (var match in sortedMatches)
                    {
                        w.Write(match.Score, match.App.ID, match.App.Label, match.App.IsInstalled);
                    }
                }
            }
            else
            {
                var sortedMatches = matches
                    .OrderByDescending(m => m.Score) // stable sort
                    .Take(Limit)
                    .ToList();
                foreach (var match in sortedMatches)
                {
                    WriteLine(match.App.ID);
                }
            }
            return matches.Length > 0;
        }
    }
}
