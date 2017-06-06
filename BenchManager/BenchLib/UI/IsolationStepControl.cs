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
    /// A wizzard control, which allows the user to setup advanced configuration parameters
    /// for the Bench initialization.
    /// </summary>
    internal partial class IsolationStepControl : WizzardStepControl
    {
        public IsolationStepControl()
        {
            Description = "Isloation settings...";
            InitializeComponent();
        }

        public bool IntegrateIntoUserProfile
        {
            get { return radIntegrated.Checked; }
            set
            {
                if (value)
                {
                    radIntegrated.Checked = true;
                }
                else
                {
                    radIsolated.Checked = true;
                }
            }
        }

    }
}
