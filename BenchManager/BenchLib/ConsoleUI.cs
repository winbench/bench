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
        /// <summary>
        /// Displays an informational message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        public void ShowInfo(string topic, string message)
        {
            Console.WriteLine("[INFO] " + topic + ": " + message);
        }

        /// <summary>
        /// Displays a warning message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        public void ShowWarning(string topic, string message)
        {
            Console.WriteLine("[WARNING] " + topic + ": " + message);
        }

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="topic">The topic of the message.</param>
        /// <param name="message">The message.</param>
        public void ShowError(string topic, string message)
        {
            Console.WriteLine("[ERROR] " + topic + ": " + message);
        }

        /// <summary>
        /// Prompts the user to input the necessary information
        /// about a Bench user.
        /// </summary>
        /// <param name="prompt">A string, shown to the user as prompt.</param>
        /// <returns>A new instance of <see cref="BenchUserInfo"/> with the read input from the user.</returns>
        public BenchUserInfo ReadUserInfo(string prompt)
        {
            var userInfo = new BenchUserInfo();
            Console.WriteLine(prompt);
            Console.Write("Username: ");
            userInfo.Name = Console.ReadLine();
            Console.Write("Email: ");
            userInfo.Email = Console.ReadLine();
            return userInfo;
        }

        /// <summary>
        /// Reads a password from the user in a safe fashion.
        /// </summary>
        /// <param name="prompt">A string, shown to the user as prompt.</param>
        /// <returns>An instance of <see cref="SecureString"/>.</returns>
        public SecureString ReadPassword(string prompt)
        {
            Console.WriteLine(prompt);

            // http://stackoverflow.com/questions/3404421/password-masking-console-application

            Console.Write("Password: ");
            SecureString pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.RemoveAt(pwd.Length - 1);
                        //Console.Write("\b \b");
                    }
                }
                else
                {
                    pwd.AppendChar(i.KeyChar);
                    //Console.Write("*");
                }
            }
            Console.WriteLine();
            return pwd;
        }

        /// <summary>
        /// Allow the user to edit the given text file.
        /// This methods returns, when the user ends the editing.
        /// </summary>
        /// <param name="path">A path to the text file to edit.</param>
        public void EditTextFile(string path)
        {
            var p = Process.Start(
            Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "notepad.exe"),
                path);
            p.WaitForExit();
        }

        /// <summary>
        /// Allow the user to edit the given text file.
        /// This methods returns, when the user ends the editing.
        /// </summary>
        /// <param name="path">A path to the text file to edit.</param>
        /// <param name="prompt">A string, shown to the user as prompt.</param>
        public void EditTextFile(string path, string prompt)
        {
            Console.WriteLine(prompt);
            Console.WriteLine("Close the text editor to proceed ...");
            EditTextFile(path);
        }
    }
}
