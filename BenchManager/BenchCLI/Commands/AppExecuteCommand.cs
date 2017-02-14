using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppExecuteCommand : BenchCommand
    {
        private const string FLAG_DETACHED = "detached";
        private const string POSITIONAL_APP_ID = "App ID";

        public override string Name => "execute";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command starts the main executable of the specified app.")
                .End(BlockType.Paragraph);

            var flagDetached = new FlagArgument(FLAG_DETACHED, 'd', "async");
            flagDetached.Description
                .Text("Do not wait for the end of the process.");

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to execute.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                flagDetached,
                positionalAppId);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration();

            var appId = Arguments.GetPositionalValue(POSITIONAL_APP_ID);

            if (!cfg.Apps.Exists(appId))
            {
                WriteError("The app '{0}' was not found.", appId);
                return false;
            }

            var app = cfg.Apps[appId];
            if (app.Exe == null)
            {
                WriteError("The app '{0}' has no main executable.", app.Label);
                return false;
            }
            WriteDetail("Found apps executable: {0}", app.Exe);

            var detached = Arguments.GetFlag(FLAG_DETACHED);
            WriteDetail("Starting app '{0}' {1} ...", app.Label, detached ? "detached" : "synchronously");

            using (var mgr = new DefaultBenchManager(cfg))
            {
                mgr.Verbose = Verbose;
                if (detached)
                {
                    mgr.ProcessExecutionHost.StartProcess(mgr.Env,
                        cfg.BenchRootDir, app.Exe, CommandLine.FormatArgumentList(args),
                        null, ProcessMonitoring.ExitCode);
                    return true;
                }
                else
                {
                    var r = mgr.ProcessExecutionHost.RunProcess(mgr.Env,
                        cfg.BenchRootDir, app.Exe, CommandLine.FormatArgumentList(args),
                        ProcessMonitoring.ExitCodeAndOutput);
                    Console.Write(r.Output);
                    return r.ExitCode == 0;
                }
            }
        }
    }
}
