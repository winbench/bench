using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Markdown
{
    internal enum MdContext
    {
        Inactive,
        YamlHeader,
        Text,
        CodeBlock,
        HtmlComment,
        List
    }
}
