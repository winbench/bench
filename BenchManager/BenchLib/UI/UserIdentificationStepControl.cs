using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.UI
{
    /// <summary>
    /// A wizzard control, which allows the user to identify himself
    /// with name and email address.
    /// </summary>
    internal partial class UserIdentificationStepControl : WizzardStepControl
    {
        public UserIdentificationStepControl()
        {
            Description = "Identify yourself...";
            InitializeComponent();
        }

        public string UserName
        {
            get { return txtUsername.Text; }
            set { txtUsername.Text = value; }
        }

        public string UserEmail
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }
    }
}
