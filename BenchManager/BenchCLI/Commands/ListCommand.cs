using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ListCommand : BenchCommand
    {
        public override string Name => "list";

        private const string FLAG_TABLE = "table";
        private const string OPTION_FORMAT = "format";

        private const DataOutputFormat DEF_FORMAT = DataOutputFormat.Plain;

        private readonly BenchCommand listConfigFilesCommand = new ListConfigFilesCommand();
        private readonly BenchCommand listAppLibsCommand = new ListAppLibrariesCommand();
        private readonly BenchCommand listAppsCommand = new ListAppsCommand();

        public ListCommand()
        {
            RegisterSubCommand(listConfigFilesCommand);
            RegisterSubCommand(listAppLibsCommand);
            RegisterSubCommand(listAppsCommand);
        }

        public DataOutputFormat Format
            => (DataOutputFormat)Enum.Parse(
                    typeof(DataOutputFormat),
                    Arguments.GetOptionValue(OPTION_FORMAT, DEF_FORMAT.ToString()),
                    ignoreCase: true);

        public bool OutputAsTable => Arguments.GetFlag(FLAG_TABLE);

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command lists different kinds of objects from the Bench environment.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("Choose a sub-command to specify the kind of object, you want to list.")
                .End(BlockType.Paragraph);

            var flagTable = new FlagArgument(FLAG_TABLE, 't');
            flagTable.Description
                .Text("Prints properties of the listed objects as a table.")
                .Text(" Otherwise only a short form is printed.");

            var optionFormat = new EnumOptionArgument<DataOutputFormat>(OPTION_FORMAT, 'f', DEF_FORMAT);
            optionFormat.Description
                .Text("Specifies the output format of the listed data.");

            var commandListFiles = new CommandArgument(listConfigFilesCommand.Name, 'f');
            commandListFiles.Description
                .Text("List configuration and app library index files.");

            var commandListAppLibs = new CommandArgument(listAppLibsCommand.Name, 'l');
            commandListAppLibs.Description
                .Text("List app libraries with ID and URL.");

            var commandListApps = new CommandArgument(listAppsCommand.Name, 'a');
            commandListApps.Description
                .Text("List apps from the app library.");

            parser.RegisterArguments(
                flagTable,
                optionFormat,
                commandListFiles,
                commandListAppLibs,
                commandListApps);
        }
    }
}
