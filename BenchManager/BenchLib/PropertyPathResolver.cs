using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.Bench.PropertyCollections;

namespace Mastersign.Bench
{
    internal class PropertyPathResolver : IValueResolver
    {
        public PropertyCriteria Selector { get; set; }

        public PropertyBasePathSource BasePathSource { get; set; }

        public PropertyPathResolver()
        {
        }

        public PropertyPathResolver(PropertyCriteria selector, PropertyBasePathSource basePathSource)
            : this()
        {
            Selector = selector;
            BasePathSource = basePathSource;
        }

        public object ResolveValue(string name, object value)
        {
            if (value == null) return null;
            if (Selector == null || !Selector(name)) return value;
            if (value is string[])
            {
                return Array.ConvertAll((string[])value, v => (string)ResolveValue(name, v));
            }
            if (value is string)
            {
                var path = (string)value;
                if (!Path.IsPathRooted(path) && BasePathSource != null && BasePathSource(name) != null)
                {
                    value = Path.Combine(BasePathSource(name), path);
                }
            }
            return value;
        }
    }
}
