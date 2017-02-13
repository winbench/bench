using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench
{
    internal class GroupedPropertyPathResolver : IGroupedValueResolver
    {
        public GroupedPropertyCriteria Selector { get; set; }

        public GroupedPropertyBasePathSource BasePathSource { get; set; }

        public GroupedPropertyPathResolver()
        {
        }

        public GroupedPropertyPathResolver(GroupedPropertyCriteria selector, GroupedPropertyBasePathSource basePathSource)
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
                if (!Path.IsPathRooted(path) && BasePathSource != null && BasePathSource(group, name) != null)
                {
                    value = Path.Combine(BasePathSource(group, name), path);
                }
            }
            return value;
        }
    }
}
