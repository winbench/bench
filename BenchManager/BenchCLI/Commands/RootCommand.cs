using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class RootCommand : BenchCommand
    {
        private const string FLAG_VERBOSE = "verbose";
        private const string FLAG_YES = "yes";
        private const string OPTION_HELP_FORMAT = "help-format";
        private const string OPTION_LOGFILE = "logfile";
        private const string OPTION_BENCH_ROOT = "root";

        private readonly BenchCommand helpCommand = new HelpCommand();
        private readonly BenchCommand listCommand = new ListCommand();

        private readonly BenchCommand initializeCommand = new InitializeCommand();
        private readonly BenchCommand autoSetupCommand = new AutoSetupCommand();
        private readonly BenchCommand updateEnvCommand = new UpdateEnvironmentCommand();
        private readonly BenchCommand reinstallCommand = new ReinstallCommand();
        private readonly BenchCommand renewCommand = new RenewCommand();
        private readonly BenchCommand upgradeCommand = new UpgradeCommand();

        private readonly BenchCommand appCommand = new AppCommand();
        private readonly BenchCommand configCommand = new ConfigCommand();
        private readonly BenchCommand downloadsCommand = new DownloadsCommand();
        private readonly BenchCommand projectCommand = new ProjectCommand();


        public override string Name
            => Assembly.GetExecutingAssembly()
                .GetName().Name.ToLowerInvariant();

        private const DocumentOutputFormat DEF_HELP_FORMAT = DocumentOutputFormat.Plain;

        public RootCommand()
        {
            ToolName = "Bench CLI";
            ToolVersion = Program.Version();
            ToolDescription
                .Begin(BlockType.Paragraph)
                .Text("The ").Emph("Bench CLI")
                .Text(" allows to interact with a Bench environment on the command line.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("It supports a hierarchy of sub-commands with flags and options,")
                .Text(" which can be specified as command line arguments.").LineBreak()
                .Text("Additionally it supports an ").Emph("interactive mode")
                .Text(" when called without a sub-command specified.").LineBreak()
                .Text("Help texts can be displayed for each sub-command")
                .Text(" with the ").Keyword("-?").Text(" argument.")
                .Text(" The help texts can be printed in ").Emph("different formats").Text(".")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("Take a look at ")
                .Link("http://mastersign.github.io/bench", "the project website")
                .Text(" for a description of Bench.")
                .End(BlockType.Paragraph);

            RegisterSubCommand(helpCommand);
            RegisterSubCommand(listCommand);

            RegisterSubCommand(initializeCommand);
            RegisterSubCommand(autoSetupCommand);
            RegisterSubCommand(updateEnvCommand);
            RegisterSubCommand(reinstallCommand);
            RegisterSubCommand(renewCommand);
            RegisterSubCommand(upgradeCommand);

            RegisterSubCommand(appCommand);
            RegisterSubCommand(configCommand);
            RegisterSubCommand(downloadsCommand);
            RegisterSubCommand(projectCommand);

        }

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name)
                .Text(" command is the executable of the Bench CLI.").LineBreak()
                .Text("You can call it without a sub-command to enter the ")
                .Emph("interactive mode").Text(".")
                .End(BlockType.Paragraph);

            var flagVerbose = new FlagArgument(FLAG_VERBOSE, "v");
            flagVerbose.Description
                .Text("Activates verbose output.");

            var flagNoAssurance = new FlagArgument(FLAG_YES, "y", "force");
            flagNoAssurance.Description
                .Text("Suppresses all assurance questions.");

            var optionHelpFormat = new EnumOptionArgument<DocumentOutputFormat>(
                OPTION_HELP_FORMAT, "hf", DEF_HELP_FORMAT);
            optionHelpFormat.Description
                .Text("Specifies the output format of help texts.");

            var optionLogFile = new OptionArgument(OPTION_LOGFILE, "l",
                ArgumentValidation.IsValidPath,
                "log");
            optionLogFile.Description
                .Text("Specifies a custom location for the log file.");
            optionLogFile.PossibleValueInfo
                .Text("A path to the log file.");
            optionLogFile.DefaultValueInfo
                .Text("Auto generated filename in ")
                .Variable("bench root")
                .Text("\\log\\")
                .Text(".");

            var optionBenchRoot = new OptionArgument(OPTION_BENCH_ROOT, "r",
                v => ArgumentValidation.IsValidPath(v) && Directory.Exists(v),
                "base");
            optionBenchRoot.Description
                .Text("Specifies the root directory of the Bench environment.");
            optionBenchRoot.PossibleValueInfo
                .Text("A path to a valid Bench root directory.");
            optionBenchRoot.DefaultValueInfo
                .Text("The root directory of the Bench environment, this Bench CLI belongs to.");

            var commandHelp = new CommandArgument(helpCommand.Name, "h");
            commandHelp.Description
                .Text("Displays the full help for all commands.");

            var commandList = new CommandArgument(listCommand.Name, "l");
            commandList.Description
                .Text("Lists different kinds of objects in the Bench environment.");

            var commandInitialize = new CommandArgument(initializeCommand.Name, "i", "init");
            commandInitialize.Description
                .Text("Initialize the Bench configuration and start the setup process.");

            var commandSetup = new CommandArgument(autoSetupCommand.Name, "s");
            commandSetup.Description
                .Text("Run the auto-setup for the active Bench apps.");

            var commandUpdateEnv = new CommandArgument(updateEnvCommand.Name, "e");
            commandUpdateEnv.Description
                .Text("Update the paths in the Bench environment.");

            var commandReinstall = new CommandArgument(reinstallCommand.Name, "r");
            commandReinstall.Description
                .Text("Remove all installed apps, then install all active apps.");

            var commandRenew = new CommandArgument(renewCommand.Name, "n");
            commandRenew.Description
                .Text("Redownload all app resources, remove all installed apps, then install all active apps.");

            var commandUpgrade = new CommandArgument(upgradeCommand.Name, "u");
            commandUpgrade.Description
                .Text("Download and extract the latest Bench release, then run the auto-setup.");

            var commandConfig = new CommandArgument(configCommand.Name, "c", "cfg");
            commandConfig.Description
                .Text("Read or write values from the user configuration.");
            commandConfig.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, configCommand);

            var commandDownloads = new CommandArgument(downloadsCommand.Name, "d", "cache", "dl");
            commandDownloads.Description
                .Text("Manage the app resource cache.");
            commandDownloads.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, downloadsCommand);

            var commandApp = new CommandArgument(appCommand.Name, "a");
            commandApp.Description
                .Text("Manage individual apps.");
            commandApp.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appCommand);

            var commandProject = new CommandArgument(projectCommand.Name, "p", "prj");
            commandProject.Description
                .Text("Manage projects in the Bench environment.");
            commandProject.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, projectCommand);

            parser.RegisterArguments(
                flagVerbose,
                flagNoAssurance,

                optionHelpFormat,
                optionLogFile,
                optionBenchRoot,

                commandHelp,
                commandList,

                commandInitialize,
                commandSetup,
                commandUpdateEnv,
                commandReinstall,
                commandRenew,
                commandUpgrade,

                commandConfig,
                commandDownloads,
                commandApp,
                commandProject);
        }

        public string LogFilePath
        {
            get
            {
                var p = Arguments.GetOptionValue(OPTION_LOGFILE);
                return p == null || Path.IsPathRooted(p)
                    ? p : Path.Combine(Environment.CurrentDirectory, p);
            }
        }

        protected override bool ValidateArguments()
        {
            Verbose = Arguments.GetFlag(FLAG_VERBOSE);
            NoAssurance = Arguments.GetFlag(FLAG_YES);
            HelpFormat = (DocumentOutputFormat)Enum.Parse(typeof(DocumentOutputFormat),
                Arguments.GetOptionValue(OPTION_HELP_FORMAT, DEF_HELP_FORMAT.ToString()), true);

            WriteDetail("{0} v{1}: {2}", ToolName, ToolVersion, Program.CliExecutable());

            if (BenchTasks.IsDashboardSupported)
            {
                WriteDetail("Bench Dashboard: " + (DashboardExecutable() ?? "not found"));
            }
            else
            {
                WriteDetail("Bench Dashboard: Not Supported. Microsoft .NET Framework 4.5 not installed.");
            }

            var rp = Arguments.GetOptionValue(OPTION_BENCH_ROOT, DefaultRootPath());
            if (rp != null)
            {
                RootPath = Path.IsPathRooted(rp)
                    ? rp
                    : Path.Combine(Environment.CurrentDirectory, rp);
            }
            else
            {
                WriteError("No valid Bench root path.");
                WriteLine("Try specifying the Bench root directory with the --root option.");
                return false;
            }

            WriteDetail("Bench Root: " + (RootPath ?? "unknown"));
            WriteDetail("Log File: " + (LogFilePath ?? "automatic"));

            return true;
        }
    }
}
