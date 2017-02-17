using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    class WinFormsUserInterface : IUserInterface
    {
        public Form ParentWindow { get; set; }

        public void Show(MessageBoxIcon typ, string topic, string message, string detailedMessage = null,
            Exception exception = null)
        {
            if (ParentWindow != null && ParentWindow.InvokeRequired)
            {
                ParentWindow.Invoke((InfoShowCase)Show, typ, topic, message, detailedMessage, exception);
                return;
            }
            if (ParentWindow.IsDisposed) ParentWindow = null;
            MessageBox.Show(ParentWindow, message, topic,
                MessageBoxButtons.OK, typ);
        }

        public void ShowVerbose(string topic, string message, string detailedMessage = null)
        {
            Show(MessageBoxIcon.None, topic, message, 
                detailedMessage: detailedMessage);
        }

        public void ShowInfo(string topic, string message, string detailedMessage = null)
        {
            Show(MessageBoxIcon.Information, topic, message,
                detailedMessage: detailedMessage);
        }

        public void ShowWarning(string topic, string message, string detailedMessage = null, Exception exception = null)
        {
            Show(MessageBoxIcon.Warning, topic, message,
                detailedMessage: detailedMessage,
                exception: exception);
        }

        public void ShowError(string topic, string message, string detailedMessage = null, Exception exception = null)
        {
            Show(MessageBoxIcon.Error, topic, message,
                detailedMessage: detailedMessage,
                exception: exception);
        }

        private delegate void InfoShowCase(MessageBoxIcon typ, string topic, string message, string detailedMessage, Exception exception);
    }
}
