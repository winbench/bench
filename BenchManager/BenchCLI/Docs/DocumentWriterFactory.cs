using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Docs
{
    public static class DocumentWriterFactory
    {
        public static DocumentWriter Create(DocumentOutputFormat format, Stream target = null)
        {
            switch (format)
            {
                case DocumentOutputFormat.Plain:
                    if (target == null)
                        return new PlainTextDocumentWriter(Console.Out) { UseConsoleColor = true };
                    else
                        return new PlainTextDocumentWriter(target);
                case DocumentOutputFormat.Markdown:
                    return new MarkdownDocumentWriter(target ?? Console.OpenStandardOutput());
                // case DocumentOutputFormat.Html:
                //    return new HtmlDocumentWriter(target);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
