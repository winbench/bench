using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mastersign.Bench.Cli
{
    class MainController : Controller
    {
        private static bool ContainsOneOfChars(string v, char[] chars)
        {
            foreach (var c in chars)
            {
                if (v.Contains(new string(new[] { c }))) return true;
            }
            return false;
        }

        private static bool IsValidPath(string v)
        {
            return !ContainsOneOfChars(v, Path.GetInvalidPathChars());
        }

        private const string FLAG_VERBOSE = "verbose";
        private const string OPTION_LOGFILE = "logfile";
        private const string OPTION_BENCH_ROOT = "root";
        private const string COMMAND_INITIALIZE = "initialize";
        private const string COMMAND_SETUP = "setup";
        private const string COMMAND_UPDATE_ENV = "update-env";
        private const string COMMAND_REINSTALL = "reinstall";
        private const string COMMAND_RENEW = "renew";
        private const string COMMAND_UPGRADE = "upgrade";
        private const string COMMAND_CONFIG = "config";
        private const string COMMAND_DOWNLOADS = "downloads";
        private const string COMMAND_APP = "app";
        private const string COMMAND_PROJECT = "project";

        private static ArgumentParser parser
            = new ArgumentParser(
                new FlagArgument(FLAG_VERBOSE, "v",
                    "Activates verbose output.",
                    "verb"),

                new OptionArgument(OPTION_LOGFILE, "l",
                    "Specifies a custom location for the log file.",
                    "A path to the log file.",
                    "Auto generated filename in <bench root>\\log\\.",
                    IsValidPath,
                    "log"),
                new OptionArgument(OPTION_BENCH_ROOT, "r",
                    "Specifies the root directory of the Bench environment.",
                    "A path to a valid Bench root directory.",
                    "The root directory of the Bench environment, this Bench CLI belongs to.",
                    v => IsValidPath(v) && Directory.Exists(v),
                    "base"),

                new CommandArgument(COMMAND_INITIALIZE, "i",
                    "Initialize the Bench configuration and start the setup process."),
                new CommandArgument(COMMAND_SETUP, "s",
                    "Run the auto-setup for the active Bench apps."),
                new CommandArgument(COMMAND_UPDATE_ENV, "e",
                    "Update the paths in the Bench environment."),
                new CommandArgument(COMMAND_REINSTALL, "r",
                    "Remove all installed apps, then install all active apps."),
                new CommandArgument(COMMAND_RENEW, "n",
                    "Redownload all app resources, remove all installed apps, then install all active apps."),
                new CommandArgument(COMMAND_UPGRADE, "u",
                    "Download and extract the latest Bench release, then run the auto-setup."),

                new CommandArgument(COMMAND_CONFIG, "c",
                    "Read or write values from the user configuration.",
                    "<sub-command> [<property name>]",
                    "cfg"),
                new CommandArgument(COMMAND_DOWNLOADS, "d",
                    "Manage the app resource cache.",
                    "<sub-command>",
                    "cache", "dl"),
                new CommandArgument(COMMAND_APP, "a",
                    "Manage individual apps.",
                    "<sub-command> <app ID> [args*]"),
                new CommandArgument(COMMAND_PROJECT, "p",
                    "Manage projects in the Bench environment.",
                    "<sub-command> [<project name>] [args*]",
                    "prj"));

        public MainController(string[] args)
        {
            Arguments = parser.Parse(args);
            Verbose = GetVerboseValue(Arguments);
        }

        private static bool GetVerboseValue(ArgumentParsingResult arguments)
        {
            return arguments.GetFlag(FLAG_VERBOSE);
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

        public string RootPath
        {
            get
            {
                var p = Arguments.GetOptionValue(OPTION_BENCH_ROOT, DefaultRootPath());
                return Path.IsPathRooted(p)
                    ? p : Path.Combine(Environment.CurrentDirectory, p);
            }
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

        protected override void PrintHelp()
        {
            Console.WriteLine("Bench CLI v" + Program.Version());
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine(HelpFormatter.INDENT + "bench (/? | -? | -h | --help)");
            Console.WriteLine(HelpFormatter.INDENT + "bench (<flag> | <option>)*");
            Console.WriteLine(HelpFormatter.INDENT + "bench (<flag> | <option>)* <command>");
            Console.WriteLine(HelpFormatter.INDENT + "bench <command> (/? | -? | -h | --help)");
            Console.WriteLine(HelpFormatter.INDENT + "bench (<flag> | <option>)* <command> (<flag> | <option>)* arg*");
            Console.WriteLine(HelpFormatter.GenerateHelp(parser));
        }

        protected override bool ExecuteCommand(string command, string[] args)
        {
            WriteDetail("Bench CLI: " + Program.CliExecutable());
            if (BenchTasks.IsDashboardSupported)
            {
                WriteDetail("Bench Dashboard: " + (DashboardExecutable() ?? "Not found."));
            }
            else
            {
                WriteDetail("Bench Dashboard: Not Supported. Microsoft .NET Framework 4.5 not installed.");
            }
            WriteDetail("Bench Root: " + RootPath);
            WriteDetail("Log File: " + (LogFilePath ?? "automatic"));
            WriteDetail("Command: " + command);
            WriteDetail("");
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

                case COMMAND_CONFIG:
                    WriteError("This command is not implemented yet.");
                    return false;
                case COMMAND_DOWNLOADS:
                    WriteError("This command is not implemented yet.");
                    return false;
                case COMMAND_APP:
                    return new AppController(this, args).Execute();
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
                Process.Start(pi);
                return true;
            }
            else if (autoSetup)
            {
                return TaskAutoSetup();
            }
            else
            {
                return true;
            }
        }

        public BenchConfiguration LoadConfiguration()
        {
            return new BenchConfiguration(RootPath, true, true, true);
        }

        public DefaultBenchManager CreateManager()
        {
            return new DefaultBenchManager(LoadConfiguration())
            {
                Verbose = Verbose
            };
        }

        private bool TaskAutoSetup()
        {
            using (var mgr = CreateManager())
            {
                return mgr.AutoSetup();
            }
        }

        private bool TaskUpdateEnvironment()
        {
            var cfg = new BenchConfiguration(RootPath, true, true, true);
            using (var mgr = new DefaultBenchManager(cfg))
            {
                mgr.Verbose = Verbose;
                return mgr.UpdateEnvironment();
            }
        }

        private bool TaskReinstallApps()
        {
            var cfg = new BenchConfiguration(RootPath, true, true, true);
            using (var mgr = new DefaultBenchManager(cfg))
            {
                mgr.Verbose = Verbose;
                return mgr.ReinstallApps();
            }
        }

        private bool TaskUpgradeApps()
        {
            var cfg = new BenchConfiguration(RootPath, true, true, true);
            using (var mgr = new DefaultBenchManager(cfg))
            {
                mgr.Verbose = Verbose;
                return mgr.UpgradeApps();
            }
        }
    }
}
