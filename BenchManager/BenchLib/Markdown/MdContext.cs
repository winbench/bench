using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Markdown
{
    public enum MdContext
    {
        Inactive,
        YamlHeader,
        Text,
        CodeBlock,
        HtmlComment,
        List
    }
}
