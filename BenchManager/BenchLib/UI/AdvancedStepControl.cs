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
    internal partial class AdvancedStepControl : WizzardStepControl
    {
        public AdvancedStepControl()
        {
            Description = "Advanced settings...";
            InitializeComponent();
        }

        public bool EditCustomConfigBeforeSetup
        {
            get { return chkEditCustomConfigBeforeSetup.Checked; }
            set { chkEditCustomConfigBeforeSetup.Checked = value; }
        }

        public bool StartAutoSetup
        {
            get { return chkStartAutoSetup.Checked; }
            set { chkStartAutoSetup.Checked = value; }
        }
    }
}
