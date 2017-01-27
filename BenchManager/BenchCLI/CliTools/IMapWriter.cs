using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public interface IMapWriter : IDisposable
    {
        void Write(string key, object value);
    }
}
