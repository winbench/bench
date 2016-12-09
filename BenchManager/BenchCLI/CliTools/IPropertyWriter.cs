using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public interface IPropertyWriter : IDisposable
    {
        void Write(string key, object value);
    }
}
