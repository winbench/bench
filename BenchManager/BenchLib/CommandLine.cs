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
        /// Given a number of strings, containing batch style placeholders for
        /// environment variables and numbered parameters, the placeholders are 
        /// replaced by the referred values.
        /// </summary>
        /// <example>
        /// Given the environment variable <c>MY_DIR</c> with a value of <c>C:\my-dir</c>
        /// and an <paramref name="args"/> array with the following values <c>["a", "b"]</c>,
        /// an <paramref name="argumentList"/> with <c>["-out", "%MY_DIR%\result", "-msg", "%2 + %1 = ?"]</c>
        /// is substituted into the following array:
        /// <c>["-out", "C:\my-dir\result", "-msg", "b + a = ?"]</c>.
        /// </example>
        /// <param name="argumentList">
        /// A number of strings possibly with placeholders for environment
        /// varibales and numbered parameters.
        /// Environment variables are written as <c>%NAME%</c>, and numbered parameters
        /// are written as <c>%x</c>, with <c>x</c> beeing a digit from <c>0</c> to <c>9</c>.
        /// </param>
        /// <param name="args">An array with parameters.</param>
        /// <returns>An array with the substituted strings.</returns>
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
