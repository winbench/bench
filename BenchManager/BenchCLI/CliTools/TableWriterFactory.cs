using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public static class TableWriterFactory
    {
        public static ITableWriter Create(DataOutputFormat format)
        {
            switch (format)
            {
                case DataOutputFormat.Plain:
                    return new ConsoleTableWriter();
                case DataOutputFormat.Markdown:
                    return new MarkdownTableWriter(Console.OpenStandardOutput());
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
