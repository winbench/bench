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
        private readonly static string template;

        static MarkdownViewer()
        {
            template = Resources.MarkdownViewerTemplate.Replace("$CSS$", Resources.MarkdownViewerStyle);
        }

        private readonly IBenchManager core;
        private readonly string windowTitle;
        private string tempFile;

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
            TempFile = null;
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

        private string TempFile
        {
            get { return tempFile; }
            set
            {
                if (tempFile != null && File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
                tempFile = value;
            }
        }

        private Uri TempDocumentUrl { get { return new Uri("file:///" + TempFile); } }

        private string NewTempFilePath()
        {
            return Path.Combine(
                core.Config.GetStringValue(PropertyKeys.TempDir),
                Path.GetRandomFileName() + ".html");
        }

        public void LoadMarkdown(string file, string title = null)
        {
            TempFile = NewTempFilePath();
            title = title ?? Path.GetFileNameWithoutExtension(file);
            Text = windowTitle + " - " + title;
            string html;
            var md2html = new MarkdownToHtmlConverter();
            try
            {
                using (var s = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    html = md2html.ConvertToHtml(s);
                }
                html = template.Replace("$TITLE$", title).Replace("$CONTENT$", html);

                File.WriteAllText(TempFile, html, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                TempFile = null;
                return;
            }
            webBrowser.Navigate(TempDocumentUrl);
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
                var element = webBrowser.Document.GetElementById(mdElement.Id);
                if (element != null)
                {
                    element.ScrollIntoView(true);
                }
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
