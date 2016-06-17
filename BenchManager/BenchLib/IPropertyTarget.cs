using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IPropertyTarget
    {
        void SetValue(string name, object value);
    }
}
