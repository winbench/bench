using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    abstract class Controller
    {
        protected ArgumentParsingResult Arguments { get; set; }

        public bool Verbose { get; protected set; }

        protected void WriteError(string message)
        {
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = colorBackup;
        }

        protected void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        protected void WriteInfo(string message)
        {
            if (!Verbose) return;
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = colorBackup;
        }

        protected void WriteDetail(string message)
        {
            if (!Verbose) return;
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(message);
            Console.ForegroundColor = colorBackup;
        }

        public bool Execute()
        {
            switch (Arguments.Type)
            {
                case ArgumentParsingResultType.Help:
                    PrintHelp();
                    return true;
                case ArgumentParsingResultType.InvalidArgument:
                    PrintInvalidArgumentWarning(Arguments.InvalidArgument);
                    return false;
                case ArgumentParsingResultType.Command:
                    return ExecuteCommand(Arguments.Command, Arguments.Rest);
                case ArgumentParsingResultType.NoCommand:
                    PrintNoCommandWarning();
                    return false;
                default:
                    WriteError("Argument parsing result not supported.");
                    return false;
            }
        }

        protected virtual void PrintHelpHint()
        {
            WriteLine("Use 'bench -?' to display the help.");
        }

        private void PrintInvalidArgumentWarning(string arg)
        {
            WriteError("Invalid Argument: " + arg);
            PrintHelpHint();
        }

        private void PrintNoCommandWarning()
        {
            WriteError("No command given.");
            PrintHelpHint();
        }

        protected abstract void PrintHelp();

        protected abstract bool ExecuteCommand(string command, string[] args);
    }
}
