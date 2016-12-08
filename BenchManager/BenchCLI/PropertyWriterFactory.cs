using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    static class PropertyWriterFactory
    {
        public static IPropertyWriter Create(DataOutputFormat format)
        {
            switch (format)
            {
                case DataOutputFormat.Plain:
                    return new ConsolePropertyWriter();
                case DataOutputFormat.Markdown:
                    return new MarkdownPropertyWriter(Console.OpenStandardOutput());
                //case OutputFormat.JSON:
                //    return new JsonPropertyWriter(Console.OpenStandardOutput());
                //case OutputFormat.XML:
                //    return new XmlPropertyWriter(Console.OpenStandardOutput());
                default:
                    throw new NotSupportedException();
            }
        }

    }
}
