using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    static class HelpFormatter
    {
        public const string INDENT = "  ";
        public const string INDENT_2 = "      ";

        private static string FormatFlag(Argument a)
        {
            return "--" + a.Name
                + (a.Aliases.Length > 0
                    ? " | --" + string.Join(" | --", a.Aliases)
                    : "")
                + " | -" + a.Mnemonic;
        }

        private static string FormatOption(Argument a)
        {
            return FormatFlag(a) + " <value>";
        }

        private static string FormatCommand(Argument a)
        {
            return a.Name
                + (a.Aliases.Length > 0
                    ? ", " + string.Join(", ", a.Aliases)
                    : "")
                + ", " + a.Mnemonic;
        }

        public static string GenerateHelp(ArgumentParser parser)
        {
            var sb = new StringBuilder();
            sb.AppendLine("  /? | -? | -h | --help");
            sb.AppendLine("    Display the help.");
            var flags = parser.GetFlags();
            if (flags.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Flags:");
                foreach (FlagArgument flag in flags)
                {
                    sb.AppendLine(INDENT + FormatFlag(flag));
                    if (flag.Description != null)
                    {
                        sb.AppendLine(INDENT_2 + flag.Description);
                    }
                }
            }
            var options = parser.GetOptions();
            if (options.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Options:");
                foreach (OptionArgument option in options)
                {
                    sb.AppendLine();
                    sb.AppendLine(INDENT + FormatOption(option));
                    if (option.Description != null)
                    {
                        sb.AppendLine(INDENT_2 + option.Description); 
                    }
                    if (option.PossibleValueInfo != null || option.DefaultValueInfo != null)
                    {
                        sb.AppendLine();
                        if (option.PossibleValueInfo != null)
                        {
                            sb.AppendLine(INDENT_2 + "Expected:  " + option.PossibleValueInfo);
                        }
                        if (option.DefaultValueInfo != null)
                        {
                            sb.AppendLine(INDENT_2 + "Default:   " + option.DefaultValueInfo); 
                        }
                    }
                }
            }
            var commands = parser.GetCommands();
            if (commands.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Commands:");
                foreach (CommandArgument cmd in parser.GetCommands())
                {
                    sb.AppendLine();
                    sb.AppendLine(INDENT + FormatCommand(cmd));
                    sb.AppendLine(INDENT_2 + cmd.Description);
                    if (cmd.SyntaxInfo != null)
                    {
                        sb.AppendLine();
                        sb.AppendLine(INDENT_2 + "Syntax: " + cmd.SyntaxInfo);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
