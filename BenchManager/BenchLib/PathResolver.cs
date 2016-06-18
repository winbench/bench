using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench
{
    public class PathResolver : IGroupedValueResolver
    {
        public PropertyCriteria Selector { get; set; }

        public BasePathSource BasePathSource { get; set; }

        public PathResolver()
        {
        }

        public PathResolver(PropertyCriteria selector, BasePathSource basePathSource)
            : this()
        {
            Selector = selector;
            BasePathSource = basePathSource;
        }

        public object ResolveGroupValue(string group, string name, object value)
        {
            if (value == null) return null;
            if (Selector == null || !Selector(group, name)) return value;
            if (value is string[])
            {
                return Array.ConvertAll((string[])value, v => (string)ResolveGroupValue(group, name, v));
            }
            if (value is string)
            {
                var path = (string)value;
                if (!Path.IsPathRooted(path) && BasePathSource != null)
                {
                    value = Path.Combine(BasePathSource(group, name), path);
                }
            }
            return value;
        }
    }
}
