using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    public partial class PasswordDialog : Form
    {
        public PasswordDialog()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public string Prompt
        {
            get { return lblPrompt.Text; }
            set { lblPrompt.Text = value; }
        }

        public SecureString Password
        {
            get {
                var s = new SecureString();
                foreach (var c in txtPassword.Text) {
                    s.AppendChar(c);
                }
                return s;
            }
            set { txtPassword.Text = value.ToString(); }
        }

        public static SecureString GetPassword(string prompt) {
            var dlg = new PasswordDialog();
            dlg.Prompt = prompt;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.Password;
            }
            else
            {
                return null;
            }
        }
    }
}
