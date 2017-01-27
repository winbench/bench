using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mastersign.Bench.Dashboard.Properties;
using Mastersign.Bench.Markdown;

namespace Mastersign.Bench.Dashboard
{
    public partial class MarkdownViewer : Form
    {
        private readonly IBenchManager core;
        private readonly string windowTitle;

        public MarkdownViewer(IBenchManager core)
        {
            this.core = core;
            InitializeComponent();
            this.windowTitle = Text;
        }

        private void MarkdownViewer_Load(object sender, EventArgs e)
        {
            InitializeBounds();
        }

        private void MarkdownViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void InitializeBounds()
        {
            var region = Screen.PrimaryScreen.WorkingArea;
            var w = Math.Max(MinimumSize.Width, region.Width / 2);
            var h = Math.Max(MinimumSize.Height, region.Height);
            var x = 0;
            var y = region.Top;
            SetBounds(x, y, w, h);
        }

        public void LoadMarkdown(string file, string title = null)
        {
            Text = windowTitle + " - " + (title ?? Path.GetFileNameWithoutExtension(file));
            var md2html = markdownControl.ShowMarkdownFile(file, title);
            LoadTree(md2html.Anchors);
        }

        private void LoadTree(IList<MdAnchor> anchors)
        {
            treeView.Nodes.Clear();
            var parents = new Stack<TreeNode>();

            foreach (var a in anchors)
            {
                if (a is MdHeadline)
                {
                    var h = (MdHeadline)a;
                    for (int i = parents.Count; i >= h.Level; i--)
                    {
                        parents.Pop();
                    }
                    for (int i = parents.Count; i < h.Level - 1; i++)
                    {
                        var pU = new TreeNode("Unlabeled");
                        var cU = parents.Count > 0 ? parents.Peek().Nodes : treeView.Nodes;
                        cU.Add(pU);
                        parents.Push(pU);
                    }
                    var n = new TreeNode(h.Label);
                    n.Tag = h;
                    var c = parents.Count > 0 ? parents.Peek().Nodes : treeView.Nodes;
                    c.Add(n);
                    parents.Push(n);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            treeView.ExpandAll();
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var mdElement = e.Node.Tag as MdHeadline;
            if (mdElement != null)
            {
                markdownControl.ScrollToElement(mdElement.Id);
            }
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Scheme != "file")
            {
                e.Cancel = true;
                Process.Start(e.Url.ToString());
            }
        }
    }
}
