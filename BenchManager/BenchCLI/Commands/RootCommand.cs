using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Mastersign.Bench.Cli.Controller
{
    class RootCommand : BenchCommand
    {
        private const string FLAG_VERBOSE = "verbose";
        private const string FLAG_YES = "yes";
        private const string OPTION_LOGFILE = "logfile";
        private const string OPTION_BENCH_ROOT = "root";
        public const string COMMAND_INITIALIZE = "initialize";
        public const string COMMAND_SETUP = "setup";
        public const string COMMAND_UPDATE_ENV = "update-env";
        public const string COMMAND_REINSTALL = "reinstall";
        public const string COMMAND_RENEW = "renew";
        public const string COMMAND_UPGRADE = "upgrade";
        public const string COMMAND_PROJECT = "project";

        protected override ArgumentParser InitializeArgumentParser(ArgumentParser parent)
        {
            var flagVerbose = new FlagArgument(FLAG_VERBOSE, "v", "verb");
            flagVerbose.Description
                .Text("Activates verbose output.");

            var flagNoAssurance = new FlagArgument(FLAG_YES, "y");
            flagNoAssurance.Description
                .Text("Suppresses all assurance questions.");

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
                .Syntactic("\\log\\")
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

            var commandInitialize = new CommandArgument(COMMAND_INITIALIZE, "i");
            commandInitialize.Description
                .Text("Initialize the Bench configuration and start the setup process.");

            var commandSetup = new CommandArgument(COMMAND_SETUP, "s");
            commandSetup.Description
                .Text("Run the auto-setup for the active Bench apps.");

            var commandUpdateEnv = new CommandArgument(COMMAND_UPDATE_ENV, "e");
            commandUpdateEnv.Description
                .Text("Update the paths in the Bench environment.");

            var commandReinstall = new CommandArgument(COMMAND_REINSTALL, "r");
            commandReinstall.Description
                .Text("Remove all installed apps, then install all active apps.");

            var commandRenew = new CommandArgument(COMMAND_RENEW, "n");
            commandRenew.Description
                .Text("Redownload all app resources, remove all installed apps, then install all active apps.");

            var commandUpgrade = new CommandArgument(COMMAND_UPGRADE, "u");
            commandUpgrade.Description
                .Text("Download and extract the latest Bench release, then run the auto-setup.");

            var commandConfig = new CommandArgument(ConfigCommand.CMD_NAME, "c", "cfg");
            commandConfig.Description
                .Text("Read or write values from the user configuration.");
            commandConfig.SyntaxInfo
                .Variable("sub-command")
                .Syntactic(" ")
                .Variable("property name");

            var commandDownloads = new CommandArgument(DownloadsCommand.CMD_NAME, "d", "cache", "dl");
            commandDownloads.Description
                .Text("Manage the app resource cache.");
            commandDownloads.SyntaxInfo
                .Variable("sub-command");

            var commandApp = new CommandArgument(AppCommand.CMD_NAME, "a");
            commandApp.Description
                .Text("Manage individual apps.");
            commandApp.SyntaxInfo
                .Variable("sub-command")
                .Syntactic(" ")
                .Variable("app ID")
                .Syntactic(" ...");

            var commandProject = new CommandArgument(COMMAND_PROJECT, "p", "prj");
            commandProject.Description
                .Text("Manage projects in the Bench environment.");
            commandProject.SyntaxInfo
                .Variable("sub-command")
                .Syntactic(" ")
                .Variable("project name")
                .Syntactic(" ...");

            return new ArgumentParser(parent, Name,
                flagVerbose,
                flagNoAssurance,

                optionLogFile,
                optionBenchRoot,

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

        public override string Name
            => Assembly.GetExecutingAssembly()
                .GetName().Name.ToLowerInvariant();

        public RootCommand()
        {
            ToolName = "Bench CLI";
            ToolVersion = Program.Version();
            RegisterSubCommand(new AppCommand());
            RegisterSubCommand(new ConfigCommand());
            RegisterSubCommand(new DownloadsCommand());
        }

        private static string MyPath()
        {
            return Path.GetDirectoryName(Program.CliExecutable());
        }

        private static string DefaultRootPath()
        {
            var rootPath = Path.GetFullPath(Path.Combine(Path.Combine(MyPath(), ".."), ".."));
            return File.Exists(Path.Combine(rootPath, @"res\apps.md")) ? rootPath : null;
        }

        private string DashboardExecutable()
        {
            if (!BenchTasks.IsDashboardSupported) return null;
            var path = Path.Combine(MyPath(), "BenchDashboard.exe");
            return File.Exists(path) ? path : null;
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

        protected override bool ExecuteUnknownSubCommand(string command, string[] args)
        {

            switch (command)
            {
                case COMMAND_INITIALIZE:
                    return TaskInitialize();
                case COMMAND_SETUP:
                    return TaskAutoSetup();
                case COMMAND_UPDATE_ENV:
                    return TaskUpdateEnvironment();
                case COMMAND_REINSTALL:
                    return TaskReinstallApps();
                case COMMAND_RENEW:
                    return TaskUpgradeApps();
                case COMMAND_UPGRADE:
                    WriteError("This command is not implemented yet.");
                    return false;
                case COMMAND_PROJECT:
                    WriteError("This command is not implemented yet.");
                    return false;

                default:
                    WriteError("Unsupported command: " + command + ".");
                    return false;
            }
        }

        private bool TaskInitialize()
        {
            var cfg = BenchTasks.InitializeSiteConfiguration(RootPath);
            if (cfg == null)
            {
                WriteInfo("Initialization canceled.");
                return false;
            }

            var autoSetup = cfg.GetBooleanValue(PropertyKeys.WizzardStartAutoSetup, true);
            var mgr = new DefaultBenchManager(cfg);
            mgr.Verbose = Verbose;
            cfg = null;

            var success = mgr.SetupRequiredApps();
            if (!success)
            {
                WriteError("Initial app setup failed.");
                return false;
            }

            cfg = BenchTasks.InitializeCustomConfiguration(mgr);
            if (cfg == null)
            {
                WriteInfo("Initialization canceled.");
                return false;
            }
            mgr.Dispose();

            var dashboardPath = DashboardExecutable();
            if (dashboardPath != null)
            {
                var arguments = string.Format("-root \"{0}\"", RootPath);
                if (autoSetup)
                {
                    arguments += " -setup";
                }
                var pi = new ProcessStartInfo()
                {
                    FileName = dashboardPath,
                    Arguments = arguments,
                    UseShellExecute = false
                };
                System.Diagnostics.Process.Start(pi);
                return true;
            }
            else if (autoSetup)
                return TaskAutoSetup();
            else
                return true;
        }

        private bool TaskAutoSetup()
            => RunManagerTask(mgr => mgr.AutoSetup());

        private bool TaskUpdateEnvironment()
            => RunManagerTask(mgr => mgr.UpdateEnvironment());

        private bool TaskReinstallApps()
            => RunManagerTask(mgr => mgr.ReinstallApps());

        private bool TaskUpgradeApps()
            => RunManagerTask(mgr => mgr.UpgradeApps());
    }
}
