﻿using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli
{
    abstract class CommandBase
    {
        private ArgumentParser argParser;

        public ArgumentParser ArgumentParser
            => argParser ?? (argParser = InitializeArgumentParser(Parent?.ArgumentParser));

        protected abstract ArgumentParser InitializeArgumentParser(ArgumentParser parent);

        protected ArgumentParsingResult Arguments { get; set; }

        public abstract string Name { get; }

        private CommandBase parent;

        public CommandBase Parent { get; protected set; }

        public IDictionary<string, CommandBase> SubCommands { get; private set; }

        protected CommandBase()
        {
            SubCommands = new Dictionary<string, CommandBase>();
        }

        protected void RegisterSubCommand(CommandBase subCommand)
        {
            SubCommands[subCommand.Name] = subCommand;
            subCommand.Parent = this;
        }

        #region Bubbeling Properties

        private string toolName;

        public string ToolName
        {
            get { return Parent != null ? Parent.ToolName : toolName; }
            protected set { toolName = value; }
        }

        private string toolVersion;

        public string ToolVersion
        {
            get { return Parent != null ? Parent.ToolVersion : toolVersion; }
            protected set { toolVersion = value; }
        }

        private bool verbose;

        public bool Verbose
        {
            get { return Parent != null ? Parent.Verbose : verbose; }
            protected set { verbose = value; }
        }

        private bool noAssurance;

        public bool NoAssurance
        {
            get { return Parent != null ? Parent.NoAssurance : noAssurance; }
            protected set { noAssurance = value; }
        }

        #endregion

        #region Console Output

        protected void WriteLine(string message)
            => Console.WriteLine(message);

        protected void WriteLine(string format, params object[] args)
            => Console.WriteLine(format, args);

        protected void WriteError(string message)
        {
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] (cli) " + message);
            Console.ForegroundColor = colorBackup;
        }

        protected void WriteError(string format, params object[] args)
            => WriteError(string.Format(format, args));

        protected void WriteInfo(string message)
        {
            if (!Verbose) return;
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[INFO] (cli) " + message);
            Console.ForegroundColor = colorBackup;
        }

        protected void WriteInfo(string format, params object[] args)
            => WriteInfo(string.Format(format, args));

        protected void WriteDetail(string message)
        {
            if (!Verbose) return;
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[VERBOSE] (cli) " + message);
            Console.ForegroundColor = colorBackup;
        }

        protected void WriteDetail(string format, params object[] args)
            => WriteDetail(string.Format(format, args));

        private void Backspace(int l)
        {
            Console.Write("".PadRight(l, (char)0x08));
        }

        protected virtual bool AskForAssurance(string question)
        {
            if (NoAssurance) return true;

            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            var extent = "(y/N)";
            Console.Write(question + " " + extent);
            bool? result = null;
            while (result == null)
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

        #endregion

        #region Help

        private string[] CommandChain(bool withRoot = true)
        {
            var names = new List<string>();
            var cmd = this;
            while (cmd != null && (withRoot || cmd.Parent != null))
            {
                names.Add(cmd.Name);
                cmd = cmd.Parent;
            }
            names.Reverse();
            return names.ToArray();
        }

        private string CommandChain(string separator, bool withRoot = true)
            => string.Join(separator, CommandChain(withRoot));

        protected virtual void PrintHelpHint()
        {
            WriteLine("Use '{0} {1}' to display the help.",
                CommandChain(" "),
                ArgumentParser.MainHelpIndicator);
        }

        protected virtual void PrintInvalidArgumentWarning(string arg)
        {
            WriteError("Invalid Argument: " + arg);
            PrintHelpHint();
        }

        protected virtual void PrintHelp()
        {
            var w = new PlainTextDocumentWriter(Console.Out);
            PrintHelp(w);
        }

        protected virtual void PrintHelp(DocumentWriter w)
        {
            w.Begin(BlockType.Document);
            if (Parent == null)
                w.Title("{0} v{1}", ToolName, ToolVersion);
            else
                w.Title("{0} v{1} - [{2}]", ToolName, ToolVersion, CommandChain(" ", false));
            HelpFormatter.WriteHelp(w, ArgumentParser);
            w.End(BlockType.Document);
        }

        #endregion

        #region Execution

        public virtual bool Process(string[] args)
        {
            WriteDetail("Arguments: {0}", string.Join(" ", args));
            Arguments = ArgumentParser.Parse(args);
            if (Arguments.Type == ArgumentParsingResultType.Help)
            {
                PrintHelp();
                return true;
            }
            if (Arguments.Type == ArgumentParsingResultType.InvalidArgument)
            {
                PrintInvalidArgumentWarning(Arguments.InvalidArgument);
                return false;
            }
            if (!ValidateArguments())
                return false;
            if (Arguments.Type == ArgumentParsingResultType.NoCommand)
            {
                return ExecuteCommand(Arguments.Rest);
            }
            if (Arguments.Type == ArgumentParsingResultType.Command)
            {
                WriteDetail("Command: {0}", Arguments.Command);
                return ExecuteSubCommand(Arguments.Command, Arguments.Rest);
            }

            WriteError("Argument parsing result not supported.");
            return false;
        }

        protected virtual bool ValidateArguments()
        {
            return true;
        }

        protected virtual bool ExecuteSubCommand(string command, string[] args)
        {
            CommandBase cmd;
            if (SubCommands.TryGetValue(command, out cmd))
                return cmd.Process(args);
            else
                return ExecuteUnknownSubCommand(command, args);
        }

        protected virtual bool ExecuteUnknownSubCommand(string command, string[] args)
        {
            WriteError("The sub-command '{0}' is not implemented.", command);
            PrintHelpHint();
            return false;
        }

        protected virtual bool ExecuteCommand(string[] args)
        {
            WriteError("This command has no meaning on its own. Try specify a sub-command.");
            PrintHelpHint();
            return false;
        }

        #endregion
    }
}
