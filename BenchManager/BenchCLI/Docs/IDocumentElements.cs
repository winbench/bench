using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Docs
{
    public interface IDocumentElements
    {
        void WriteTo(IDocumentWriter writer);
    }
}
