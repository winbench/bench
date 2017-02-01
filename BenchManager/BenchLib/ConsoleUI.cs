using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// An implementation for <see cref="IUserInterface"/>, which is based on the console.
    /// </summary>
    public class ConsoleUserInterface : IUserInterface
    {
        private static readonly object consoleSyncHandle = new object();

        private void Show(string typ, string topic, string message,
            ConsoleColor? color = null,
            string detailedMessage = null,
            Exception exception = null)
        {
            lock (consoleSyncHandle)
            {
                if (color.HasValue) Console.ForegroundColor = color.Value;

                Console.WriteLine(
                    "[{0}] {1}: {2}",
                    typ,
                    topic,
                    message);

                if (!string.IsNullOrEmpty(detailedMessage))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(detailedMessage);
                }
                if (exception != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(exception.ToString());
                }
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Displays a detail message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        public void ShowVerbose(string topic, string message, string detailedMessage = null)
        {
            Show("VERBOSE", topic, message, color: ConsoleColor.DarkCyan);
        }

        /// <summary>
        /// Displays an informational message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        public void ShowInfo(string topic, string message, string detailedMessage = null)
        {
            Show("INFO", topic, message, detailedMessage: detailedMessage);
        }

        /// <summary>
        /// Displays a warning message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        /// <param name="exception">An <see cref="Exception"/> object, causing the warning.</param>
        public void ShowWarning(string topic, string message, string detailedMessage = null,
            Exception exception = null)
        {
            Show("WARNING", topic, message,
                color: ConsoleColor.Yellow,
                detailedMessage: detailedMessage,
                exception: exception);
        }

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        /// <param name="exception">An <see cref="Exception"/> object, causing the warning.</param>
        public void ShowError(string topic, string message, string detailedMessage = null,
            Exception exception = null)
        {
            Show("ERROR", topic, message,
                color: ConsoleColor.Red,
                detailedMessage: detailedMessage,
                exception: exception);
        }
    }
}
