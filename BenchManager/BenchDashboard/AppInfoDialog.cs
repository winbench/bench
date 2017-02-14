using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    internal partial class AppInfoDialog : Form
    {
        public AppInfoDialog(BenchConfiguration config, AppFacade app)
        {
            InitializeComponent();
            gridResolved.DoubleBuffered(true);
            lblAppId.Text = app.Label;
            LoadProperties(config, app);
            LoadLicense(app);
            LoadDocumentation(app);
        }

        private void LoadProperties(BenchConfiguration config, AppFacade app)
        {
            gridResolved.Rows.Clear();
            foreach (var kvp in app.KnownProperties)
            {
                AddRow(gridResolved, kvp.Key, kvp.Value);
            }
            foreach (var kvp in app.UnknownProperties)
            {
                AddRow(gridResolved, kvp.Key, kvp.Value);
            }

            gridRaw.Rows.Clear();
            foreach (var key in config.AppProperties.PropertyNames(app.ID))
            {
                AddRow(gridRaw, key, config.AppProperties.GetRawGroupValue(app.ID, key));
            }
        }

        private void AddRow(DataGridView grid, string name, object value)
        {
            if (value is bool)
            {
                AddRow(grid, name, value.ToString());
            }
            else if (value is string)
            {
                AddRow(grid, name, (string)value);
            }
            else if (value is string[])
            {
                AddRow(grid, name, string.Join(", ",
                    ((string[])value).Select(v => $"`{v}`")));
            }
            else if (value is IDictionary<string, string>)
            {
                AddRow(grid, name, string.Join(", ",
                    ((IDictionary<string, string>)value).Select(kvp => $"`{kvp.Key}`=`{kvp.Value}`")));
            }
            else if (value == null)
            {
                AddRow(grid, name, null);
            }
            else
            {
                AddRow(grid, name, "UNKNOWN: " + value.ToString());
            }
        }

        private void AddRow(DataGridView grid, string name, string value)
        {
            grid.Rows.Add(name, value);
        }

        private void LoadLicense(AppFacade app)
        {
            llblLicense.Tag = app.LicenseUrl;
            llblLicense.Visible = llblLicense.Tag != null;
        }

        private void LoadDocumentation(AppFacade app)
        {
            var docText = app.MarkdownDocumentation;
            if (!string.IsNullOrWhiteSpace(docText))
            {
                mdDocumentation.ShowMarkdownText(docText, app.Label);
            }
            else
            {
                tabControl.TabPages.Remove(tabDocumentation);
            }
        }

        private void LicenseHandler(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = ((Control)sender).Tag as Uri;
            if (url != null)
            {
                Process.Start(url.AbsoluteUri);
            }
        }
    }
}
