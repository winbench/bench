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
                PropertyKeys.AppVersion,
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
            lblAppId.Text = app.ID;
            LoadProperties(config, app);
        }

        private void LoadProperties(BenchConfiguration config, AppFacade app)
        {
            gridResolved.Rows.Clear();
            AddRow(gridResolved, PropertyKeys.AppTyp, app.Typ);
            AddRow(gridResolved, PropertyKeys.AppWebsite, app.Website);
            AddRow(gridResolved, PropertyKeys.AppVersion, app.Version);
            AddRow(gridResolved, PropertyKeys.AppDependencies, app.Dependencies);
            AddRow(gridResolved, PropertyKeys.AppForce, app.Force);
            AddRow(gridResolved, PropertyKeys.AppSetupTestFile, app.SetupTestFile);
            AddRow(gridResolved, PropertyKeys.AppPackageName, app.PackageName);
            AddRow(gridResolved, PropertyKeys.AppUrl, app.Url);
            AddRow(gridResolved, PropertyKeys.AppDownloadCookies, app.DownloadCookies);
            AddRow(gridResolved, PropertyKeys.AppDownloadHeaders, app.DownloadHeaders);
            AddRow(gridResolved, PropertyKeys.AppResourceName, app.ResourceFileName);
            AddRow(gridResolved, PropertyKeys.AppArchiveName, app.ResourceArchiveName);
            AddRow(gridResolved, PropertyKeys.AppArchivePath, app.ResourceArchivePath);
            AddRow(gridResolved, PropertyKeys.AppDir, app.Dir);
            AddRow(gridResolved, PropertyKeys.AppExe, app.Exe);
            AddRow(gridResolved, PropertyKeys.AppRegister, app.Register);
            AddRow(gridResolved, PropertyKeys.AppPath, app.Path);
            AddRow(gridResolved, PropertyKeys.AppEnvironment, app.Environment);
            AddRow(gridResolved, PropertyKeys.AppAdornedExecutables, app.AdornedExecutables);
            AddRow(gridResolved, PropertyKeys.AppRegistryKeys, app.RegistryKeys);
            AddRow(gridResolved, PropertyKeys.AppLauncher, app.Launcher);
            AddRow(gridResolved, PropertyKeys.AppLauncherExecutable, app.LauncherExecutable);
            AddRow(gridResolved, PropertyKeys.AppLauncherArguments, app.LauncherArguments);
            AddRow(gridResolved, PropertyKeys.AppLauncherIcon, app.LauncherIcon);
            foreach(var key in config.PropertyNames(app.ID))
            {
                if (!KnownProperties.Contains(key))
                {
                    AddRow(gridResolved, key, config.GetGroupValue(app.ID, key));
                }
            }

            gridRaw.Rows.Clear();
            foreach(var key in config.PropertyNames(app.ID))
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
    }
}
