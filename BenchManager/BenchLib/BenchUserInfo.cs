using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class represents the user of the Bench system.
    /// It holds the name and the email address, of the user, which are used
    /// to configure the environment variables and apps.
    /// </summary>
    public class BenchUserInfo
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Initializes a new and empty instance of <see cref="BenchUserInfo"/>.
        /// </summary>
        public BenchUserInfo() { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchUserInfo"/>.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="email">The email of the user.</param>
        public BenchUserInfo(string name, string email)
        {
            Name = name;
            Email = email;
        }

        /// <summary>
        /// Creates a string representation of the user info.
        /// </summary>
        /// <returns>A string with name and email of the user.</returns>
        public override string ToString()
        {
            return string.Format("Name='{0}', Email='{1}'", Name, Email);
        }
    }
}
