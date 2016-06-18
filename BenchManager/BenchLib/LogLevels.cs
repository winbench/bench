using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class LogLevels
    {
        public const string None = "none";
        public const string Info = "info";
        public const string Error = "error";

        public static string GuessLevel(string value)
        {
            if (Info.Equals(value, StringComparison.InvariantCultureIgnoreCase)) return Info;
            if (None.Equals(value, StringComparison.InvariantCultureIgnoreCase)) return None;
            return Error;
        }
    }
}
