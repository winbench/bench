using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IConfigurationPart
    {
        void Transfer(IDictionary<string, string> dict);
    }
}
