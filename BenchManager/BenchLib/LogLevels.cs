using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This static class knows the different possible log levels, while running Bench tasks.
    /// It helps to recognize a log level from a string.
    /// </summary>
    public static class LogLevels
    {
        /// <summary>
        /// Log nothing.
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Log errors and informational messages.
        /// </summary>
        public const string Info = "info";

        /// <summary>
        /// Only log error messages.
        /// </summary>
        public const string Error = "error";

        /// <summary>
        /// Try to recognize a known log level in the given string.
        /// </summary>
        /// <param name="value">The string, which possibly describes a log level.</param>
        /// <returns>One of the known log levels.</returns>
        public static string GuessLevel(string value)
        {
            if (Info.Equals(value, StringComparison.InvariantCultureIgnoreCase)) return Info;
            if (None.Equals(value, StringComparison.InvariantCultureIgnoreCase)) return None;
            return Error;
        }
    }
}
