using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IGroupedPropertyTarget
    {
        void SetGroupCategory(string group, string category);

        void SetGroupValue(string group, string name, object value);
    }
}
