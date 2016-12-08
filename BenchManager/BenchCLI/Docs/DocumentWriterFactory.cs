using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Docs
{
    public static class DocumentWriterFactory
    {
        public static DocumentWriter Create(DocumentOutputFormat format, Stream target)
        {
            switch (format)
            {
                case DocumentOutputFormat.Plain:
                    return new PlainTextDocumentWriter(target);
                // case DocumentOutputFormat.Markdown:
                //    return new MarkdownDocumentWriter(target);
                // case DocumentOutputFormat.Html:
                //    return new HtmlDocumentWriter(target);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
