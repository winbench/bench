using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mastersign.Bench.Dashboard.Properties;
using System.IO;
using Mastersign.Bench.Markdown;
using System.Diagnostics;

namespace Mastersign.Bench.Dashboard
{
    public partial class MarkdownControl : UserControl
    {
        private readonly static string template;

        static MarkdownControl()
        {
            template = Resources.MarkdownViewerTemplate.Replace("$CSS$", Resources.MarkdownViewerStyle);
        }

        private string tempFile;

        private Uri TempDocumentUrl { get { return new Uri("file:///" + TempFile); } }

        public MarkdownControl()
        {
            InitializeComponent();
            Disposed += MarkdownControl_Disposed;
            webBrowser.Navigating += WebBrowser_Navigating;
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // force opening URLs in System browser
        }

        private void MarkdownControl_Disposed(object sender, EventArgs e)
        {
            TempFile = null;
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

        private string NewTempFilePath()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".html");
        }

        public MarkdownToHtmlConverter ShowMarkdownText(string text, string title)
        {
            TempFile = NewTempFilePath();
            var md2html = new MarkdownToHtmlConverter();
            try
            {
                string html;
                using (var r = new StringReader(text))
                {
                    html = md2html.ConvertToHtml(r);
                }
                html = template.Replace("$TITLE$", title).Replace("$CONTENT$", html);

                File.WriteAllText(TempFile, html, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                TempFile = null;
                return null;
            }
            webBrowser.Navigate(TempDocumentUrl);
            return md2html;
        }

        public MarkdownToHtmlConverter ShowMarkdownFile(string filePath, string title = null)
        {
            TempFile = NewTempFilePath();
            title = title ?? Path.GetFileNameWithoutExtension(filePath);
            var md2html = new MarkdownToHtmlConverter();
            try
            {
                string html;
                using (var s = File.Open(filePath, FileMode.Open, FileAccess.Read))
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
                return null;
            }
            webBrowser.Navigate(TempDocumentUrl);
            return md2html;
        }

        public void ScrollToElement(string id)
        {
            var element = webBrowser.Document.GetElementById(id);
            if (element != null)
            {
                element.ScrollIntoView(true);
            }
        }
    }
}
