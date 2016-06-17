using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var rootPath = GetBenchRoot(args);
            if (rootPath == null)
            {
                MessageBox.Show(
                    "Initialization failed. Could not determine the root path of Bench."
                    + Environment.NewLine
                    + "Use the -root switch to provide the Bench root path.",
                    "Bench",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return 1;
            }
            if (!Directory.Exists(rootPath))
            {
                MessageBox.Show(
                    "Initialization failed. Root path of bench not found."
                    + Environment.NewLine + Environment.NewLine
                    + rootPath,
                    "Bench",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return 1;
            }
            Core = new Core(rootPath);
            Core.SetupOnStartup = IsImmediateSetupRequested(args);

            var mainForm = new MainForm(Core);
            Core.GuiContext = mainForm;
            var ui = Core.UI as WinFormsUserInterface;
            if (ui != null) ui.ParentWindow = mainForm;

            Application.ApplicationExit += ApplicationExitHandler;
            Application.Run(mainForm);

            Core.Dispose();
            return 0;
        }

        private static void ApplicationExitHandler(object sender, EventArgs e)
        {
            var cancelation = Core.Cancelation;
            if (cancelation != null)
            {
                cancelation.Cancel();
            }
        }

        private static string GetBenchRoot(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "-root")
                {
                    return args[i + 1];
                }
            }
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var codeBase = new Uri(assemblyName.CodeBase).LocalPath;
            var rootPath = Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(codeBase), ".."), ".."));
            return File.Exists(Path.Combine(rootPath, @"res\apps.md")) ? rootPath : null;
        }

        private static bool IsImmediateSetupRequested(string[] args)
        {
            foreach(var arg in args)
            {
                if (arg == "-setup") return true;
            }
            return false;
        }

        public static Core Core { get; private set; }
    }
}
