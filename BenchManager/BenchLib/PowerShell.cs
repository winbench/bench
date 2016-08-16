using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// A static class, containing some helper properties and methods,
    /// for dealing with the PowerShell.
    /// </summary>
    public static class PowerShell
    {
        /// <summary>
        /// An absolute path to the PowerShell executable.
        /// </summary>
        public static string Executable =>
            Environment.ExpandEnvironmentVariables(@"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe");

        /// <summary>
        /// Formats the given strings as a string array in PowerShell syntax.
        /// </summary>
        /// <param name="args">The strings, to be formatted as a PowerShell array.</param>
        /// <returns>A string with the formatted values.</returns>
        /// <example>
        /// The call <c>PowerShell.FormatString("ab", "cd ef", gh")</c> yields the
        /// result string <c>"@(\"ab\", \"cd ef\", \"gh\")"</c>.
        /// </example>
        public static string FormatStringList(params string[] args)
        {
            var list = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                list[i] = CommandLine.EscapeArgument(args[i], true);
            }
            return "@(" + string.Join(", ", list) + ")";
        }
    }
}
