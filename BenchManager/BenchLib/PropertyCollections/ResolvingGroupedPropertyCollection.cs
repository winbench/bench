using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This class is an extension of the <see cref="GroupedPropertyCollection"/>,
    /// which allows to register mutliple property resolver.
    /// </summary>
    public class ResolvingGroupedPropertyCollection : GroupedPropertyCollection
    {
        private readonly List<IGroupedValueResolver> resolvers = new List<IGroupedValueResolver>();

        /// <summary>
        /// Registers a number of property resolvers.
        /// </summary>
        /// <param name="resolvers">The property resolvers to register.</param>
        public void AddResolver(params IGroupedValueResolver[] resolvers)
        {
            this.resolvers.AddRange(resolvers);
        }

        /// <summary>
        /// The implementation of <see cref="GroupedPropertyCollection.ResolveGroupValue(string, string, object)"/>,
        /// calling all registered resolvers in the order they were registered.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The original value of the property.</param>
        /// <returns>The resolved or transformed value of the property.</returns>
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
