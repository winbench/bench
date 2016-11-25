using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    class MemoryDocument : IDocumentWriter
    {
        #region Element Classes

        interface IDocumentElement
        {
            void WriteTo(IDocumentWriter w);
        }

        class TextElement : IDocumentElement
        {
            public string Text { get; private set; }
            public TextElement(string format, params object[] args)
            {
                Text = string.Format(format, args);
            }
            public virtual void WriteTo(IDocumentWriter w) { w.Text(Text); }
        }

        class DocumentBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginDocument(); }
        }

        class DocumentEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndDocument(); }
        }

        class SectionBeginElement : TextElement, IDocumentElement
        {
            public SectionBeginElement(string format, params object[] args)
                : base(format, args)
            { }
            public override void WriteTo(IDocumentWriter w) { w.BeginSection(Text); }
        }

        class SectionEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndSection(); }
        }

        class ParagraphBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginParagraph(); }
        }

        class ParagraphEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndParagraph(); }
        }

        class LineBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginLine(); }
        }

        class LineEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndLine(); }
        }

        class ListBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginList(); }
        }

        class ListEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndList(); }
        }

        class ListItemBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginListItem(); }
        }

        class ListItemEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndListItem(); }
        }

        class SyntaxListBeginElement : TextElement, IDocumentElement
        {
            public SyntaxListBeginElement(string format, params object[] args)
                : base(format, args)
            { }
            public override void WriteTo(IDocumentWriter w) { w.BeginSyntaxList(this.Text); }
        }

        class SyntaxListEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndSyntaxList(); }
        }

        class SyntaxListItemBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginSyntaxListItem(); }
        }

        class SyntaxListItemEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndSyntaxListItem(); }
        }

        class DetailBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginDetail(); }
        }

        class DetailEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndDetail(); }
        }

        class DefinitionListBeginElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.BeginDefinitionList(); }
        }

        class DefinitionListEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndDefinitionList(); }
        }

        class DefinitionBeginElement : TextElement, IDocumentElement
        {
            public DefinitionBeginElement(string format, params object[] args)
                : base(format, args)
            { }
            public override void WriteTo(IDocumentWriter w) { w.BeginDefinition(Text); }
        }

        class DefinitionEndElement : IDocumentElement
        {
            public void WriteTo(IDocumentWriter w) { w.EndDefinition(); }
        }

        class TitleElement : TextElement, IDocumentElement
        {
            public TitleElement(string format, params object[] args)
                : base(format, args)
            { }
            public override void WriteTo(IDocumentWriter w) { w.Title(Text); }
        }

        class SyntaxElementElement : TextElement, IDocumentElement
        {
            public SyntaxElementElement(string format, params object[] args)
                : base(format, args)
            { }
            public override void WriteTo(IDocumentWriter w) { w.SyntaxElement(Text); }
        }

        class KeywordElement : TextElement, IDocumentElement
        {
            public KeywordElement(string format, params object[] args)
                : base(format, args)
            { }
            public override void WriteTo(IDocumentWriter w) { w.Keyword(Text); }
        }

        class VariableElement : TextElement, IDocumentElement
        {
            public VariableElement(string format, params object[] args)
                : base(format, args)
            { }
            public override void WriteTo(IDocumentWriter w) { w.Variable(Text); }
        }

        #endregion

        private readonly List<IDocumentElement> elements = new List<IDocumentElement>();

        private void Record(IDocumentElement e)
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

        public void BeginDocument() { Record(new DocumentBeginElement()); }

        public void EndDocument() { Record(new DocumentEndElement()); }

        public void BeginSection(string format, params object[] args)
        {
            Record(new SectionBeginElement(format, args));
        }

        public void EndSection() { Record(new SectionEndElement()); }

        public void Title(string format, params object[] args)
        {
            Record(new TitleElement(format, args));
        }

        public void BeginParagraph() { Record(new ParagraphBeginElement()); }

        public void EndParagraph() { Record(new ParagraphEndElement()); }

        public void BeginLine() { Record(new LineBeginElement()); }

        public void EndLine() { Record(new LineEndElement()); }

        public void Line(string format, params object[] args)
        {
            Record(new LineBeginElement());
            Record(new TextElement(format, args));
            Record(new LineEndElement());
        }

        public void BeginList() { Record(new ListBeginElement()); }

        public void EndList() { Record(new ListEndElement()); }

        public void BeginListItem() { Record(new ListItemBeginElement()); }

        public void EndListItem() { Record(new ListItemEndElement()); }

        public void ListItem(string format, params object[] args)
        {
            Record(new ListItemBeginElement());
            Record(new TextElement(format, args));
            Record(new ListItemEndElement());
        }

        public void BeginSyntaxList(string format, params object[] args)
        {
            Record(new SyntaxListBeginElement(format, args));
        }

        public void EndSyntaxList() { Record(new SyntaxListEndElement()); }

        public void BeginSyntaxListItem() { Record(new SyntaxListItemBeginElement()); }

        public void EndSyntaxListItem() { Record(new SyntaxListItemEndElement()); }

        public void BeginDetail() { Record(new DetailBeginElement()); }

        public void EndDetail() { Record(new DetailEndElement()); }

        public void BeginDefinitionList() { Record(new DefinitionListBeginElement()); }

        public void EndDefinitionList() { Record(new DefinitionListEndElement()); }

        public void BeginDefinition(string format, params object[] args)
        {
            Record(new DefinitionBeginElement(format, args));
        }

        public void EndDefinition() { Record(new DefinitionEndElement()); }

        public void Definition(string key, string format, params object[] args)
        {
            Record(new DefinitionBeginElement(key));
            Record(new TextElement(format, args));
            Record(new DefinitionEndElement());
        }

        public void Text(string format, params object[] args)
        {
            Record(new TextElement(format, args));
        }

        public void SyntaxElement(string format, params object[] args)
        {
            Record(new SyntaxElementElement(format, args));
        }

        public void Keyword(string format, params object[] args)
        {
            Record(new KeywordElement(format, args));
        }

        public void Variable(string format, params object[] args)
        {
            Record(new VariableElement(format, args));
        }
    }
}
