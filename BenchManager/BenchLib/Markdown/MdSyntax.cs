using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench.Markdown
{
    public static class MdSyntax
    {
        private static readonly string YamlHeaderStart = "---";
        private static readonly Regex YamlHeaderEndExp = new Regex("^---|...$");
        private static readonly Regex CodeBlockExp = new Regex("^(?<preamble>```+|~~~+)");
        private static readonly string HtmlCommentStart = "<!--";
        private static readonly string HtmlCommentEnd = "-->";

        public static bool IsYamlHeaderStart(int lineNo, string line)
        {
            if (lineNo > 0) return false;
            return line == YamlHeaderStart;
        }

        public static bool IsYamlHeaderEnd(int lineNo, string line)
        {
            if (lineNo == 0) return false;
            return YamlHeaderEndExp.IsMatch(line);
        }

        public static bool IsHtmlCommentStart(string line, int pos = 0)
        {
            var startP = line.LastIndexOf(HtmlCommentStart, pos);
            if (startP < 0) return false;
            return line.IndexOf(HtmlCommentEnd, startP) < 0;
        }

        public static bool IsHtmlCommentEnd(string line)
        {
            var endP = line.IndexOf(HtmlCommentEnd);
            if (endP < 0) return false;
            return !IsHtmlCommentStart(line, endP);
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
            if (line.Trim() == preamble)
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
