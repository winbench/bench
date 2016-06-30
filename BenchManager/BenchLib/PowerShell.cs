using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public static class PowerShell
    {
        public static string Executable =>
            Environment.ExpandEnvironmentVariables(@"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe");

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
