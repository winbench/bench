using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    public partial class UserInfoDialog : Form
    {
        public UserInfoDialog()
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

        public string UserName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }

        public string UserEmail
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        public static BenchUserInfo GetUserInfo(string prompt) {
            var dlg = new UserInfoDialog();
            dlg.Prompt = prompt;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return new BenchUserInfo(dlg.UserName, dlg.UserEmail);
            }
            else
            {
                return null;
            }
        }
    }
}
