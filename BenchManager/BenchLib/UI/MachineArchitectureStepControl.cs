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
    internal partial class MachineArchitectureStepControl : WizzardStepControl
    {
        public MachineArchitectureStepControl()
        {
            Description = "Configure preferred machine architecture...";
            InitializeComponent();
        }

        public bool Allow64Bit
        {
            get { return chkAllow64Bit.Checked; }
            set { chkAllow64Bit.Checked = value; }
        }
    }
}
