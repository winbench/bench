using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli
{
    abstract class BaseController
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

        protected void PrintHelpHint()
        {
            var commandChain = string.Join(" ",
                HelpFormatter.GetCommandChain(Arguments.Parser));
            WriteLine("Use '" + commandChain + " "
                + ArgumentParser.MainHelpIndicator + "' to display the help.");
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

        private void PrintHelp()
        {
            var w = new PlainTextDocumentWriter(Console.Out);
            PrintHelp(w);
        }

        protected abstract void PrintHelp(DocumentWriter w);

        protected abstract bool ExecuteCommand(string command, string[] args);
    }
}
