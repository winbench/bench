using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    /// <summary>
    /// This static class provides a number of methods to deal with command line arguments.
    /// </summary>
    public static class CommandLine
    {
        /// <summary>
        /// Given an array of strings, containing batch style placeholders for
        /// environment variables and numbered parameters, the placeholders are 
        /// replaced by the referred parameter values.
        /// </summary>
        /// <example>
        /// Given the environment variable <c>MY_DIR</c> with a value of <c>C:\my-dir</c>,
        /// and an array with <c>["a", "b"]</c> for <paramref name="parameters"/>,
        /// the array with <c>["-out", "%MY_DIR%\result", "-msg", "%2 + %1 = ?"]</c>
        /// for <paramref name="values"/> is substituted into the following array:
        /// <c>["-out", "C:\my-dir\result", "-msg", "b + a = ?"]</c>.
        /// </example>
        /// <param name="values">
        /// A number of strings possibly with placeholders for environment
        /// varibales and numbered parameters.
        /// Environment variables are written as <c>%NAME%</c>, and numbered parameters
        /// are written as <c>%x</c>, with <c>x</c> beeing a digit from <c>0</c> to <c>9</c>.
        /// </param>
        /// <param name="parameters">An array with ordered parameter strings.</param>
        /// <returns>An array with the substituted strings.</returns>
        public static string SubstituteArgumentList(string[] values, string[] parameters)
        {
            var result = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                var arg = SubstituteArgument(values[i], parameters);
                if (arg == "%*")
                {
                    result.AddRange(parameters);
                }
                else
                {
                    result.Add(arg);
                }
            }
            return FormatArgumentList(result.ToArray());
        }


        public static string SubstituteArgument(string argument, string[] args)
        {
            argument = Environment.ExpandEnvironmentVariables(argument);
            for (int i = 0; i < 10; i++)
            {
                var v = args.Length > i ? args[i] : "";
                argument = argument.Replace("%" + i, v);
            }
            return argument;
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
