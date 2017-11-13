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
using System.Diagnostics;

namespace Mastersign.Bench.Dashboard
{
    public partial class AppLauncherControl : UserControl
    {
        private AppFacade contextApp;

        public AppLauncherControl()
        {
            InitializeComponent();
            VisibleChanged += VisibleChangedHandler;
        }
        
        private void VisibleChangedHandler(object sender, EventArgs e)
        {
            if (!Visible) return;
            if (listView.Items.Count > 0 && icons32.Images.Count == 0)
            {
                Application.DoEvents();
                LoadIconImages();
            }
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

        private void BindAppIndex()
        {
            icons16.Images.Clear();
            icons32.Images.Clear();
            listView.Items.Clear();
            var items = from app in appIndex.ActiveApps
                        where app.Launcher != null
                        select AppItem(app);
            listView.Items.AddRange(items.ToArray());
            if (Visible) LoadIconImages();
        }

        private async void LoadIconImages()
        {
            foreach (var app in appIndex.ActiveApps)
            {
                if (app.Launcher != null)
                {
                    var icons = await LoadIcons(app);
                    icons16.Images.Add(app.ID, icons.Item1);
                    icons32.Images.Add(app.ID, icons.Item2);
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

        private void ShowAppContextMenu(AppFacade app, Point position)
        {
            contextApp = app;
            var ctxm = new ContextMenuStrip();
            var runItem = new ToolStripMenuItem("Run App", Resources.do_16, RunAppHandler);
            runItem.Font = new Font(runItem.Font, runItem.Font.Style | FontStyle.Bold);
            ctxm.Items.Add(runItem);
            ctxm.Items.Add(new ToolStripSeparator());
            ctxm.Items.Add(new ToolStripMenuItem("App Info", Resources.info_16, AppInfoHandler));
            if (!string.IsNullOrWhiteSpace(contextApp.Website))
            {
                ctxm.Items.Add(new ToolStripMenuItem("Website", Resources.website_16, AppWebsiteHandler));
            }
            foreach (var n in contextApp.Docs.Keys.OrderBy(k => k))
            {
                ctxm.Items.Add(new ToolStripMenuItem(n, Resources.website_16, AppDocLinkHandler)
                { Tag = contextApp.Docs[n] });
            }
            ctxm.Items.Add(new ToolStripSeparator());
            ctxm.Items.Add(new ToolStripMenuItem("Open App Directory", Resources.browse_folder_16, AppOpenDirHandler));

            var copyMenu = new ToolStripMenuItem("Copy to Clipboard ...", Resources.copy_16);
            copyMenu.DropDownItems.Add(new ToolStripMenuItem("Launcher Path", null, CopyLauncherPathHandler));
            copyMenu.DropDownItems.Add(new ToolStripMenuItem("Executable Path", null, CopyExecutablePathHandler));
            if (!string.IsNullOrWhiteSpace(contextApp.Url))
            {
                copyMenu.DropDownItems.Add(new ToolStripMenuItem("Download URL", null, CopyDownloadUrlHandler));
            }
            ctxm.Items.Add(copyMenu);
            ctxm.Show(listView, position);
        }

        private void DoubleClickHandler(object sender, EventArgs e)
        {
            if (Core == null) return;
            var item = listView.SelectedItems.Count > 0 ? listView.SelectedItems[0] : null;
            if (item != null)
            {
                Core.LaunchApp((string)item.Tag);
            }
        }

        private void MouseClickHandler(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (Core == null) return;
            var item = listView.SelectedItems.Count > 0 ? listView.SelectedItems[0] : null;
            if (item != null)
            {
                ShowAppContextMenu(Core.Config.Apps[(string)item.Tag], e.Location);
            }
        }

        private void RunAppHandler(object sender, EventArgs e)
            => Core.LaunchApp(contextApp.ID);

        private void AppInfoHandler(object sender, EventArgs e)
            => Core.ShowAppInfo(contextApp.ID);

        private void AppWebsiteHandler(object sender, EventArgs e)
            => Core.ShowAppWebsite(contextApp.ID);

        private void AppDocLinkHandler(object sender, EventArgs e)
            => Process.Start((sender as ToolStripItem)?.Tag as string);

        private void AppOpenDirHandler(object sender, EventArgs e)
            => Process.Start(contextApp.Dir);

        private void CopyLauncherPathHandler(object sender, EventArgs e) 
            => Clipboard.SetData("Text", contextApp.GetLauncherScriptFile());

        private void CopyExecutablePathHandler(object sender, EventArgs e)
            => Clipboard.SetData("Text", contextApp.Exe);

        private void CopyDownloadUrlHandler(object sender, EventArgs e)
            => Clipboard.SetData("Text", contextApp.Url);

        private void DragEnterHandler(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop", true))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent("System.String", true))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void DragOverHandler(object sender, DragEventArgs e)
        {
            var p = listView.PointToClient(new Point(e.X, e.Y));
            var item = listView.GetItemAt(p.X, p.Y);
            e.Effect = item != null
                ? e.Data.GetDataPresent("FileDrop", true)
                    ? DragDropEffects.Move
                    : DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private void DragDropHandler(object sender, DragEventArgs e)
        {
            var p = listView.PointToClient(new Point(e.X, e.Y));
            var item = listView.GetItemAt(p.X, p.Y);
            if (item == null) return;

            if (e.Data.GetDataPresent("FileDrop", true))
            {
                var paths = e.Data.GetData("FileDrop", true) as string[];
                Core.LaunchApp((string)item.Tag, paths);
            }
            else if (e.Data.GetDataPresent("System.String", true))
            {
                var text = e.Data.GetData("System.String", true) as string;
                Core.LaunchApp((string)item.Tag, text);
            }
        }
    }
}
