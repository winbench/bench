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
        private const FileType DEF_TYPE = FileType.All;

        [Flags]
        public enum FileType : int
        {
            All = 0xFFFF,
            Config = 0x000F,
            AppLib = 0x00F0,
            AppSelection = 0x0F00,
            BenchConfig = 0x0001,
            UserConfig = 0x0002,
            SiteConfig = 0x0004,
            BenchAppLib = 0x0010,
            UserAppLib = 0x0020,
            Activation = 0x0100,
            Deactivation = 0x0200,
        }

        public class ConfigurationFile
        {
            public FileType Type { get; private set; }

            public int OrderIndex { get; private set; }

            public string Path { get; private set; }

            public ConfigurationFile(FileType type, int orderIndex, string path)
            {
                Type = type;
                OrderIndex = orderIndex;
                Path = path;
            }
        }

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command lists the paths of all loaded configuration files.")
                .End(BlockType.Paragraph);

            var optionType = new EnumOptionArgument<FileType>(OPTION_TYPE, 't', FileType.All);
            optionType.Description
                .Text("Specify the type of files to show.");

            parser.RegisterArguments(
                optionType);
        }

        private FileType Type => (FileType)Enum.Parse(typeof(FileType),
            Arguments.GetOptionValue(OPTION_TYPE, DEF_TYPE.ToString()));

        private DataOutputFormat Format => ((ListCommand)Parent).Format;

        private bool OutputAsTable => ((ListCommand)Parent).OutputAsTable;

        protected override bool ExecuteCommand(string[] args)
        {
            List<ConfigurationFile> files = GetPaths(Type);
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

        private List<ConfigurationFile> GetPaths(FileType type)
        {
            var cfg = LoadConfiguration(true);
            var files = new List<ConfigurationFile>();
            if ((type & FileType.BenchConfig) == FileType.BenchConfig)
            {
                files.Add(new ConfigurationFile(FileType.BenchConfig, 0,
                    Path.Combine(
                        cfg.BenchRootDir,
                        BenchConfiguration.CONFIG_FILE)));
            }
            if ((type & FileType.UserConfig) == FileType.UserConfig)
            {

                var userConfigFile = cfg.GetStringValue(PropertyKeys.CustomConfigFile);
                if (File.Exists(userConfigFile))
                {
                    files.Add(new ConfigurationFile(FileType.UserConfig, 1,
                        userConfigFile));
                }
            }
            if ((type & FileType.SiteConfig) == FileType.SiteConfig)
            {
                var siteConfigFiles = cfg.FindSiteConfigFiles();
                for (int i = 0; i < siteConfigFiles.Length; i++)
                {
                    files.Add(new ConfigurationFile(FileType.SiteConfig, 10 + i,
                        siteConfigFiles[i]));
                }
            }
            if ((type & FileType.BenchAppLib) == FileType.BenchAppLib)
            {
                var appLibraries = cfg.AppLibraries;
                for (var i = 0; i < appLibraries.Length; i++)
                {
                    files.Add(new ConfigurationFile(FileType.BenchAppLib, 100 + i,
                        Path.Combine(
                            appLibraries[i].BaseDir,
                            cfg.GetStringValue(PropertyKeys.AppLibIndexFileName))));
                }
            }
            if ((type & FileType.UserAppLib) == FileType.UserAppLib)
            {
                var userAppLib = Path.Combine(
                    cfg.GetStringValue(PropertyKeys.CustomConfigDir),
                    cfg.GetStringValue(PropertyKeys.AppLibIndexFileName));
                if (File.Exists(userAppLib))
                {
                    files.Add(new ConfigurationFile(FileType.UserAppLib, 999,
                        userAppLib));
                }
            }
            if ((type & FileType.Activation) == FileType.Activation)
            {
                var activationFile = cfg.GetStringValue(PropertyKeys.AppActivationFile);
                if (File.Exists(activationFile))
                {
                    files.Add(new ConfigurationFile(FileType.Activation, 1000,
                        activationFile));
                }
            }
            if ((type & FileType.Deactivation) == FileType.Deactivation)
            {
                var deactivationFile = cfg.GetStringValue(PropertyKeys.AppDeactivationFile);
                if (File.Exists(deactivationFile))
                {
                    files.Add(new ConfigurationFile(FileType.Deactivation, 1001,
                        deactivationFile));
                }
            }
            return files;
        }
    }
}
