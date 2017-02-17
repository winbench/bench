using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench.Markdown
{
    internal static class MdSyntax
    {
        private static readonly string YamlHeaderStart = "---";
        private static readonly Regex YamlHeaderEndExp = new Regex(@"^-{3}|\.{3}$");
        private static readonly Regex CodeBlockExp = new Regex("^(?<preamble>`{3,}|~{3,})");
        private static readonly string HtmlCommentStart = "<!--";
        private static readonly string HtmlCommentEnd = "-->";

        public static bool IsYamlHeaderStart(int lineNo, string line)
        {
            if (lineNo > 0) return false;
            return string.Equals(line, YamlHeaderStart);
        }

        public static bool IsYamlHeaderEnd(int lineNo, string line)
        {
            if (lineNo == 0) return false;
            return YamlHeaderEndExp.IsMatch(line);
        }

        public static bool IsHtmlCommentStart(string line, int pos = 0)
        {
            var startP = line.LastIndexOf(HtmlCommentStart);
            if (startP < 0) return false;
            var endP = line.LastIndexOf(HtmlCommentEnd);
            if (endP < 0) return true;
            return startP > endP;
        }

        public static bool IsHtmlCommentEnd(string line)
        {
            var endP = line.LastIndexOf(HtmlCommentEnd);
            if (endP < 0) return false;
            var startP = line.LastIndexOf(HtmlCommentStart);
            if (startP < 0) return true;
            return endP > startP;
        }

        public static bool IsCodeBlockStart(string line, ref string preamble)
        {
            var m = CodeBlockExp.Match(line);
            if (m.Success)
            {
                preamble = m.Groups["preamble"].Value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsCodeBlockEnd(string line, ref string preamble)
        {
            if (string.Equals(line.Trim(), preamble))
            {
                preamble = null;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
