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

        private const string OPTION_FORMAT = "format";
        private const DataOutputFormat DEF_FORMAT = DataOutputFormat.Plain;

        private readonly BenchCommand listAppsCommand = new ListAppsCommand();

        public ListCommand()
        {
            RegisterSubCommand(listAppsCommand);
        }

        public DataOutputFormat Format
        {
            get
            {
                return (DataOutputFormat)Enum.Parse(
                    typeof(DataOutputFormat),
                    Arguments.GetOptionValue(OPTION_FORMAT, DEF_FORMAT.ToString()), 
                    true);
            }
        }

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command lists different kinds of objects from the Bench environment.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("Choose a sub-command to specify the kind of obhect, you want to list.")
                .End(BlockType.Paragraph);

            var optionFormat = new EnumOptionArgument<DataOutputFormat>(OPTION_FORMAT, "f", DEF_FORMAT);
            optionFormat.Description
                .Text("Specifies the output format of the listed data.");

            var commandListApps = new CommandArgument(listAppsCommand.Name, "a");
            commandListApps.Description
                .Text("List apps from the app library.");

            parser.RegisterArguments(
                optionFormat,
                commandListApps);
        }
    }
}
