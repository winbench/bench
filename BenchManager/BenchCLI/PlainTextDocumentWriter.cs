using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench.Cli
{
    class PlainTextDocumentWriter : IDocumentWriter
    {
        private const string INDENT_1 = "  ";
        private const string INDENT_2 = "    ";
        public readonly TextWriter Target;

        public PlainTextDocumentWriter(TextWriter target)
        {
            Target = target;
        }

        private enum WriteMode
        {
            Target,
            DefinitionItem
        }

        private WriteMode writeMode = WriteMode.Target;

        private void W(string format, params object[] args)
        {
            switch (writeMode)
            {
                case WriteMode.DefinitionItem:
                    AddToCurrentDefinition(string.Format(format, args));
                    break;
                default:
                    Target.Write(format, args);
                    break;
            }
        }

        private void WL()
        {
            switch (writeMode)
            {
                case WriteMode.DefinitionItem:
                    AddToCurrentDefinition(Environment.NewLine);
                    break;
                default:
                    Target.WriteLine();
                    break;
            }
        }

        private void WL(string format, params object[] args)
        {
            switch (writeMode)
            {
                case WriteMode.DefinitionItem:
                    AddToCurrentDefinition(string.Format(format, args) + Environment.NewLine);
                    break;
                default:
                    Target.WriteLine(format, args);
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
            foreach (var i in indentStack) { W(i); }
        }

        public void BeginDocument() { }

        public void EndDocument() { }

        public void Title(string format, params object[] args)
        {
            var txt = string.Format(format, args);
            WL(txt);
            WL("".PadLeft(txt.Length, '='));
            WL();
        }

        public void BeginSection(string format, params object[] args)
        {
            var txt = string.Format(format, args);
            WL(txt);
            WL("".PadLeft(txt.Length, '-'));
            WL();
        }

        public void EndSection() { }

        public void BeginParagraph() { }

        public void EndParagraph()
        {
            Target.WriteLine();
        }

        public void BeginLine()
        {
            Indent();
        }

        public void EndLine() { WL(); }

        public void Line(string format, params object[] args)
        {
            Indent();
            WL(format, args);
        }

        public void BeginSyntaxList(string format, params object[] args)
        {
            WL(format + ":", args);
            WL();
            PushIndent(INDENT_1);
        }

        public void EndSyntaxList()
        {
            PopIndent();
            WL();
        }

        public void BeginSyntaxListItem()
        {
            Indent();
        }

        public void EndSyntaxListItem()
        {
            WL();
        }

        public void BeginDetail()
        {
            PushIndent(INDENT_2);
        }

        public void EndDetail()
        {
            PopIndent();
        }

        private struct DefinitionItem
        {
            public string Key;
            public string Description;
        }

        private List<DefinitionItem> definitionItems;

        private DefinitionItem CurrentDefinitionItem
        {
            get { return definitionItems[definitionItems.Count - 1]; }
            set { definitionItems[definitionItems.Count - 1] = value; }
        }

        private DefinitionItem PopCurrentDefinitionItem()
        {
            var res = CurrentDefinitionItem;
            definitionItems.RemoveAt(definitionItems.Count - 1);
            return res;
        }

        private void AddToCurrentDefinition(string txt)
        {
            var current = CurrentDefinitionItem;
            CurrentDefinitionItem = new DefinitionItem
            {
                Key = current.Key,
                Description = current.Description + txt
            };
        }

        public void BeginDefinitionList()
        {
            definitionItems = new List<DefinitionItem>();
        }

        public void EndDefinitionList()
        {
            int maxKeyLength = 0;
            foreach (var item in definitionItems)
            {
                if (item.Key.Length > maxKeyLength)
                {
                    maxKeyLength = item.Key.Length;
                }
            }
            foreach (var item in definitionItems)
            {
                Indent();
                W((item.Key + ":").PadRight(maxKeyLength + 2));
                var lines = item.Description.Split(
                    new string[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0) Indent();
                    WL(lines[i]);
                }
            }
            definitionItems = null;
            WL();
        }

        public void BeginDefinition(string format, params object[] args)
        {
            definitionItems.Add(new DefinitionItem
            {
                Key = string.Format(format, args)
            });
            writeMode = WriteMode.DefinitionItem;
        }

        public void EndDefinition()
        {
            writeMode = WriteMode.Target;
        }

        public void Definition(string key, string format, params object[] args)
        {
            definitionItems.Add(new DefinitionItem
            {
                Key = key,
                Description = string.Format(format, args)
            });
        }

        public void BeginList() { }

        public void EndList() { WL(); }

        public void BeginListItem()
        {
            Indent();
            W("* ");
        }

        public void EndListItem() { WL(); }

        public void ListItem(string format, params object[] args)
        {
            Indent();
            W("* ");
            WL(format, args);
        }

        public void Text(string format, params object[] args)
        {
            W(format, args);
        }

        public void SyntaxElement(string format, params object[] args)
        {
            W(format, args);
        }

        public void Keyword(string format, params object[] args)
        {
            W(format, args);
        }

        public void Variable(string format, params object[] args)
        {
            W("<");
            W(format, args);
            W(">");
        }
    }
}
