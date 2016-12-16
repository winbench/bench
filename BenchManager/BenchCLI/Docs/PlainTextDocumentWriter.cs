using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Docs
{
    public class PlainTextDocumentWriter : DocumentWriter, IDocumentWriter
    {
        private const string INDENT_1 = "  ";
        private const string INDENT_2 = "    ";

        private int listDepth = 0;

        public TextWriter writer;

        public bool UseConsoleColor { get; set; }

        public PlainTextDocumentWriter(Stream target)
        {
            writer = new StreamWriter(target, Encoding.Default);
        }

        public PlainTextDocumentWriter(TextWriter target = null)
        {
            writer = target ?? Console.Out;
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

        private readonly List<string> urls = new List<string>();
        private int lastUrl = 0;

        private bool indentFlag = false;
        private int breakCounter = 0;
        private bool bufferIndentFlag = false;
        private int bufferBreakCounter = 0;

        private const ConsoleColor RESET_COLOR = ConsoleColor.Gray;
        private const string ESCAPE = "\x12";
        private const char COLOR_OFFSET = 'A';
        private static Regex colorPattern = new Regex(ESCAPE + @"\w");

        private static string EncodeColor(ConsoleColor color)
        {
            return ESCAPE + ((char)(COLOR_OFFSET + color));
        }

        private static ConsoleColor DecodeColor(string escapedColor)
        {
            if (escapedColor.Length != 2) throw new ArgumentException();
            var colorNo = escapedColor[1] - COLOR_OFFSET;
            if (Enum.IsDefined(typeof(ConsoleColor), colorNo))
                return (ConsoleColor)colorNo;
            else
                return ConsoleColor.Gray;
        }

        private void WriteWithEscapedColors(string text)
        {
            var p = 0;
            foreach (Match m in colorPattern.Matches(text))
            {
                writer.Write(text.Substring(p, m.Index - p));
                if (UseConsoleColor) Console.ForegroundColor = DecodeColor(m.Value);
                p = m.Index + m.Length;
            }
            writer.Write(text.Substring(p));
        }

        private readonly Stack<ConsoleColor> colors = new Stack<ConsoleColor>();

        private void C(ConsoleColor color)
        {
            colors.Push(color);
            W(EncodeColor(color));
        }

        private void CR()
        {
            colors.Pop();
            if (colors.Count > 0)
                W(EncodeColor(colors.Peek()));
            else
                W(EncodeColor(RESET_COLOR));
        }

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
                    WriteWithEscapedColors(string.Format(format, args));
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
                case BlockType.Section:
                case BlockType.Property:
                    break;
                // DOCUMENT
                case BlockType.Document:
                    urls.Clear();
                    break;
                // INDENT
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
                case BlockType.Headline2:
                case BlockType.PropertyName:
                case BlockType.PropertyContent:
                    BeginBuffering();
                    break;
                // LINK
                case BlockType.Link:
                    lastUrl = -1;
                    C(ConsoleColor.White);
                    break;
                case BlockType.LinkContent:
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
                // SECTION
                case BlockType.Document:
                case BlockType.Section:
                    if (urls.Count > 0)
                    {
                        Begin(BlockType.Headline1);
                        Text("References");
                        End(BlockType.Headline1);
                        for (int i = 0; i < urls.Count; i++)
                        {
                            C(ConsoleColor.White);
                            W("{0,3})", i + 1);
                            CR();
                            W(" {0}", urls[i]);
                            BR();
                        }
                        urls.Clear();
                    }
                    BR();
                    break;
                // NEWLINE
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
                    C(ConsoleColor.Yellow);
                    W(text);
                    BR();
                    W(string.Empty.PadRight(text.Length, '='));
                    CR();
                    BR();
                    BR();
                    break;
                case BlockType.Headline1:
                    text = EndBuffering();
                    C(ConsoleColor.Yellow);
                    W(text);
                    BR();
                    W(string.Empty.PadRight(text.Length, '-'));
                    CR();
                    BR();
                    BR();
                    break;
                case BlockType.Headline2:
                    text = EndBuffering();
                    C(ConsoleColor.Yellow);
                    W(text);
                    W(":");
                    CR();
                    BR();
                    BR();
                    break;
                // LINK
                case BlockType.LinkContent:
                    break;
                case BlockType.Link:
                    if (lastUrl >= 0)
                    {
                        W(" [{0}]", lastUrl + 1);
                    }
                    CR();
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
                // IGNORE
                case InlineType.Anchor:
                    break;
                // PASS
                case InlineType.Text:
                    W(text);
                    break;
                // LINK TARGET
                case InlineType.LinkTarget:
                    if (!text.StartsWith("#"))
                    {
                        var i = urls.IndexOf(text);
                        if (i < 0)
                        {
                            urls.Add(text);
                            i = urls.Count - 1;
                        }
                        lastUrl = i;
                    }
                    break;
                // COLORED
                case InlineType.Keyword:
                    C(ConsoleColor.Cyan);
                    W(text);
                    CR();
                    break;
                case InlineType.Emphasized:
                    C(ConsoleColor.White);
                    W(text);
                    CR();
                    break;
                case InlineType.StronglyEmphasized:
                    C(ConsoleColor.Red);
                    W(text);
                    CR();
                    break;
                case InlineType.Code:
                    C(ConsoleColor.Green);
                    W(text);
                    CR();
                    break;
                // ADORN
                case InlineType.Variable:
                    C(ConsoleColor.Magenta);
                    W("<{0}>", text);
                    CR();
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
