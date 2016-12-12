using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public class ArgumentCompletionConsoleDialog : ConsoleDialog
    {
        private readonly ArgumentParser parser;

        public ArgumentCompletionConsoleDialog(ArgumentParser parser)
        {
            this.parser = parser;
        }

        public ArgumentParsingResult ShowFor(ArgumentParsingResult prelimResult)
        {
            if (prelimResult.Type != ArgumentParsingResultType.MissingArgument &&
                prelimResult.Type != ArgumentParsingResultType.NoCommand)
            {
                throw new InvalidOperationException("The arguments are already satisfying.");
            }

            var helpSelected = false;
            var canceled = false;

            var commandArgs = parser.GetCommands();
            var selectedCommand = prelimResult.Command;
            var isCommandMissing = commandArgs.Length > 0 && selectedCommand == null;

            if (isCommandMissing)
            {
                Open();
                WriteLine();
                WriteLine("Choose one of the following commands:");
                WriteLine();
                var keyChars = new List<char>();
                keyChars.Add(ESC);
                keyChars.Add('?');
                foreach (var cmd in commandArgs)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Write("  {0}  ", cmd.Mnemonic[0]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    WriteLine(cmd.Description.ToString());
                    keyChars.Add(cmd.Mnemonic[0]);
                }
                WriteLine();
                Write("Press a character key to select a command or ESC. ");
                var commandSelection = ReadExpectedChar(keyChars);
                if (commandSelection == '?')
                {
                    helpSelected = true;
                }
                else if (commandSelection == ESC)
                {
                    canceled = true;
                }
                else
                {
                    foreach (var cmd in commandArgs)
                    {
                        if (cmd.Mnemonic[0] == commandSelection)
                        {
                            selectedCommand = cmd.Name;
                            break;
                        }
                    }
                }
                Close();
                if (helpSelected)
                {
                    return new ArgumentParsingResult(parser, ArgumentParsingResultType.Help,
                        null, null, prelimResult.Rest,
                        prelimResult.OptionValues, prelimResult.Flags, prelimResult.PositionalValues);
                }
                if (canceled)
                {
                    return null;
                }
                if (selectedCommand != null)
                {
                    Console.Write("Selected Command: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(selectedCommand);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                }
            }

            var positionalArgs = parser.GetPositionals();
            var missingPositionals = new List<PositionalArgument>();
            foreach (var pArg in positionalArgs)
            {
                if (prelimResult.GetPositionalValue(pArg.Name) == null)
                {
                    var ok = false;
                    while (!ok)
                    {
                        Write("{0}: ", pArg.Name);
                        var v = Console.ReadLine();
                        if (pArg.ValuePredicate == null || pArg.ValuePredicate(v))
                        {
                            ok = true;
                            prelimResult.PositionalValues.Add(pArg.Name, v);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            WriteLine("Invalid value for '{0}'.", pArg.Name);
                        }
                    }
                }
            }

            return new ArgumentParsingResult(parser,
                selectedCommand != null
                    ? ArgumentParsingResultType.Command
                    : ArgumentParsingResultType.NoCommand,
                selectedCommand, null, prelimResult.Rest,
                prelimResult.OptionValues, prelimResult.Flags, prelimResult.PositionalValues)
            { IsCompletedInteractively = true };
        }
    }
}
