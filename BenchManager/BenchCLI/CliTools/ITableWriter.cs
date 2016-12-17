using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public interface ITableWriter : IDisposable
    {
        void Initialize(params string[] columns);

        void Write(params object[] values);
    }
}
