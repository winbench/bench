using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Controller
{
    class DownloadsController : BaseController
    {
        private static ArgumentParser parser;

        public static ArgumentParser Parser
        {
            get
            {
                if (parser == null) { parser = InitializeParser(); }
                return parser;
            }
        }

        private const string COMMAND_CLEAN = "clean";
        private const string COMMAND_PURGE = "purge";
        private const string COMMAND_DOWNLOAD = "download";

        private static ArgumentParser InitializeParser()
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

            return new ArgumentParser(MainController.Parser, MainController.COMMAND_APP,
                commandClean,
                commandPurge,
                commandDownload);
        }

        private readonly MainController mainController;

        public DownloadsController(MainController mainController, string[] args)
        {
            this.mainController = mainController;
            Verbose = mainController.Verbose;
            NoAssurance = mainController.NoAssurance;
            Arguments = Parser.Parse(args);
        }

        protected override void PrintHelp(DocumentWriter w)
        {
            w.Begin(BlockType.Document);
            w.Title("Bench CLI v{0} - [{1}]", Program.Version(), MainController.COMMAND_DOWNLOADS);
            HelpFormatter.WriteHelp(w, Parser);
            w.End(BlockType.Document);
        }

        protected override bool ExecuteCommand(string command, string[] args)
        {
            WriteDetail("Downloads Command: " + command);
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
        {
            using (var mgr = mainController.CreateManager())
            {
                return mgr.CleanUpAppResources();
            }
        }

        private bool TaskPurge(string[] args)
        {
            if (!AskForAssurance("Are you sure, you want to delete all downloaded app resources?"))
            {
                return false;
            }
            using (var mgr = mainController.CreateManager())
            {
                return mgr.DeleteAppResources();
            }
        }

        private bool TaskDownload(string[] args)
        {
            using (var mgr = mainController.CreateManager())
            {
                return mgr.DownloadAppResources();
            }
        }
    }
}
