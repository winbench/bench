using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Docs
{
    public class PlainTextDocumentWriter : DocumentWriter, IDocumentWriter
    {
        private const string INDENT_1 = "  ";
        private const string INDENT_2 = "    ";

        private int listDepth = 0;

        public TextWriter writer;

        public PlainTextDocumentWriter(Stream target)
        {
            writer = new StreamWriter(target, Encoding.Default);
        }

        public PlainTextDocumentWriter(TextWriter target)
        {
            writer = target;
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

        private void W(string format, params object[] args)
        {
            switch (writeMode)
            {
                case WriteMode.Buffer:
                    bufferBreakCounter = 0;
                    buffer.Append(string.Format(format, args));
                    break;
                default:
                    breakCounter = 0;
                    writer.Write(format, args);
#if DEBUG
                    writer.Flush();
#endif
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
#if DEBUG
                    writer.Flush();
#endif
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
            if (listDepth < 2) return " *  ";
            if (listDepth < 3) return " +  ";
            return " -  ";
        }

        public override DocumentWriter Begin(BlockType type)
        {
            switch (type)
            {
                // IGNORE
                case BlockType.Document:
                case BlockType.Section:
                case BlockType.Property:
                    break;
                // INDENT
                case BlockType.Headline2:
                case BlockType.Paragraph:
                case BlockType.Definition:
                case BlockType.DefinitionTopic:
                    Indent();
                    break;
                // CHANGE INDENT
                case BlockType.DefinitionList:
                    PushIndent(INDENT_1);
                    break;
                case BlockType.DefinitionContent:
                case BlockType.Detail:
                    PushIndent(INDENT_2);
                    Indent();
                    break;
                // BUFFER
                case BlockType.Title:
                case BlockType.Headline1:
                case BlockType.PropertyName:
                case BlockType.PropertyContent:
                    BeginBuffering();
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
                    PushIndent(string.Empty.PadRight(prefix.Length));
                    break;
                // PROPERTY TABLE
                case BlockType.PropertyList:
                    properties = new List<PropertyItem>();
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
                case BlockType.Property:
                    BR();
                    break;
                // EMPTY LINE
                case BlockType.Paragraph:
                    BR();
                    BR();
                    break;
                // HEADLINES
                case BlockType.Title:
                    text = EndBuffering();
                    W(text);
                    BR();
                    W(string.Empty.PadRight(text.Length, '='));
                    BR();
                    BR();
                    break;
                case BlockType.Headline1:
                    text = EndBuffering();
                    W(text);
                    BR();
                    W(string.Empty.PadRight(text.Length, '-'));
                    BR();
                    BR();
                    break;
                case BlockType.Headline2:
                    W(":");
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
                    PushProperty(EndBuffering());
                    break;
                case BlockType.PropertyContent:
                    UpdateCurrentProperty(EndBuffering());
                    break;
                case BlockType.PropertyList:
                    int maxKeyLength = MaximalPropertyKeyLength();
                    foreach (var item in properties)
                    {
                        Indent();
                        W((item.Key + ":").PadRight(maxKeyLength + 2));
                        var lines = item.Description.Split(
                            new string[] { Environment.NewLine }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (i > 0)
                            {
                                BR();
                                Indent();
                                W(string.Empty.PadRight(maxKeyLength + 2));
                            }
                            W(lines[i]);
                        }
                        BR();
                    }
                    properties = null;
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
                case InlineType.Keyword:
                case InlineType.Syntactic:
                    W(text);
                    break;
                // ADORN
                case InlineType.Variable:
                    W("<{0}>", text);
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

        private struct PropertyItem
        {
            public string Key;
            public string Description;
        }

        private List<PropertyItem> properties;

        private PropertyItem CurrentProperty
        {
            get { return properties[properties.Count - 1]; }
            set { properties[properties.Count - 1] = value; }
        }

        private PropertyItem PopCurrentProperty()
        {
            var res = CurrentProperty;
            properties.RemoveAt(properties.Count - 1);
            return res;
        }

        public void PushProperty(string text)
        {
            properties.Add(new PropertyItem { Key = text });
        }

        public void UpdateCurrentProperty(string description)
        {
            var p = CurrentProperty;
            CurrentProperty = new PropertyItem
            {
                Key = p.Key,
                Description = description
            };
        }

        private int MaximalPropertyKeyLength()
        {
            int maxKeyLength = 0;
            foreach (var item in properties)
            {
                if (item.Key.Length > maxKeyLength)
                {
                    maxKeyLength = item.Key.Length;
                }
            }

            return maxKeyLength;
        }
    }
}
