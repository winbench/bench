using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ConfigEditCommand : BenchCommand
    {
        private const string FLAG_DETACHED = "detached";

        public override string Name => "edit";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command opens the user configuration in the default Markdown editor.")
                .End(BlockType.Paragraph);

            var flagDetached = new FlagArgument(FLAG_DETACHED, 'd', "async");
            flagDetached.Description
                .Text("Do not wait for the editor to be closed.");

            parser.RegisterArguments(
                flagDetached);
        }

        private bool Detached => Arguments.GetFlag(FLAG_DETACHED);

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration(true);
            var path = cfg.GetStringValue(ConfigPropertyKeys.UserConfigFile);
            return EditMarkdownFile(cfg, path);
        }

        private static string SystemEditorPath
            => Path.Combine(
                Environment.GetEnvironmentVariable("SystemRoot"),
                "notepad.exe");

        private bool EditFile(BenchConfiguration config, string path, string appId)
        {
            if (!File.Exists(path))
            {
                WriteError("File not found.");
                WriteLine("Path: " + path);
                return false;
            }
            WriteDetail("Opening editor for: " + path);

            var editorApp = config.Apps[appId];
            if (editorApp.IsInstalled)
            {
                LaunchApp(config, Detached, appId, path);
                return false;
            }
            else
            {
                if (!File.Exists(SystemEditorPath))
                {
                    WriteError("Could not find " + Path.GetFileName(SystemEditorPath) + ".");
                    WriteLine("Path: " + SystemEditorPath);
                    return false;
                }
                var p = System.Diagnostics.Process.Start(SystemEditorPath, path);
                if (!Detached)
                {
                    p.WaitForExit();
                    return p.ExitCode == 0;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool EditMarkdownFile(BenchConfiguration config, string path)
            => EditFile(config, path, config.GetStringValue(ConfigPropertyKeys.MarkdownEditorApp));
    }
}
