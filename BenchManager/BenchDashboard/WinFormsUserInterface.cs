using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    class WinFormsUserInterface : IUserInterface
    {
        public Form ParentWindow { get; set; }

        public void ShowInfo(string topic, string message)
        {
            if (ParentWindow != null && ParentWindow.InvokeRequired)
            {
                ParentWindow.Invoke((InfoShowCase)ShowInfo, topic, message);
                return;
            }
            if (ParentWindow.IsDisposed) ParentWindow = null;
            MessageBox.Show(ParentWindow, message, topic,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowWarning(string topic, string message)
        {
            if (ParentWindow != null && ParentWindow.InvokeRequired)
            {
                ParentWindow.Invoke((InfoShowCase)ShowWarning, topic, message);
                return;
            }
            if (ParentWindow.IsDisposed) ParentWindow = null;
            MessageBox.Show(ParentWindow, message, topic,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ShowError(string topic, string message)
        {
            if (ParentWindow != null && ParentWindow.InvokeRequired)
            {
                ParentWindow.Invoke((InfoShowCase)ShowError, topic, message);
                return;
            }
            if (ParentWindow.IsDisposed) ParentWindow = null;
            MessageBox.Show(ParentWindow, message, topic,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public BenchUserInfo ReadUserInfo(string prompt)
        {
            return UserInfoDialog.GetUserInfo(prompt);
        }

        public System.Security.SecureString ReadPassword(string prompt)
        {
            return PasswordDialog.GetPassword(prompt);
        }

        public void EditTextFile(string path)
        {
            var p = Process.Start(
                Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "notepad.exe"),
                path);
            p.WaitForExit();
        }

        public void EditTextFile(string path, string prompt)
        {
            MessageBox.Show(ParentWindow, prompt
                + Environment.NewLine + Environment.NewLine
                + "Close the editor to continue.");
            EditTextFile(path);
        }

        private delegate void InfoShowCase(string topic, string message);
    }
}
