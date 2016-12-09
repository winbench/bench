using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;

namespace Mastersign.Bench.Cli.Commands
{
    class DownloadsCommand : BenchCommand
    {
        public const string CMD_NAME = "downloads";

        private const string COMMAND_CLEAN = "clean";
        private const string COMMAND_PURGE = "purge";
        private const string COMMAND_DOWNLOAD = "download";

        public override string Name => CMD_NAME;

        protected override ArgumentParser InitializeArgumentParser()
        {
            var commandClean = new CommandArgument(COMMAND_CLEAN, "c", "cl");
            commandClean.Description
                .Text("Deletes obsolete app resources.");

            var commandPurge = new CommandArgument(COMMAND_PURGE, "x");
            commandPurge.Description
                .Text("Deletes all cached app resources.");

            var commandDownload = new CommandArgument(COMMAND_DOWNLOAD, "d", "dl");
            commandDownload.Description
                .Text("Downloads the app resources for all active apps.");

            return new ArgumentParser(CMD_NAME,
                commandClean,
                commandPurge,
                commandDownload);
        }

        protected override bool ExecuteUnknownSubCommand(string command, string[] args)
        {
            switch (command)
            {
                case COMMAND_CLEAN:
                    return TaskClean(args);
                case COMMAND_PURGE:
                    return TaskPurge(args);
                case COMMAND_DOWNLOAD:
                    return TaskDownload(args);

                default:
                    WriteError("Unsupported command: " + command + ".");
                    return false;
            }
        }

        private bool TaskClean(string[] args)
            => RunManagerTask(mgr => mgr.CleanUpAppResources());

        private bool TaskPurge(string[] args)
        {
            if (!AskForAssurance("Are you sure, you want to delete all downloaded app resources?"))
            {
                return false;
            }
            return RunManagerTask(mgr => mgr.DeleteAppResources());
        }

        private bool TaskDownload(string[] args)
            => RunManagerTask(mgr => mgr.DownloadAppResources());
    }
}
