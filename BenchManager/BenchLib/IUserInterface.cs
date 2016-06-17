using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Mastersign.Bench
{
    public interface IUserInterface
    {
        BenchUserInfo ReadUserInfo(string prompt);

        SecureString ReadPassword(string prompt);

        void EditTextFile(string file);

        void EditTextFile(string file, string prompt);

        void ShowInfo(string topic, string message);

        void ShowWarning(string topic, string message);

        void ShowError(string topic, string message);
    }
}
