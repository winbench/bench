using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Mastersign.Bench.Dashboard.Properties;

namespace Mastersign.Bench.Dashboard
{
    public partial class AppLauncherControl : UserControl
    {
        public AppLauncherControl()
        {
            InitializeComponent();
        }

        public Core Core { get; set; }

        private AppIndexFacade appIndex;

        public AppIndexFacade AppIndex
        {
            get { return appIndex; }
            set
            {
                if (appIndex != null) ReleaseAppIndex();
                appIndex = value;
                if (appIndex != null) BindAppIndex();
            }
        }

        private void ReleaseAppIndex()
        {
            listView.Items.Clear();
            icons16.Images.Clear();
            icons32.Images.Clear();
        }

        private void BindAppIndex()
        {
            foreach (var app in appIndex.ActiveApps)
            {
                if (app.Launcher != null)
                {
                    LoadIcons(app);
                    listView.Items.Add(AppItem(app));
                }
            }
        }

        private ListViewItem AppItem(AppFacade app)
        {
            return new ListViewItem(app.Launcher)
            {
                Tag = app.ID,
                ImageKey = app.ID,
            };
        }

        private void LoadIcons(AppFacade app)
        {
            var path = app.LauncherIcon;
            Icon icon;
            try
            {
                icon = Icon.ExtractAssociatedIcon(path);
            }
            catch (Exception)
            {
                icon = Resources.MissingApp;
            }
            icons16.Images.Add(app.ID, new Icon(icon, icons16.ImageSize));
            icons32.Images.Add(app.ID, new Icon(icon, icons32.ImageSize));
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if (Core == null) return;
            var item = listView.SelectedItems.Count > 0 ? listView.SelectedItems[0] : null;
            if (item != null)
            {
                Core.LaunchApp((string)item.Tag);
            }
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (Core == null) return;
            var item = listView.SelectedItems.Count > 0 ? listView.SelectedItems[0] : null;
            if (item != null)
            {
                // nothing yet
            }
        }
    }
}
