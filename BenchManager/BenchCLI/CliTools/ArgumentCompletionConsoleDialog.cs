using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.CliTools
{
    public class ArgumentCompletionConsoleDialog : ConsoleDialog
    {
        private const char HELP_MNEMONIC = '?';
        private const char FLAG_OR_OPTION_MNEMONIC = '-';

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

            var flags = prelimResult.Flags;
            var optionValues = prelimResult.OptionValues;
            var positionalValues = prelimResult.PositionalValues;

            var positionalArgs = parser.GetPositionals();
            var missingPositionalArgs = new List<PositionalArgument>();
            foreach (var pArg in positionalArgs)
            {
                if (prelimResult.GetPositionalValue(pArg.Name) == null)
                {
                    missingPositionalArgs.Add(pArg);
                }
            }
            var hasMissingPositionalArguments = missingPositionalArgs.Count > 0;

            var selectedCommand = prelimResult.Command;
            var isCommandMissing = parser.GetCommands().Length > 0 && selectedCommand == null;

            if (isCommandMissing)
            {
                CommandMenuResult result;
                do
                {
                    result = ShowCommandMenu(ref selectedCommand);
                    switch (result)
                    {
                        case CommandMenuResult.Escape:
                            return null;
                        case CommandMenuResult.Help:
                            return prelimResult.DeriveHelp();
                        case CommandMenuResult.Command:
                            if (selectedCommand != null)
                            {
                                Console.Write("Selected Command: ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(selectedCommand);
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine();
                            }
                            break;
                        case CommandMenuResult.FlagOrOption:
                            Argument arg;
                            var result2 = ShowFlagAndOptionMenu(flags, optionValues, out arg, MenuFollowUp.Return);
                            switch (result2)
                            {
                                case FlagAndOptionMenuResult.Exit:
                                    break;
                                case FlagAndOptionMenuResult.Help:
                                    return prelimResult.DeriveHelp();
                                case FlagAndOptionMenuResult.FlagOrOption:
                                    PrintFlagOrOptionCompletion(flags, optionValues, arg);
                                    break;
                                default:
                                    throw new NotSupportedException();
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                } while (result == CommandMenuResult.FlagOrOption);
            }
            else if (hasMissingPositionalArguments &&
                (parser.GetFlags().Length > 0 || parser.GetOptions().Length > 0))
            {
                Argument arg;
                FlagAndOptionMenuResult result;
                do
                {
                    result = ShowFlagAndOptionMenu(flags, optionValues, out arg, MenuFollowUp.Proceed);
                    switch (result)
                    {
                        case FlagAndOptionMenuResult.Exit:
                            break;
                        case FlagAndOptionMenuResult.Help:
                            return prelimResult.DeriveHelp();
                        case FlagAndOptionMenuResult.FlagOrOption:
                            PrintFlagOrOptionCompletion(flags, optionValues, arg);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                } while (result == FlagAndOptionMenuResult.FlagOrOption);
            }

            if (hasMissingPositionalArguments)
            {
                foreach (var arg in missingPositionalArgs)
                {
                    positionalValues[arg.Name] = ReadArgumentValue(arg.Name,
                        arg.ValuePredicate, arg.Description, arg.PossibleValueInfo);
                }
            }

            return prelimResult.DeriveInteractivelyCompleted(selectedCommand);
        }

        private void PrintFlagOrOptionCompletion(IDictionary<string, bool> flags, IDictionary<string, string> optionValues, Argument arg)
        {
            if (arg is FlagArgument)
            {
                Write("Flag {0}: ", arg.Name);
                Console.ForegroundColor = ConsoleColor.White;
                WriteLine(flags.ContainsKey(arg.Name) && flags[arg.Name] ? "active" : "inactive");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (arg is OptionArgument)
            {
                Write("Option {0}: ", arg.Name);
                Console.ForegroundColor = ConsoleColor.White;
                WriteLine(optionValues[arg.Name]);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private enum CommandMenuResult
        {
            Escape,
            Help,
            Command,
            FlagOrOption
        }

        private CommandMenuResult ShowCommandMenu(ref string selectedCommand)
        {
            var commandArgs = parser.GetCommands();
            SortArgumentsByMnemonic(commandArgs);
            var result = CommandMenuResult.Command;
            Open();
            WriteLine();
            WriteLine("Choose one of the following commands:");
            WriteLine();
            var keyChars = new List<char>();
            keyChars.Add(ESC);
            keyChars.Add(HELP_MNEMONIC);
            WriteMenuItem(HELP_MNEMONIC, "Display the help.");
            if (parser.GetFlags().Length > 0 || parser.GetOptions().Length > 0)
            {
                keyChars.Add(FLAG_OR_OPTION_MNEMONIC);
                WriteMenuItem(FLAG_OR_OPTION_MNEMONIC, "Specify a flag or an option value.");
            }
            foreach (var arg in commandArgs)
            {
                WriteMenuItem(arg.Mnemonic[0], arg.Description.ToString());
                keyChars.Add(arg.Mnemonic[0]);
            }
            WriteLine();
            Write("Press a character key to choose a menu item or ESC to quit. ");
            var selectedMnemonic = ReadExpectedChar(keyChars);
            Close();
            if (selectedMnemonic == ESC)
            {
                result = CommandMenuResult.Escape;
            }
            else if (selectedMnemonic == HELP_MNEMONIC)
            {
                result = CommandMenuResult.Help;
            }
            else if (selectedMnemonic == FLAG_OR_OPTION_MNEMONIC)
            {
                result = CommandMenuResult.FlagOrOption;
            }
            else
            {
                foreach (var cmd in commandArgs)
                {
                    if (cmd.Mnemonic[0] == selectedMnemonic)
                    {
                        selectedCommand = cmd.Name;
                        break;
                    }
                }
            }
            return result;
        }

        private enum FlagAndOptionMenuResult
        {
            Exit,
            Help,
            FlagOrOption
        }

        private enum MenuFollowUp
        {
            Return,
            Proceed
        }

        private FlagAndOptionMenuResult ShowFlagAndOptionMenu(
            IDictionary<string, bool> flags, IDictionary<string, string> optionValues,
            out Argument selectedArgument, MenuFollowUp followUp)
        {
            var flagArgs = parser.GetFlags();
            SortArgumentsByMnemonic(flagArgs);
            var optionArgs = parser.GetOptions();
            SortArgumentsByMnemonic(optionArgs);
            var hasFlags = flagArgs.Length > 0;
            var hasOptions = optionArgs.Length > 0;
            var result = FlagAndOptionMenuResult.FlagOrOption;
            Open();
            WriteLine();
            if (hasFlags && hasOptions)
                WriteLine("Choose one of the following flags or options:");
            else if (hasFlags)
                WriteLine("Choose one of the following flags:");
            else if (hasOptions)
                WriteLine("Choose one of the following options:");
            WriteLine();
            var keyChars = new List<char>();
            if (followUp == MenuFollowUp.Return)
                keyChars.Add(ESC);
            else
                keyChars.Add(ENTER);
            keyChars.Add(HELP_MNEMONIC);
            if (hasFlags)
            {
                WriteLine("Flags");
                foreach (var arg in flagArgs)
                {
                    WriteMenuItem(arg.Mnemonic[0], arg.Description.ToString());
                    keyChars.Add(arg.Mnemonic[0]);
                }
            }
            if (hasOptions)
            {
                WriteLine("Options");
                foreach (var arg in optionArgs)
                {
                    WriteMenuItem(arg.Mnemonic[0], arg.Description.ToString());
                    keyChars.Add(arg.Mnemonic[0]);
                }
            }
            WriteLine();
            if (followUp == MenuFollowUp.Return)
                Write("Press a character key to choose a menu item or ESC to go back. ");
            else
                Write("Press a character key to choose a menu item or ENTER to proceed. ");
            var selectedMnemonic = ReadExpectedChar(keyChars);
            Close();
            selectedArgument = null;
            if (selectedMnemonic == ESC || selectedMnemonic == ENTER)
            {
                result = FlagAndOptionMenuResult.Exit;
            }
            else if (selectedMnemonic == HELP_MNEMONIC)
            {
                result = FlagAndOptionMenuResult.Help;
            }
            else
            {
                foreach (var arg in flagArgs)
                {
                    if (arg.Mnemonic[0] == selectedMnemonic)
                    {
                        selectedArgument = arg;
                        if (flags.ContainsKey(arg.Name) && flags[arg.Name])
                            flags.Remove(arg.Name);
                        else
                            flags[arg.Name] = true;
                        return result;
                    }
                }
                foreach (var arg in optionArgs)
                {
                    if (arg.Mnemonic[0] == selectedMnemonic)
                    {
                        selectedArgument = arg;
                        optionValues[arg.Name] = ReadArgumentValue(arg.Name,
                            arg.ValuePredicate, arg.Description, arg.PossibleValueInfo);
                        return result;
                    }
                }
            }
            return result;
        }

        private string ReadArgumentValue(string name, ArgumentValuePredicate predicate,
            Document description, Document possibleValueInfo)
        {
            string result = null;
            var valid = true;
            do
            {
                Open();
                WriteLine();
                WriteLine("Enter value for {0}", name);
                WriteLine();
                WriteLine("Description: " + description.ToString());
                if (!valid)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    WriteLine("The given value for '{0}' is invalid, try again.", name);
                    WriteLine("Expected: " + possibleValueInfo.ToString());
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    WriteLine("Expected: " + possibleValueInfo.ToString());
                }
                WriteLine();
                Write("Value for {0}: ", name);
                result = ReadLine();
                Close();
                if (predicate != null)
                {
                    valid = predicate(result);
                }
            } while (!valid);
            return result;
        }

        private static void SortArgumentsByMnemonic<T>(T[] args)
            where T : NamedArgument
        {
            Array.Sort(args, (c1, c2) =>
            {
                if (c1.Mnemonic == null && c2.Mnemonic == null) return 0;
                if (c1.Mnemonic == null) return -1;
                if (c2.Mnemonic == null) return 1;
                return c1.Mnemonic.CompareTo(c2.Mnemonic);
            });
        }

        private void WriteMenuItem(char mnemonic, string description)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Write("  {0}  ", mnemonic);
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteLine(description);
        }
    }
}
