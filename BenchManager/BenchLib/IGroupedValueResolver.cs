using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IGroupedValueResolver
    {
        object ResolveGroupValue(string group, string name, object value);
    }
}
