using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IGroupedPropertySource
    {
        object GetGroupValue(string group, string name);

        bool CanGetGroupValue(string group, string name);
    }
}
