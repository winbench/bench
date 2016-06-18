using System;
using System.Collections.Generic;

namespace Mastersign.Bench.Test
{
    class PropertyCollection : IPropertyCollection
    {
        private readonly IDictionary<string, object> properties;

        public PropertyCollection(IDictionary<string, object> properties = null)
        {
            this.properties = properties ?? new Dictionary<string, object>();
        }

        public bool CanGetValue(string name)
        {
            return ContainsValue(name);
        }

        public bool ContainsValue(string name)
        {
            return properties.ContainsKey(name);
        }

        public object GetValue(string name)
        {
            return GetValue(name, null);
        }

        public object GetValue(string name, object def)
        {
            object v;
            return properties.TryGetValue(name, out v)
                ? v : def;
        }

        public IEnumerable<string> PropertyNames()
        {
            return properties.Keys;
        }

        public void SetValue(string name, object value)
        {
            properties[name] = value;
        }
    }
}
