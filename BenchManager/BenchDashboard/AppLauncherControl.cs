using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Mastersign.Bench.Dashboard.Properties;
using System.Threading.Tasks;

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
                appIndex = value;
                if (appIndex != null) BindAppIndex();
            }
        }

        private async void BindAppIndex()
        {
            icons16.Images.Clear();
            icons32.Images.Clear();
            foreach (var app in appIndex.ActiveApps)
            {
                if (app.Launcher != null)
                {
                    var icons = await LoadIcons(app);
                    icons16.Images.Add(app.ID, icons.Item1);
                    icons32.Images.Add(app.ID, icons.Item2);
                }
            }
            listView.Items.Clear();
            var items = from app in appIndex.ActiveApps
                        where app.Launcher != null
                        select AppItem(app);
            listView.Items.AddRange(items.ToArray());
        }

        private ListViewItem AppItem(AppFacade app)
        {
            return new ListViewItem(app.Launcher)
            {
                Tag = app.ID,
                ImageKey = app.ID,
            };
        }

        private Task<Tuple<Icon, Icon>> LoadIcons(AppFacade app)
        {
            return Task.Run(() =>
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
                return Tuple.Create(
                    new Icon(icon, icons16.ImageSize),
                    new Icon(icon, icons32.ImageSize));
            });
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
