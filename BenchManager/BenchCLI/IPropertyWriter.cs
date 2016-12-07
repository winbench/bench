using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    interface IPropertyWriter : IDisposable
    {
        void Write(string key, object value);
    }
}
