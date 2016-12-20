using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class UpgradeCommand : BenchCommand
    {
        public override string Name => "upgrade";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command checks if a new version of Bench is available, and incase installs it.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Strong("WARNING: This command ist not implemented yet.")
                .End(BlockType.Paragraph);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            if (!AskForAssurance("Are you sure you want to upgrade the Bench system?")) return false;

            if (!RunManagerTask(mgr => mgr.DownloadBenchUpdate())) return false;

            var si = new System.Diagnostics.ProcessStartInfo("cmd",
                "/C \"@ECHO.Starting Bench Upgrade... && @ECHO. && @ECHO.Make sure, all programs in the Bench environment are closed. && @PAUSE && @CALL ^\""
                    + Path.Combine(RootPath, "bench-install.bat") + "^\"\"");
            si.UseShellExecute = true;
            si.WorkingDirectory = RootPath;
            System.Diagnostics.Process.Start(si);

            return true;
        }
    }
}
