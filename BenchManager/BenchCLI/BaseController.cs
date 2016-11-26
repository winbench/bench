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

        public bool NoAssurance { get; protected set; }

        protected void WriteError(string message)
        {
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] (cli) " + message);
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
            Console.WriteLine("[INFO] (cli) " + message);
            Console.ForegroundColor = colorBackup;
        }

        protected void WriteDetail(string message)
        {
            if (!Verbose) return;
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[VERBOSE] (cli) " + message);
            Console.ForegroundColor = colorBackup;
        }

        private void Backspace(int l)
        {
            Console.Write("".PadRight(l, (char)0x08));
        }

        protected bool AskForAssurance(string question)
        {
            if (NoAssurance) return true;

            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            var extent = "(y/N)";
            Console.Write(question + " " + extent);
            bool? result = null;
            while(result == null)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    result = false;
                else if (key.Key == ConsoleKey.N)
                    result = false;
                else if (key.Key == ConsoleKey.Y)
                    result = true;
            }
            Backspace(extent.Length);
            Console.Write("<- ");
            if (result.Value)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Yes");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("No");
            }
            Console.ForegroundColor = colorBackup;
            Console.WriteLine();
            return result.Value;
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
