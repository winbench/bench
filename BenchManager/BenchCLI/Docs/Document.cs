using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Docs
{
    public class Document : DocumentWriter, IDocumentWriter, IDocumentElements
    {
        #region Element Classes

        public abstract class Element
        {
            public abstract void WriteTo(IDocumentWriter writer);
        }

        public class BlockBeginElement : Element
        {
            public BlockType BlockType { get; private set; }

            public BlockBeginElement(BlockType type) { BlockType = type; }

            public override void WriteTo(IDocumentWriter writer)
            {
                writer.Begin(BlockType);
            }
        }

        public class BlockEndElement : Element
        {
            public BlockType BlockTyp { get; private set; }

            public BlockEndElement(BlockType typ) { BlockTyp = typ; }

            public override void WriteTo(IDocumentWriter writer)
            {
                writer.End(BlockTyp);
            }
        }

        public class InlineElement : Element
        {
            public InlineType InlineType { get; private set; }

            public string Text { get; private set; }

            public InlineElement(InlineType type, string text)
            {
                InlineType = type;
                Text = text;
            }

            public override void WriteTo(IDocumentWriter writer)
            {
                writer.Inline(InlineType, Text);
            }
        }

        public class LineBreakElement : Element
        {
            public override void WriteTo(IDocumentWriter writer)
            {
                writer.LineBreak();
            }
        }

        #endregion

        public Document() { }

        public Document(IDocumentElements elements)
        {
            elements.WriteTo(this);
        }

        public override void Dispose()
        {
            // nothing
        }

        private readonly List<Element> elements = new List<Element>();

        private void Record(Element e)
        {
            elements.Add(e);
        }

        public bool IsEmpty { get { return elements.Count == 0; } }

        public void Clear() { elements.Clear(); }

        public void WriteTo(IDocumentWriter writer)
        {
            foreach (var e in elements)
            {
                e.WriteTo(writer);
            }
        }

        public override DocumentWriter Begin(BlockType type)
        {
            Record(new BlockBeginElement(type));
            return this;
        }

        public override DocumentWriter End(BlockType type)
        {
            Record(new BlockEndElement(type));
            return this;
        }

        public override DocumentWriter Inline(InlineType type, string format, params object[] args)
        {
            Record(new InlineElement(type, string.Format(format, args)));
            return this;
        }

        public override DocumentWriter LineBreak()
        {
            Record(new LineBreakElement());
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            using (var w = new StringWriter(sb))
            using (var dw = new PlainTextDocumentWriter(w))
            {
                WriteTo(dw);
            }
            return sb.ToString();
        }
    }
}
