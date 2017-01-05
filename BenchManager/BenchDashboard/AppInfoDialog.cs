using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    internal partial class AppInfoDialog : Form
    {
        private static readonly string[] KnownProperties = new[]
            {
                PropertyKeys.AppTyp,
                PropertyKeys.AppWebsite,
                PropertyKeys.AppDocs,
                PropertyKeys.AppVersion,
                PropertyKeys.AppInstalledVersion,
                PropertyKeys.AppDependencies,
                PropertyKeys.AppForce,
                PropertyKeys.AppSetupTestFile,
                PropertyKeys.AppPackageName,
                PropertyKeys.AppUrl,
                PropertyKeys.AppDownloadCookies,
                PropertyKeys.AppDownloadHeaders,
                PropertyKeys.AppResourceName,
                PropertyKeys.AppArchiveName,
                PropertyKeys.AppArchiveTyp,
                PropertyKeys.AppArchivePath,
                PropertyKeys.AppDir,
                PropertyKeys.AppExe,
                PropertyKeys.AppRegister,
                PropertyKeys.AppPath,
                PropertyKeys.AppEnvironment,
                PropertyKeys.AppAdornedExecutables,
                PropertyKeys.AppRegistryKeys,
                PropertyKeys.AppLauncher,
                PropertyKeys.AppLauncherExecutable,
                PropertyKeys.AppLauncherArguments,
                PropertyKeys.AppLauncherIcon,
            };

        public AppInfoDialog(BenchConfiguration config, AppFacade app)
        {
            InitializeComponent();
            gridResolved.DoubleBuffered(true);
            lblAppId.Text = app.Label;
            LoadProperties(config, app);
            LoadDocumentation(app);
        }

        private void LoadProperties(BenchConfiguration config, AppFacade app)
        {
            gridResolved.Rows.Clear();
            AddRow(gridResolved, "ID", app.ID);
            foreach (var kvp in app.KnownProperties)
            {
                AddRow(gridResolved, kvp.Key, kvp.Value);
            }
            foreach (var kvp in app.UnknownProperties)
            {
                AddRow(gridResolved, kvp.Key, kvp.Value);
            }

            gridRaw.Rows.Clear();
            AddRow(gridRaw, "ID", app.ID);
            foreach (var key in config.PropertyNames(app.ID))
            {
                AddRow(gridRaw, key, config.GetRawGroupValue(app.ID, key));
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
    }
}
