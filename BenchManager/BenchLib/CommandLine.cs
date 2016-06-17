using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public static class CommandLine
    {
        public static string SubstituteArgumentList(string[] argumentList, string[] args)
        {
            var result = new List<string>();
            for (int i = 0; i < argumentList.Length; i++)
            {
                var arg = SubstituteArgument(argumentList[i], args);
                if (arg == "%*")
                {
                    result.AddRange(args);
                }
                else
                {
                    result.Add(arg);
                }
            }
            return FormatArgumentList(result.ToArray());
        }

        public static string SubstituteArgument(string arg, string[] args)
        {
            arg = Environment.ExpandEnvironmentVariables(arg);
            for (int i = 0; i < 10; i++)
            {
                var v = args.Length > i ? args[i] : "";
                arg = arg.Replace("%" + i, v);
            }
            return arg;
        }

        // http://www.windowsinspired.com/understanding-the-command-line-string-and-arguments-received-by-a-windows-program/

        public static string FormatArgumentList(params string[] args)
        {
            var list = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                list[i] = EscapeArgument(args[i]);
            }
            return string.Join(" ", list);
        }

        public static string EscapeArgument(string arg, bool alwaysQuote = false)
        {
            var s = Regex.Replace(arg.Trim('"'), @"(\\*)" + "\"", @"$1$1\" + "\"");
            s = Regex.Replace(s, @"(\\+)$", @"$1$1");
            var quote = alwaysQuote || Regex.IsMatch(s, @"\s");
            if (quote) s = "\"" + s + "\"";
            return s;
        }
    }
}
