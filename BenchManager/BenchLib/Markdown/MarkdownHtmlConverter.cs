using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MarkdownSharp.Extensions;

namespace Mastersign.Bench.Markdown
{
    public class MarkdownToHtmlConverter
    {
        private static readonly Regex HeadlinePattern = new Regex("^(?<hashes>#+)\\s+(?<text>.+?)\\s*(?:\\{.*?\\})?\\s*\\1?$");
        private static readonly Regex AnchorParagraphPattern = new Regex(@"\<p\>(?<anchor>\<a\s+name="".*?""\s*\>\</a\>)\</p\>\s*");

        private StringBuilder Output;
        private int LineNo;
        private string CodePreamble;

        public IList<MdAnchor> Anchors;

        private MdContext Context;

        public string ConvertToHtml(Stream source)
        {
            return ConvertToHtml(new StreamReader(source, Encoding.UTF8));
        }

        public string ConvertToHtml(TextReader source)
        {
            Output = new StringBuilder();
            LineNo = 0;
            CodePreamble = null;
            Anchors = new List<MdAnchor>();
            Context = MdContext.Text;
            string line = null;
            while ((line = source.ReadLine()) != null)
            {
                ProcessLine(line);
                LineNo++;
            }
            ProcessLine("");
            return ConvertToHtml(Output.ToString());
        }

        private string ConvertToHtml(string source)
        {
            var md = new MarkdownSharp.Markdown();
            md.AddExtension(new GitHubCodeBlockExtension());
            var html = md.Transform(source);
            html = CleanHtml(html);
            return html;
        }

        private static string CleanHtml(string source)
        {
            return AnchorParagraphPattern.Replace(source, m =>
            {
                return m.Groups["anchor"].Value;
            });
        }

        private void ProcessLine(string line)
        {
            switch (Context)
            {
                case MdContext.Inactive:
                    if (MdSyntax.IsYamlHeaderStart(LineNo, line))
                        Context = MdContext.YamlHeader;
                    else if (MdSyntax.IsHtmlCommentStart(line))
                        Context = MdContext.HtmlComment;
                    else if (MdSyntax.IsCodeBlockStart(line, ref CodePreamble))
                        Context = MdContext.CodeBlock;
                    break;
                case MdContext.YamlHeader:
                    if (MdSyntax.IsYamlHeaderEnd(LineNo, line))
                        Context = MdContext.Text;
                    break;
                case MdContext.HtmlComment:
                    if (MdSyntax.IsHtmlCommentEnd(line))
                        Context = MdContext.Text;
                    break;
                case MdContext.CodeBlock:
                    if (MdSyntax.IsCodeBlockEnd(line, ref CodePreamble))
                        Context = MdContext.Text;
                    break;
                case MdContext.Text:
                    if (MdSyntax.IsYamlHeaderStart(LineNo, line))
                        Context = MdContext.YamlHeader;
                    else if (MdSyntax.IsHtmlCommentStart(line))
                        Context = MdContext.HtmlComment;
                    else if (MdSyntax.IsCodeBlockStart(line, ref CodePreamble))
                        Context = MdContext.CodeBlock;
                    else
                        ProcessTextLine(line);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Output.AppendLine(line);
        }

        private void ProcessTextLine(string line)
        {
            var m = HeadlinePattern.Match(line);
            if (m.Success)
            {
                var anchor = string.Format("line_{0:0000}", LineNo);
                Output.AppendLine(string.Format("<a name=\"{0}\"></a>", anchor));
                Anchors.Add(new MdHeadline(anchor,
                    m.Groups["text"].Value,
                    m.Groups["hashes"].Value.Length));
            }
        }
    }
}
