using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench.Markdown
{
    public static class MarkdownPropertyEditor
    {
        private const string PatternTemplate = @"^{0}\s+((?:~~)?)\s*{1}\s*:\s*{2}\s*\1\s*$";
        private const string PropertyTemplate = @"* {0}: `{1}`";
        private const string UnquotedPropertyTemplate = @"* {0}: {1}";

        public static void UpdateFile(string file, IDictionary<string, string> dict)
        {
            var lines = new List<string>(File.ReadAllLines(file, Encoding.UTF8));
            foreach(var k in dict.Keys)
            {
                SetProperty(lines, k, dict[k]);
            }
            File.WriteAllLines(file, lines.ToArray(), Encoding.UTF8);
        }

        private static void SetProperty(IList<string> lines, string key, string value)
        {
            bool found = false;
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                found = UpdateLine(ref line, key, value) || found;
                lines[i] = line;
            }
            if (!found)
            {
                lines.Add(FormatLine(key, value));
            }
        }

        private static bool UpdateLine(ref string line, string key, string value)
        {
            var listStart = @"[\*\+-]";
            var keyPattern = Regex.Escape(key);
            var valuePattern = @".*?";
            var pattern = new Regex(
                string.Format(
                    PatternTemplate,
                    listStart, keyPattern, valuePattern));

            if (pattern.IsMatch(line))
            {
                line = FormatLine(key, value);
                return true;
            }
            return false;
        }

        private static string FormatLine(string key, string value)
        {
            return string.Format(
                value.Contains("`") ? UnquotedPropertyTemplate : PropertyTemplate,
                key, value);
        }
    }
}
