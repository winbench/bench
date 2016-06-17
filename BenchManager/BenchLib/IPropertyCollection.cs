using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IPropertyCollection : IPropertySource, IPropertyTarget
    {
        IEnumerable<string> PropertyNames();

        object GetValue(string name, object def);

        bool ContainsValue(string name);
    }
}
