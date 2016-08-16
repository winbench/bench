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
    /// The base class for wizzard controls.
    /// </summary>
    internal partial class WizzardStepControl : UserControl
    {
        public string Description { get; protected set; }

        public WizzardStepControl()
        {
            InitializeComponent();
        }
    }
}
