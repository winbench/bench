using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class ResolvingPropertyCollection : GroupedPropertyCollection
    {
        private readonly List<IGroupedValueResolver> resolvers = new List<IGroupedValueResolver>();

        public void AddResolver(params IGroupedValueResolver[] resolvers)
        {
            this.resolvers.AddRange(resolvers);
        }

        protected override object ResolveValue(string name, object value)
        {
            foreach (var r in resolvers)
            {
                value = r.ResolveGroupValue(string.Empty, name, value);
            }
            return value;
        }

        protected override object ResolveGroupValue(string group, string name, object value)
        {
            foreach (var r in resolvers)
            {
                value = r.ResolveGroupValue(group, name, value);
            }
            return value;
        }
    }
}
