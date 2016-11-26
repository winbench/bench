using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Docs
{

    public interface IDocumentWriter
    {
        void Begin(BlockType typ);

        void End(BlockType typ);

        void Inline(InlineType typ, string text);

        void LineBreak();
    }
}
