using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public static class MapWriterFactory
    {
        public static IMapWriter Create(DataOutputFormat format)
        {
            switch (format)
            {
                case DataOutputFormat.Plain:
                    return new ConsoleMapWriter();
                case DataOutputFormat.Markdown:
                    return new MarkdownMapWriter(Console.OpenStandardOutput());
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
