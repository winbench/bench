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

        /// <summary>
        /// Given a string, containing batch style placeholders for
        /// environment variables and numbered parameters, the placeholders are 
        /// replaced by the referred parameter values.
        /// </summary>
        /// <example>
        /// Given the environment variable <c>MY_DIR</c> with a value of <c>C:\my-dir</c>,
        /// and an array with <c>["a", "b"]</c> for <paramref name="parameters"/>,
        /// a string with <c>"-out %MY_DIR%\%1-%2.txt"</c>
        /// for <paramref name="value"/> is substituted into the following result:
        /// <c>"-out C:\my-dir\a-b.txt"</c>.
        /// </example>
        /// <param name="value">
        /// A string possibly with placeholders for environment varibales and numbered parameters.
        /// Environment variables are written as <c>%NAME%</c>, and numbered parameters
        /// are written as <c>%x</c>, with <c>x</c> beeing a digit from <c>0</c> to <c>9</c>.
        /// </param>
        /// <param name="parameters">An array with ordered parameter strings.</param>
        /// <returns>An array with the substituted strings.</returns>
        public static string SubstituteArgument(string value, string[] parameters)
        {
            value = Environment.ExpandEnvironmentVariables(value);
            for (int i = 0; i < 10; i++)
            {
                var v = parameters.Length > i ? parameters[i] : "";
                value = value.Replace("%" + i, v);
            }
            return value;
        }

        // http://www.windowsinspired.com/understanding-the-command-line-string-and-arguments-received-by-a-windows-program/

        /// <summary>
        /// <para>
        /// Creates a string with mutlitple command line arguments,
        /// compatible with the Windows CMD interpreter.
        /// </para>
        /// <para>
        /// All characters of the given strings are preserved and if necessary escaped.
        /// </para>
        /// </summary>
        /// <param name="args">Multiple arguments.</param>
        /// <returns>A command line argument string, containing all given arguments.</returns>
        public static string FormatArgumentList(params string[] args)
        {
            var list = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                list[i] = EscapeArgument(args[i]);
            }
            return string.Join(" ", list);
        }

        /// <summary>
        /// Escapes an argument string for the Windows CMD interpreter.
        /// If the string contains special cahracters or white space,
        /// it is quoted.
        /// </summary>
        /// <remarks>
        /// Usually you will use <see cref="FormatArgumentList(string[])"/> 
        /// instead of calling this method directly.</remarks>
        /// <param name="arg">The argument string.</param>
        /// <param name="alwaysQuote"><c>true</c> if the string will be quoted
        /// regardless of quoting is necessary; otherwise <c>false</c>.</param>
        /// <returns>A string which can be safely passed to the CMD interpreter as an argument.</returns>
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
