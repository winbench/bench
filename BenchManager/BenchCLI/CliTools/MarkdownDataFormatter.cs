using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.CliTools
{
    //class MarkdownDataFormatter : IDataFormatter
    //{
    //    private readonly TextWriter writer;

    //    private enum Container { None, Root, List, Map, MapItem }

    //    private Stack<Container> stack = new Stack<Container>();

    //    public MarkdownDataFormatter(TextWriter writer)
    //    {
    //        this.writer = writer;
    //        stack.Push(Container.None);
    //        stack.Push(Container.Root);
    //    }

    //    private bool CanWriteValue
    //    {
    //        get
    //        {
    //            var c = stack.Peek();
    //            return c == Container.Root
    //                || c == Container.List
    //                || c == Container.MapItem;
    //        }
    //    }

    //    public void Value(string value)
    //    {
    //        if (!CanWriteValue) throw new InvalidOperationException();
    //        if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
    //            writer.Write("<" + value + ">");
    //        else
    //            writer.Write("`" + value + "`");
    //        if (stack.Peek() == Container.Root) stack.Pop();
    //    }

    //    public void Value(bool value)
    //    {
    //        if (!CanWriteValue) throw new InvalidOperationException();
    //        writer.Write(value ? "`true`" : "`false`");
    //        if (stack.Peek() == Container.Root) stack.Pop();
    //    }

    //    public void Null()
    //    {
    //        if (!CanWriteValue) throw new InvalidOperationException();
    //        if (stack.Peek() == Container.Root) stack.Pop();
    //    }

    //    public void BeginList()
    //    {
    //        if (!CanWriteValue) throw new InvalidOperationException();
    //        var c = stack.Peek();
    //        stack.Push(Container.List);
    //    }

    //    public void EndList()
    //    {
    //        stack.Pop();
    //    }

    //    public void BeginMap()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void BeginMapItem(string key)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void EndMapItem()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void EndMap()
    //    {
    //        throw new NotImplementedException();
    //    }
    // }
}
