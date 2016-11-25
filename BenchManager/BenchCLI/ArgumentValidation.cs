using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench.Cli
{
    static class ArgumentValidation
    {
        public static bool ContainsOneOfChars(string v, char[] chars)
        {
            foreach (var c in chars)
            {
                if (v.Contains(new string(new[] { c }))) return true;
            }
            return false;
        }

        public static bool IsValidPath(string v)
        {
            return !ContainsOneOfChars(v, Path.GetInvalidPathChars());
        }

    }
}
