using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Docs
{
    public delegate void DocumentContentGenerator<T>(DocumentWriter w, T a);
}
