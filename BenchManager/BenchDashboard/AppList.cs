using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Mastersign.Bench.Dashboard
{
    public partial class AppList : UserControl
    {
        private DataGridViewColumn sortedColumn;
        private ListSortDirection sortDirection;

        private AppFacade contextApp;

        private readonly Dictionary<string, AppWrapper> appLookup
            = new Dictionary<string, AppWrapper>();
        private int firstVisibleRowIndex;

        private readonly List<string> columnLabels = new List<string>();
        private readonly Dictionary<string, DataGridViewColumn> columns
            = new Dictionary<string, DataGridViewColumn>();
        private static string[] defaulColumnLabels
            = new[] { "Order", "ID", "Version", "Active", "Deactivated", "Status" };
        private DataGridViewColumn iconColumn;

        private Core core;

        public event EventHandler<TaskStartedEventArgs> TaskStarted;

        public event EventHandler<TaskInfoEventArgs> TaskInfo;

        public AppList()
        {
            InitializeComponent();
            gridApps.DoubleBuffered(true);
            gridApps.AutoGenerateColumns = false;
            iconColumn = gridApps.Columns[0];
            foreach (DataGridViewColumn col in gridApps.Columns)
            {
                if (string.IsNullOrEmpty(col.HeaderText)) continue;
                columnLabels.Add(col.HeaderText);
                columns.Add(col.HeaderText, col);
            }
        }

        public Core Core
        {
            get => core;
            set
            {
                if (core == value) return;
                if (core != null)
                {
                    core.ConfigReloaded -= CoreConfigReloadedHandler;
                    core.AllAppStateChanged -= CoreAllAppStateChangedHandler;
                    core.AppActivationChanged -= CoreAppActivationChangedHandler;
                    core.AppStateChanged -= CoreAppStateChangedHandler;
                    core.BusyChanged -= CoreBusyChangedHandler;
                }
                core = value;
                if (core != null)
                {
                    InitializeAppListColumns();
                    InitializeAppList();
                    core.ConfigReloaded += CoreConfigReloadedHandler;
                    core.AllAppStateChanged += CoreAllAppStateChangedHandler;
                    core.AppActivationChanged += CoreAppActivationChangedHandler;
                    core.AppStateChanged += CoreAppStateChangedHandler;
                    core.BusyChanged += CoreBusyChangedHandler;
                }
            }
        }

        public string[] DefaultColumnLabels => defaulColumnLabels;

        public List<string> ColumnLabels => columnLabels;

        public void FocusSearchBox()
        {
            txtSearch.SelectAll();
            txtSearch.Focus();
        }

        private void InitializeAppListColumns()
        {
            if (core == null) return;
            gridApps.SuspendLayout();
            gridApps.Columns.Clear();
            var colLabels = core.Config.GetStringListValue(
                ConfigPropertyKeys.DashboardSetupAppListColumns,
                defaulColumnLabels);
            iconColumn.DisplayIndex = 0;
            gridApps.Columns.Add(iconColumn);
            var pos = 1;
            foreach (var colLabel in colLabels)
            {
                if (columns.TryGetValue(colLabel, out DataGridViewColumn col))
                {
                    col.DisplayIndex = pos++;
                    gridApps.Columns.Add(col);
                }
            }
            gridApps.ResumeLayout();
        }

        private void InitializeAppList()
        {
            AsyncManager.StartTask(() =>
            {
                appLookup.Clear();
                var list = new List<AppWrapper>();
                var cnt = 0;
                foreach (var app in core.Config.Apps)
                {
                    cnt++;
                    app.LoadCachedValues();
                    var wrapper = new AppWrapper(app, cnt);
                    list.Add(wrapper);
                    appLookup[app.ID] = wrapper;
                }

                var bindingList = new SortedBindingList<AppWrapper>(list);

                BeginInvoke((ThreadStart)(() =>
                {
                    var selectedRow = gridApps.SelectedRows.Count > 0 ? gridApps.SelectedRows[0].Index : -10;
                    gridApps.SuspendLayout();
                    gridApps.DataSource = bindingList;
                    if (sortedColumn != null)
                    {
                        gridApps.Sort(sortedColumn, sortDirection);
                    }
                    if (selectedRow >= 0 && gridApps.Rows.Count >= selectedRow + 1)
                    {
                        gridApps.Rows[selectedRow].Selected = true;
                    }
                    if (firstVisibleRowIndex >= 0 && gridApps.Rows.Count > 0)
                    {
                        gridApps.FirstDisplayedScrollingRowIndex = firstVisibleRowIndex;
                    }
                    gridApps.ResumeLayout();
                }));
            });
        }

        private void CoreConfigReloadedHandler(object sender, EventArgs e)
        {
            firstVisibleRowIndex = gridApps.FirstDisplayedScrollingRowIndex;
            InitializeAppListColumns();
            InitializeAppList();
        }

        private void CoreAllAppStateChangedHandler(object sender, EventArgs e)
        {
            gridApps.Refresh();
        }

        private void CoreAppActivationChangedHandler(object sender, EventArgs e)
        {
            gridApps.Refresh();
        }

        private void CoreAppStateChangedHandler(object sender, AppEventArgs e)
        {
            NotifyAppStateChange(e.ID);
        }

        private void CoreBusyChangedHandler(object sender, EventArgs e)
        {
            var notBusy = !core.Busy;
            foreach (ToolStripItem mi in ctxmAppActions.Items)
            {
                if (mi == miAppInfo) continue;
                if (mi == miWebsite) continue;
                mi.Enabled = notBusy;
            }
        }

        private void NotifyAppStateChange(string appId)
        {
            ForAppWrapper(appId, w =>
            {
                w.App.DiscardCachedValues();
                w.NotifyChanges();
            });
        }

        private void ForAppWrapper(string appId, Action<AppWrapper> action)
        {
            AppWrapper wrapper;
            if (appLookup.TryGetValue(appId, out wrapper))
            {
                action(wrapper);
            }
        }

        private void AnnounceTask(string msg)
        {
            TaskStarted?.Invoke(this, new TaskStartedEventArgs(msg));
        }

        private void TaskInfoHandler(TaskInfo info)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action<TaskInfo>)TaskInfoHandler, info);
                return;
            }
            TaskInfo?.Invoke(this, new TaskInfoEventArgs(info));
        }

        #region Context Menu Handler

        private void AppInfoHandler(object sender, EventArgs e)
        {
            if (contextApp == null) return;
            core.ShowAppInfo(contextApp.ID);
        }

        private void OpenWebsiteHandler(object sender, EventArgs e)
        {
            if (contextApp == null) return;
            core.ShowAppWebsite(contextApp.ID);
        }

        private async void InstallAppHandler(object sender, EventArgs e)
        {
            AnnounceTask("Install App " + contextApp.ID);
            await core.InstallAppsAsync(TaskInfoHandler, contextApp.ID);
            contextApp = null;
        }

        private async void ReinstallAppHandler(object sender, EventArgs e)
        {
            AnnounceTask("Reinstall App " + contextApp.ID);
            await core.ReinstallAppsAsync(TaskInfoHandler, contextApp.ID);
            contextApp = null;
        }

        private async void UpgradeAppHandler(object sender, EventArgs e)
        {
            AnnounceTask("Upgrade App " + contextApp.ID);
            await core.UpgradeAppsAsync(TaskInfoHandler, contextApp.ID);
            contextApp = null;
        }

        private async void UpgradePackageHandler(object sender, EventArgs e)
        {
            AnnounceTask("Upgrade App " + contextApp.ID);
            await core.UpgradeAppsAsync(TaskInfoHandler, contextApp.ID);
            contextApp = null;
        }

        private async void DownloadAppResourceHandler(object sender, EventArgs e)
        {
            AnnounceTask("Download App Resource for " + contextApp.ID);
            await core.DownloadAppResourcesAsync(contextApp.ID, TaskInfoHandler);
            contextApp = null;
        }

        private async void DeleteAppResourceHandler(object sender, EventArgs e)
        {
            AnnounceTask("Delete App Resource for " + contextApp.ID);
            await core.DeleteAppResourcesAsync(TaskInfoHandler, contextApp.ID);
            contextApp = null;
        }

        private async void UninstallAppHandler(object sender, EventArgs e)
        {
            AnnounceTask("Uninstall App " + contextApp.ID);
            await core.UninstallAppsAsync(TaskInfoHandler, contextApp.ID);
            contextApp = null;
        }

        #endregion

        #region Grid Handler

        private void gridApps_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0) return;
            var col = gridApps.Columns[e.ColumnIndex];
            if (col == sortedColumn)
            {
                switch (sortDirection)
                {
                    case ListSortDirection.Ascending:
                        sortDirection = ListSortDirection.Descending;
                        break;
                    case ListSortDirection.Descending:
                        sortDirection = ListSortDirection.Ascending;
                        break;
                }
            }
            else
            {
                sortedColumn = col;
                sortDirection = ListSortDirection.Ascending;
            }
            gridApps.Sort(sortedColumn, sortDirection);
        }

        private void gridApps_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = gridApps.Rows[e.RowIndex];
            row.Selected = true;
            var appWrapper = row.DataBoundItem as AppWrapper;
            if (appWrapper == null) return;
            contextApp = appWrapper.App;

            miWebsite.Visible = !string.IsNullOrEmpty(contextApp.Website);

            miInstall.Visible = contextApp.CanInstall;
            miReinstall.Visible = contextApp.CanReinstall;
            miUpgrade.Visible = contextApp.CanUpgrade;
            miPackageUpgrade.Visible = contextApp.IsInstalled && contextApp.IsManagedPackage;
            miUninstall.Visible = contextApp.CanUninstall;

            miDownloadResource.Visible = contextApp.CanDownloadResource;
            miDeleteResource.Visible = contextApp.CanDeleteResource;

            var g1 = contextApp.CanInstall
                  || contextApp.CanReinstall
                  || contextApp.CanUpgrade
                  || contextApp.IsInstalled && contextApp.IsManagedPackage
                  || contextApp.CanUninstall;
            var g2 = contextApp.CanDownloadResource
                  || contextApp.CanDeleteResource;

            tsSeparatorWebsite.Visible = g1;
            tsSeparatorDownloads.Visible = g1 && g2;

            e.ContextMenuStrip = ctxmAppActions;
        }

        private void gridApps_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (core.Busy) return;
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            var col = gridApps.Columns[e.ColumnIndex];
            if (col == colActivated || col == colExcluded)
            {
                var row = gridApps.Rows[e.RowIndex];
                var appWrapper = row.DataBoundItem as AppWrapper;
                if (col == colActivated)
                {
                    core.SetAppActivated(appWrapper.ID, !appWrapper.App.IsActivated);
                }
                if (col == colExcluded)
                {
                    core.SetAppDeactivated(appWrapper.ID, !appWrapper.App.IsDeactivated);
                }
            }
            if (col == colLicense)
            {
                var row = gridApps.Rows[e.RowIndex];
                var appWrapper = row.DataBoundItem as AppWrapper;
                var url = appWrapper.LicenseUrl;
                if (url != null)
                {
                    System.Diagnostics.Process.Start(url.AbsoluteUri);
                }
            }
        }

        private void gridApps_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            var row = gridApps.Rows[e.RowIndex];
            var appWrapper = row.DataBoundItem as AppWrapper;
            if (appWrapper != null)
            {
                new AppInfoDialog(core.Config, appWrapper.App).ShowDialog(this);
            }
        }

        #endregion

        #region Search

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var apps = appLookup.Values;
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchWords = AppSearch.TokenizeSearchString(txtSearch.Text).Select(AppSearch.NormalizeForSearch).ToArray();
                var sortedList = new SortedBindingList<AppWrapper>(apps
                    .Where(w => w.Match(searchWords))
                    .OrderBy(w => w.App.Label)
                    .OrderByDescending(w => w.SearchScore));
                gridApps.DataSource = sortedList;
            }
            else
            {
                gridApps.DataSource = new SortedBindingList<AppWrapper>(apps);
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
        }

        #endregion
    }

    public class TaskStartedEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public TaskStartedEventArgs(string message)
        {
            Message = message;
        }
    }

    public class TaskInfoEventArgs : EventArgs
    {
        public TaskInfo TaskInfo { get; private set; }

        public TaskInfoEventArgs(TaskInfo taskInfo)
        {
            TaskInfo = taskInfo;
        }
    }
}
