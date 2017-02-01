using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Docs
{
    public abstract class DocumentWriter : IDocumentWriter
    {
        #region IDocumentWriter

        void IDocumentWriter.Begin(BlockType type)
        {
            Begin(type);
        }

        void IDocumentWriter.End(BlockType type)
        {
            End(type);
        }

        void IDocumentWriter.Inline(InlineType type, string text)
        {
            Inline(type, text);
        }

        void IDocumentWriter.LineBreak()
        {
            LineBreak();
        }

        #endregion

        #region Abstract Methods

        public abstract DocumentWriter End(BlockType type);

        public abstract DocumentWriter Begin(BlockType type);

        public abstract DocumentWriter Inline(InlineType type, string format, params object[] args);

        public abstract DocumentWriter LineBreak();

        public abstract void Dispose();

        #endregion

        #region Convenience Methods

        public DocumentWriter Append(IDocumentElements elements)
        {
            elements.WriteTo(this);
            return this;
        }

        public DocumentWriter Append<T>(DocumentContentGenerator<T> generator, T arg)
        {
            generator(this, arg);
            return this;
        }

        public DocumentWriter AppendList(string separator, params IDocumentElements[] elements)
        {
            var first = true;
            foreach (var e in elements)
            {
                if (!first) Text(separator);
                first = false;
                e.WriteTo(this);
            }
            return this;
        }

        public DocumentWriter AppendList(IDocumentElements separator, params IDocumentElements[] elements)
        {
            var first = true;
            foreach (var e in elements)
            {
                if (!first) separator.WriteTo(this);
                first = false;
                e.WriteTo(this);
            }
            return this;
        }

        public DocumentWriter AppendList<T>(string separator, DocumentContentGenerator<T> generator, params T[] args)
        {
            var first = true;
            foreach (var a in args)
            {
                if (!first) Text(separator);
                first = false;
                generator(this, a);
            }
            return this;
        }

        public DocumentWriter AppendList<T>(IDocumentElements separator, DocumentContentGenerator<T> generator, params T[] args)
        {
            var first = true;
            foreach (var a in args)
            {
                if (!first) separator.WriteTo(this);
                first = false;
                generator(this, a);
            }
            return this;
        }

        public DocumentWriter AppendList<T>(string separator, DocumentContentGenerator<T> generator, IEnumerable<T> args)
        {
            var first = true;
            foreach (var a in args)
            {
                if (!first) Text(separator);
                first = false;
                generator(this, a);
            }
            return this;
        }

        public DocumentWriter AppendList<T>(IDocumentElements separator, DocumentContentGenerator<T> generator, IEnumerable<T> args)
        {
            var first = true;
            foreach (var a in args)
            {
                if (!first) separator.WriteTo(this);
                first = false;
                generator(this, a);
            }
            return this;
        }

        public DocumentWriter Block(BlockType type, string format, params object[] args)
        {
            Begin(type);
            Text(format, args);
            End(type);
            return this;
        }

        public DocumentWriter Block(BlockType type, IDocumentElements e)
        {
            Begin(type);
            e.WriteTo(this);
            End(type);
            return this;
        }

        public DocumentWriter Block<T>(BlockType type, DocumentContentGenerator<T> generator, T arg)
        {
            Begin(type);
            generator(this, arg);
            End(type);
            return this;
        }

        public DocumentWriter Title(string format, params object[] args)
        {
            return Block(BlockType.Title, format, args);
        }

        public DocumentWriter Title(IDocumentElements e)
        {
            return Block(BlockType.Title, e);
        }

        public DocumentWriter Title<T>(DocumentContentGenerator<T> generator, T arg)
        {
            return Block(BlockType.Title, generator, arg);
        }

        public DocumentWriter Headline1(string anchor, string format, params object[] args)
        {
            Begin(BlockType.Headline1);
            Anchor(anchor);
            Text(format, args);
            End(BlockType.Headline1);
            return this;
        }

        public DocumentWriter Headline1(string anchor, IDocumentElements e)
        {
            Begin(BlockType.Headline1);
            Anchor(anchor);
            Append(e);
            End(BlockType.Headline1);
            return this;
        }

        public DocumentWriter Headline1<T>(string anchor, DocumentContentGenerator<T> generator, T arg)
        {
            Begin(BlockType.Headline1);
            Anchor(anchor);
            Append(generator, arg);
            End(BlockType.Headline1);
            return this;
        }

        public DocumentWriter Headline2(string anchor, string format, params object[] args)
        {
            Begin(BlockType.Headline2);
            Anchor(anchor);
            Text(format, args);
            End(BlockType.Headline2);
            return this;
        }

        public DocumentWriter Headline2(string anchor, IDocumentElements e)
        {
            Begin(BlockType.Headline2);
            Anchor(anchor);
            Append(e);
            End(BlockType.Headline2);
            return this;
        }

        public DocumentWriter Headline2<T>(string anchor, DocumentContentGenerator<T> generator, T arg)
        {
            Begin(BlockType.Headline2);
            Anchor(anchor);
            Append(generator, arg);
            End(BlockType.Headline2);
            return this;
        }

        public DocumentWriter Paragraph(string format, params object[] args)
        {
            return Block(BlockType.Paragraph, format, args);
        }

        public DocumentWriter Paragraph(IDocumentElements content)
        {
            return Block(BlockType.Paragraph, content);
        }

        public DocumentWriter Paragraph<T>(DocumentContentGenerator<T> generator, T arg)
        {
            return Block(BlockType.Paragraph, generator, arg);
        }

        public DocumentWriter ListItem(string format, params object[] args)
        {
            return Block(BlockType.ListItem, format, args);
        }

        public DocumentWriter ListItem(IDocumentElements content)
        {
            return Block(BlockType.ListItem, content);
        }

        public DocumentWriter ListItem<T>(DocumentContentGenerator<T> generator, T arg)
        {
            return Block(BlockType.ListItem, generator, arg);
        }

        public DocumentWriter DefinitionTopic(string format, params object[] args)
        {
            return Block(BlockType.DefinitionTopic, format, args);
        }

        public DocumentWriter DefinitionTopic(IDocumentElements content)
        {
            return Block(BlockType.DefinitionTopic, content);
        }

        public DocumentWriter DefinitionTopic<T>(DocumentContentGenerator<T> generator, T arg)
        {
            return Block(BlockType.DefinitionTopic, generator, arg);
        }

        public DocumentWriter DefinitionContent(string format, params object[] args)
        {
            return Block(BlockType.DefinitionContent, format, args);
        }

        public DocumentWriter DefinitionContent(IDocumentElements content)
        {
            return Block(BlockType.DefinitionContent, content);
        }

        public DocumentWriter DefinitionContent<T>(DocumentContentGenerator<T> generator, T arg)
        {
            return Block(BlockType.DefinitionContent, generator, arg);
        }

        public DocumentWriter Definition(string topic, string format, params object[] args)
        {
            Begin(BlockType.Definition);
            DefinitionTopic(topic);
            DefinitionContent(format, args);
            End(BlockType.Definition);
            return this;
        }

        public DocumentWriter Definition(string topic, IDocumentElements content)
        {
            Begin(BlockType.Definition);
            DefinitionTopic(topic);
            DefinitionContent(content);
            End(BlockType.Definition);
            return this;
        }

        public DocumentWriter Definition<T>(string topic, DocumentContentGenerator<T> generator, T arg)
        {
            Begin(BlockType.Definition);
            DefinitionTopic(topic);
            DefinitionContent(generator, arg);
            End(BlockType.Definition);
            return this;
        }

        public DocumentWriter PropertyName(string format, params object[] args)
        {
            return Block(BlockType.PropertyName, format, args);
        }

        public DocumentWriter PropertyName(IDocumentElements content)
        {
            return Block(BlockType.PropertyName, content);
        }

        public DocumentWriter PropertyName<T>(DocumentContentGenerator<T> generator, T arg)
        {
            return Block(BlockType.PropertyName, generator, arg);
        }

        public DocumentWriter PropertyContent(string format, params object[] args)
        {
            return Block(BlockType.PropertyContent, format, args);
        }

        public DocumentWriter PropertyContent(IDocumentElements content)
        {
            return Block(BlockType.PropertyContent, content);
        }

        public DocumentWriter PropertyContent<T>(DocumentContentGenerator<T> generator, T arg)
        {
            return Block(BlockType.PropertyContent, generator, arg);
        }

        public DocumentWriter Property(string name, string format, params object[] args)
        {
            Begin(BlockType.Property);
            PropertyName(name);
            PropertyContent(format, args);
            End(BlockType.Property);
            return this;
        }

        public DocumentWriter Property(string name, IDocumentElements content)
        {
            Begin(BlockType.Property);
            PropertyName(name);
            PropertyContent(content);
            End(BlockType.Property);
            return this;
        }

        public DocumentWriter Property<T>(string name, DocumentContentGenerator<T> generator, T arg)
        {
            Begin(BlockType.Property);
            PropertyName(name);
            PropertyContent(generator, arg);
            End(BlockType.Property);
            return this;
        }

        public DocumentWriter Link(string href, string format, params object[] args)
        {
            Begin(BlockType.Link);
            LinkTarget(href);
            Block(BlockType.LinkContent, format, args);
            End(BlockType.Link);
            return this;
        }

        public DocumentWriter Link(string href, IDocumentElements content)
        {
            Begin(BlockType.Link);
            LinkTarget(href);
            Block(BlockType.LinkContent, content);
            End(BlockType.Link);
            return this;
        }

        public DocumentWriter Link<T>(string href, DocumentContentGenerator<T> generator, T arg)
        {
            Begin(BlockType.Link);
            LinkTarget(href);
            Block(BlockType.LinkContent, generator, arg);
            End(BlockType.Link);
            return this;
        }

        public DocumentWriter Text(string format, params object[] args)
            => Inline(InlineType.Text, format, args);

        public DocumentWriter Emph(string format, params object[] args)
            => Inline(InlineType.Emphasized, format, args);

        public DocumentWriter Strong(string format, params object[] args)
            => Inline(InlineType.StronglyEmphasized, format, args);

        public DocumentWriter Code(string format, params object[] args)
            => Inline(InlineType.Code, format, args);

        public DocumentWriter Keyword(string format, params object[] args)
            => Inline(InlineType.Keyword, format, args);

        public DocumentWriter Variable(string format, params object[] args)
            => Inline(InlineType.Variable, format, args);

        public DocumentWriter Anchor(string format, params object[] args)
            => Inline(InlineType.Anchor, format, args);

        public DocumentWriter LinkTarget(string format, params object[] args)
            => Inline(InlineType.LinkTarget, format, args);

        #endregion
    }
}
