using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    static class HelpFormatter
    {
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
                foreach (var flag in flags)
                {
                    sb.AppendLine("  " + FormatFlag(flag));
                    sb.AppendLine("    " + flag.Description);
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
                    sb.AppendLine(FormatOption(option));
                    sb.AppendLine("    " + option.Description);
                    sb.AppendLine();
                    sb.AppendLine("    Expected:  " + option.PossibleValueInfo);
                    sb.AppendLine("    Default:   " + option.DefaultValueInfo);
                }
            }
            var commands = parser.GetCommands();
            if (commands.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Commands:");
                foreach (var cmd in parser.GetCommands())
                {
                    sb.AppendLine();
                    sb.AppendLine(FormatCommand(cmd));
                    sb.AppendLine("    " + cmd.Description);
                }
            }

            return sb.ToString();
        }
    }
}
