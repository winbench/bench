using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This interface describes the capability to communicate with the user
    /// for some simple tasks. 
    /// </summary>
    public interface IUserInterface
    {
        /// <summary>
        /// Displays a detail message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        void ShowVerbose(string topic, string message, string detailedMessage = null);

        /// <summary>
        /// Displays an informational message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        void ShowInfo(string topic, string message, string detailedMessage = null);

        /// <summary>
        /// Displays a warning message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        /// <param name="exception">An <see cref="Exception"/> object, causing the warning.</param>
        void ShowWarning(string topic, string message, string detailedMessage = null,
            Exception exception = null);

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailedMessage">Additional information.</param>
        /// <param name="exception">An <see cref="Exception"/> object, causing the warning.</param>
        void ShowError(string topic, string message, string detailedMessage = null,
            Exception exception = null);

    }
}
