using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.UI
{
    public partial class WizzardStepControl : UserControl
    {
        public string Description { get; protected set; }

        public WizzardStepControl()
        {
            InitializeComponent();
        }
    }
}
