using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ListConfigFilesCommand : BenchCommand
    {
        public override string Name => "files";

        private const string OPTION_TYPE = "type";
        private const ConfigurationFileType DEF_TYPE = ConfigurationFileType.All;

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command lists the paths of all loaded configuration files.")
                .End(BlockType.Paragraph);

            var optionType = new EnumOptionArgument<ConfigurationFileType>(OPTION_TYPE, 't', ConfigurationFileType.All);
            optionType.Description
                .Text("Specify the type of files to show.");

            parser.RegisterArguments(
                optionType);
        }

        private ConfigurationFileType Type => (ConfigurationFileType)Enum.Parse(typeof(ConfigurationFileType),
            Arguments.GetOptionValue(OPTION_TYPE, DEF_TYPE.ToString()));

        private DataOutputFormat Format => ((ListCommand)Parent).Format;

        private bool OutputAsTable => ((ListCommand)Parent).OutputAsTable;

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration(withApps: true);
            var files = cfg.GetConfigurationFiles(Type, actuallyLoaded: true);
            if (OutputAsTable)
            {
                using (var w = TableWriterFactory.Create(Format))
                {
                    w.Initialize(new[] { "Order", "Type", "Path" });
                    foreach (var f in files)
                    {
                        w.Write(f.OrderIndex.ToString().PadLeft(5), f.Type.ToString(), f.Path);
                    }
                }
            }
            else
            {
                foreach (var f in files)
                {
                    Console.WriteLine(f.Path);
                }
            }
            return true;
        }
    }
}
