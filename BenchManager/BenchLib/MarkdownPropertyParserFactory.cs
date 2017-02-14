using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Mastersign.Bench.Markdown;

namespace Mastersign.Bench
{
    static class MarkdownPropertyParserFactory
    {
        public static MarkdownPropertyParser Create(IPropertyTarget target)
        {
            return new MarkdownPropertyParser
            {
                PropertyTarget = target,
                GroupBeginCue = null,
                GroupEndCue = null,
            };
        }

        public static MarkdownPropertyParser Create(IGroupedPropertyTarget target)
        {
            return new MarkdownPropertyParser
            {
                GroupPropertyTarget = target,
                GroupBeginCue = new Regex("^[\\*\\+-]\\s+ID:\\s*(`?)(?<group>\\S+?)\\1$"),
                GroupEndCue = new Regex("^\\s*$"),
                CollectGroupDocs = true,
            };
        }
    }
}
