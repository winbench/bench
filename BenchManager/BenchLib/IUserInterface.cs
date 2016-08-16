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
        /// Prompts the user to input the necessary information
        /// about a Bench user.
        /// </summary>
        /// <param name="prompt">A string, shown to the user as prompt.</param>
        /// <returns>A new instance of <see cref="BenchUserInfo"/> with the read input from the user.</returns>
        BenchUserInfo ReadUserInfo(string prompt);

        /// <summary>
        /// Reads a password from the user in a safe fashion.
        /// </summary>
        /// <param name="prompt">A string, shown to the user as prompt.</param>
        /// <returns>An instance of <see cref="SecureString"/>.</returns>
        SecureString ReadPassword(string prompt);

        /// <summary>
        /// Allow the user to edit the given text file.
        /// This methods returns, when the user ends the editing.
        /// </summary>
        /// <param name="path">A path to the text file to edit.</param>
        void EditTextFile(string path);

        /// <summary>
        /// Allow the user to edit the given text file.
        /// This methods returns, when the user ends the editing.
        /// </summary>
        /// <param name="path">A path to the text file to edit.</param>
        /// <param name="prompt">A string, shown to the user as prompt.</param>
        void EditTextFile(string path, string prompt);

        /// <summary>
        /// Displays an informational message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        void ShowInfo(string topic, string message);

        /// <summary>
        /// Displays a warning message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        void ShowWarning(string topic, string message);

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        void ShowError(string topic, string message);
    }
}
