using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using static Mastersign.Sequence.Sequence;

namespace Mastersign.Bench.UI
{
    /// <summary>
    /// A wizzard control, which allows the user to pre-select apps before the setup.
    /// </summary>
    internal partial class AppSelectionStepControl : WizzardStepControl
    {
        public AppSelectionStepControl()
        {
            Description = "App/Group Selection...";
            InitializeComponent();
        }

        private Dictionary<string, string> appLookup;

        public void InitializeStepControl(KeyValuePair<string, string>[] apps)
        {
            appLookup = Seq(apps).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            clstAppSelection.Items.Clear();
            Seq(apps).ForEach(kvp => clstAppSelection.Items.Add(kvp.Key));
        }

        public string[] SelectedApps
        {
            get
            {
                return Seq<string>(clstAppSelection.CheckedItems)
                    .Map((key, i) => appLookup[key])
                    .ToArray();
            }
        }
    }
}
