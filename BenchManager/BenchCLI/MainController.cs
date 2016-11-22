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
            return !ContainsOneOfChars(v, Path.GetInvalidFileNameChars())
                && !ContainsOneOfChars(v, Path.GetInvalidPathChars());
        }

        private static ArgumentParser parser
            = new ArgumentParser(
                new OptionArgument("verbosity", "v",
                    "Controls the verbosity of the output.",
                    "s(ilent), v(erbose", "silent",
                    v => !string.IsNullOrEmpty(v) &&
                        ("silent".StartsWith(v, StringComparison.OrdinalIgnoreCase) ||
                         "verbose".StartsWith(v, StringComparison.OrdinalIgnoreCase)),
                    "verb"),
                new OptionArgument("logfile", "l",
                    "Specifies a custom location for the log file.",
                    "A path to the log file.",
                    "Auto generated filename in <bench root>\\log\\.",
                    IsValidPath,
                    "log"),
                new OptionArgument("root", "r",
                    "Specifies the root directory of the Bench environment.",
                    "A path to a valid Bench root directory.",
                    "The root directory of the Bench environment, this Bench CLI belongs to.",
                    v => IsValidPath(v) && Directory.Exists(v),
                    "base"),

                new Argument(ArgumentType.Command, "initialize", "i",
                    "Initialize the Bench configuration and start the setup process."),
                new Argument(ArgumentType.Command, "setup", "s",
                    "Run the auto-setup for the active Bench apps."),
                new Argument(ArgumentType.Command, "update-env", "e",
                    "Update the paths in the Bench environment."),
                new Argument(ArgumentType.Command, "reinstall", "r",
                    "Remove all installed apps, then install all active apps."),
                new Argument(ArgumentType.Command, "renew", "n",
                    "Redownload all app resources, remove all installed apps, then install all active apps."),
                new Argument(ArgumentType.Command, "upgrade", "u",
                    "Download the latest Bench release and run the auto-setup."),

                new Argument(ArgumentType.Command, "config", "c",
                    "Read or write values from the user configuration.",
                    "cfg"),
                new Argument(ArgumentType.Command, "downloads", "d",
                    "Manage the app resource cache.",
                    "cache", "dl"),
                new Argument(ArgumentType.Command, "app", "a",
                    "Manage individual apps."),
                new Argument(ArgumentType.Command, "project", "p",
                    "Manage projects in the Bench environment.",
                    "prj"));

        public MainController(string[] args)
        {
            Arguments = parser.Parse(args);
            Verbose = GetVerboseValue(Arguments);
        }

        private static bool GetVerboseValue(ArgumentParsingResult arguments)
        {
            return "verbose".StartsWith(
                arguments.GetOptionValue("verbosity", "silent"),
                StringComparison.OrdinalIgnoreCase);
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
                var p = Arguments.GetOptionValue("root", DefaultRootPath());
                return Path.IsPathRooted(p)
                    ? p : Path.Combine(Environment.CurrentDirectory, p);
            }
        }

        public string LogFilePath
        {
            get
            {
                var p = Arguments.GetOptionValue("logfile");
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
            Console.WriteLine("  bench (/? | -? | -h | --help)");
            Console.WriteLine("  bench (<flag> | <option>)*");
            Console.WriteLine("  bench (<flag> | <option>)* <command>");
            Console.WriteLine("  bench <command> (/? | -? | -h | --help)");
            Console.WriteLine("  bench (<flag> | <option>)* <command> (<flag> | <option>)* arg*");
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
                case "initialize":
                    return TaskInitialize();
                case "setup":
                    return TaskAutoSetup();
                case "update-env":
                    return TaskUpdateEnvironment();
                case "reinstall":
                    return TaskReinstallApps();
                case "renew":
                    return TaskUpgradeApps();
                case "upgrade":
                    WriteError("This command is not implemented yet.");
                    return false;

                case "config":
                    WriteError("This command is not implemented yet.");
                    return false;
                case "downloads":
                    WriteError("This command is not implemented yet.");
                    return false;
                case "app":
                    return new AppController(this, args).Execute();
                case "project":
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
