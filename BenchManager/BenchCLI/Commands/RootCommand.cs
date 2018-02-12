using System;
using System.Collections.Generic;
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
        private readonly BenchCommand dashboardCommand = new DashboardCommand();
        private readonly BenchCommand manageCommand = new ManageCommand();
        private readonly BenchCommand transferCommand = new TransferCommand();
        private readonly BenchCommand appCommand = new AppCommand();
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
                .Link("https://winbench.org", "the project website")
                .Text(" for a description of the Bench system.")
                .End(BlockType.Paragraph);

            RegisterSubCommand(helpCommand);
            RegisterSubCommand(listCommand);
            RegisterSubCommand(dashboardCommand);
            RegisterSubCommand(manageCommand);
            RegisterSubCommand(transferCommand);
            RegisterSubCommand(appCommand);
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

            var flagVerbose = new FlagArgument(FLAG_VERBOSE, 'v');
            flagVerbose.Description
                .Text("Activate verbose output.");

            var flagNoAssurance = new FlagArgument(FLAG_YES, 'y', "force");
            flagNoAssurance.Description
                .Text("Suppress all assurance questions.");

            var optionHelpFormat = new EnumOptionArgument<DocumentOutputFormat>(
                OPTION_HELP_FORMAT, 'f', DEF_HELP_FORMAT);
            optionHelpFormat.Description
                .Text("Specify the output format of help texts.");

            var optionLogFile = new OptionArgument(OPTION_LOGFILE, 'l',
                ArgumentValidation.IsValidPath,
                "log");
            optionLogFile.Description
                .Text("Specify a custom location for the log file.");
            optionLogFile.PossibleValueInfo
                .Text("A path to the log file.");
            optionLogFile.DefaultValueInfo
                .Text("Auto generated filename in ")
                .Variable("bench root")
                .Text("\\log\\")
                .Text(".");

            var optionBenchRoot = new OptionArgument(OPTION_BENCH_ROOT, 'r',
                v => ArgumentValidation.IsValidPath(v) && Directory.Exists(v),
                "base");
            optionBenchRoot.Description
                .Text("Specify the root directory of the Bench environment.");
            optionBenchRoot.PossibleValueInfo
                .Text("A path to a valid Bench root directory.");
            optionBenchRoot.DefaultValueInfo
                .Text("The root directory of the Bench environment, this Bench CLI belongs to.");

            var commandHelp = new CommandArgument(helpCommand.Name, 'h');
            commandHelp.Description
                .Text("Display the full help for all commands.");

            var commandList = new CommandArgument(listCommand.Name, 'l');
            commandList.Description
                .Text("List different kinds of objects in the Bench environment.");

            var commandDashboard = new CommandArgument(dashboardCommand.Name, 'b', "gui");
            commandDashboard.Description
                .Text("Start the ").Emph("Bench Dashboard").Text(".");

            var commandManage = new CommandArgument(manageCommand.Name, 'm');
            commandManage.Description
                .Text("Manage the Bench environment and its configuration.");

            var commandTransfer = new CommandArgument(transferCommand.Name, 't');
            commandTransfer.Description
                .Text("Copy or export this Bench environment.");

            var commandApp = new CommandArgument(appCommand.Name, 'a');
            commandApp.Description
                .Text("Manage individual apps.");
            commandApp.SyntaxInfo
                .Append(HelpFormatter.CommandSyntax, appCommand);

            var commandProject = new CommandArgument(projectCommand.Name, 'p', "prj");
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
                commandManage,
                commandTransfer,
                commandDashboard,
                commandApp,
                commandProject);
        }

        private string LogFilePath()
        {
            var p = Arguments.GetOptionValue(OPTION_LOGFILE);
            return p == null || Path.IsPathRooted(p)
                ? p : Path.Combine(Environment.CurrentDirectory, p);
        }

        protected override bool ValidateArguments()
        {
            Verbose = Arguments.GetFlag(FLAG_VERBOSE);
            NoAssurance = Arguments.GetFlag(FLAG_YES);
            HelpFormat = (DocumentOutputFormat)Enum.Parse(typeof(DocumentOutputFormat),
                Arguments.GetOptionValue(OPTION_HELP_FORMAT, DEF_HELP_FORMAT.ToString()), true);

            WriteDetail("{0} v{1}: {2}", ToolName, ToolVersion, Program.CliExecutable());

            var rp = Arguments.GetOptionValue(OPTION_BENCH_ROOT, DefaultRootPath());
            if (rp != null)
            {
                RootPath = Path.IsPathRooted(rp)
                    ? rp
                    : Path.Combine(Environment.CurrentDirectory, rp);
                WriteDetail("Bench Root: " + (RootPath ?? "unknown"));
            }
            else
            {
                WriteError("No valid Bench root path.");
                WriteLine("Try specifying the Bench root directory with the --root option.");
                return false;
            }

            if (BenchTasks.IsDashboardSupported)
            {
                WriteDetail("Bench Dashboard: " + (DashboardExecutable(RootPath) ?? "not found"));
            }
            else
            {
                WriteDetail("Bench Dashboard: Not Supported. Microsoft .NET Framework 4.5 not installed.");
            }

            LogFile = LogFilePath();
            WriteDetail("Log File: " + (LogFile ?? "automatic"));

            return true;
        }
    }
}
