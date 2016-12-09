using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Docs
{
    public class MarkdownDocumentWriter : DocumentWriter, IDocumentWriter
    {
        private int listDepth = 0;

        public TextWriter writer;

        public MarkdownDocumentWriter(Stream target)
        {
            writer = new StreamWriter(target, Encoding.Default);
        }

        public override void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        private enum WriteMode
        {
            Target,
            Buffer,
        }

        private WriteMode writeMode = WriteMode.Target;

        private StringBuilder buffer = new StringBuilder();

        private void BeginBuffering()
        {
            writeMode = WriteMode.Buffer;
        }

        private string EndBuffering()
        {
            var text = buffer.ToString();
            writeMode = WriteMode.Target;
            buffer.Remove(0, buffer.Length);
            bufferIndentFlag = false;
            bufferBreakCounter = 0;
            return text;
        }

        private bool indentFlag = false;
        private int breakCounter = 0;
        private bool bufferIndentFlag = false;
        private int bufferBreakCounter = 0;

        private string Escape(string v)
        {
            return v
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("*", "\\*");
        }

        private void W(string format, params object[] args)
        {
            var t = string.Format(format, args);
            switch (writeMode)
            {
                case WriteMode.Buffer:
                    bufferBreakCounter = 0;
                    buffer.Append(t);
                    break;
                default:
                    breakCounter = 0;
                    writer.Write(t);
                    break;
            }
        }

        private void BR()
        {
            switch (writeMode)
            {
                case WriteMode.Buffer:
                    if (bufferBreakCounter > 1) return;
                    bufferBreakCounter++;
                    bufferIndentFlag = false;
                    buffer.AppendLine();
                    break;
                default:
                    if (breakCounter > 1) return;
                    breakCounter++;
                    indentFlag = false;
                    writer.WriteLine();
                    break;
            }
        }

        private readonly List<string> indentStack = new List<string>();

        private void PushIndent(string i)
        {
            indentStack.Add(i);
        }

        private void PopIndent()
        {
            if (indentStack.Count > 0)
            {
                indentStack.RemoveAt(indentStack.Count - 1);
            }
        }

        private void Indent()
        {
            switch (writeMode)
            {
                case WriteMode.Buffer:
                    if (bufferIndentFlag) return;
                    bufferIndentFlag = true;
                    break;
                default:
                    if (indentFlag) return;
                    indentFlag = true;
                    break;
            }
            foreach (var i in indentStack) { W(i); }
        }

        private string ListPrefix()
        {
            if (listDepth < 2) return "* ";
            if (listDepth < 3) return "+ ";
            return "- ";
        }

        public override DocumentWriter Begin(BlockType type)
        {
            switch (type)
            {
                // IGNORE
                case BlockType.Document:
                case BlockType.Section:
                case BlockType.DefinitionList:
                case BlockType.Definition:
                case BlockType.Property:
                    break;
                // INDENT
                case BlockType.Paragraph:
                    Indent();
                    break;
                // CHANGE INDENT
                case BlockType.PropertyList:
                    break;
                case BlockType.DefinitionContent:
                case BlockType.Detail:
                    Indent();
                    break;
                // BUFFER
                case BlockType.Title:
                    BeginBuffering();
                    break;
                // PREFIX
                case BlockType.Headline1:
                    W("## ");
                    break;
                case BlockType.Headline2:
                    W("### ");
                    break;
                case BlockType.DefinitionTopic:
                    W("#### ");
                    break;
                // LIST
                case BlockType.List:
                    BR();
                    listDepth++;
                    break;
                case BlockType.ListItem:
                    Indent();
                    var prefix = ListPrefix();
                    W(prefix);
                    PushIndent(string.Empty.PadRight(4));
                    break;
                // BUFFER
                case BlockType.PropertyName:
                case BlockType.PropertyContent:
                    BeginBuffering();
                    break;
                // UNSUPPORTED
                default:
                    throw new NotSupportedException();
            }
            return this;
        }

        public override DocumentWriter End(BlockType type)
        {
            string text;
            switch (type)
            {
                // IGNORE
                case BlockType.Document:
                    break;
                // NEWLINE
                case BlockType.Section:
                case BlockType.Definition:
                case BlockType.DefinitionTopic:
                    BR();
                    break;
                // EMPTY LINE
                case BlockType.Headline1:
                case BlockType.Headline2:
                case BlockType.Paragraph:
                    BR();
                    BR();
                    break;
                // Headline
                case BlockType.Title:
                    text = EndBuffering();
                    W(text);
                    BR();
                    W(string.Empty.PadRight(text.Length, '='));
                    BR();
                    BR();
                    break;
                // LIST
                case BlockType.List:
                    listDepth--;
                    if (listDepth == 0) BR();
                    break;
                case BlockType.ListItem:
                    PopIndent();
                    if (indentFlag) BR();
                    break;
                // PROPERTY TABLE
                case BlockType.PropertyName:
                    W("{0}: ", EndBuffering());
                    break;
                case BlockType.PropertyContent:
                    W(EndBuffering());
                    break;
                case BlockType.Property:
                    W("  ");
                    BR();
                    break;
                case BlockType.PropertyList:
                    BR();
                    break;
                // INDENT
                case BlockType.DefinitionList:
                case BlockType.DefinitionContent:
                case BlockType.Detail:
                    PopIndent();
                    BR();
                    break;
                // UNSUPPORTED
                default:
                    throw new NotSupportedException();
            }
            return this;
        }

        public override DocumentWriter Inline(InlineType type, string format, params object[] args)
        {
            var text = string.Format(format, args);
            switch (type)
            {
                // PASS
                case InlineType.Text:
                    W(Escape(text));
                    break;
                // ADORN
                case InlineType.Keyword:
                    W("`{0}`", Escape(text));
                    break;
                case InlineType.Syntactic:
                    W("{0}", Escape(text));
                    break;
                case InlineType.Variable:
                    W("_&lt;{0}&gt;_", Escape(text));
                    break;
                // UNSUPPORTED
                default:
                    throw new NotSupportedException();
            }
            return this;
        }

        public override DocumentWriter LineBreak()
        {
            BR();
            Indent();
            return this;
        }

    }
}
