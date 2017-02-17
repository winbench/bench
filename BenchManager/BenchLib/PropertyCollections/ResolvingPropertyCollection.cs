using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This class is an extension of the <see cref="PropertyCollection"/>,
    /// which allows to register mutliple property resolver.
    /// </summary>
    public class ResolvingPropertyCollection : PropertyCollection
    {
        private readonly List<IValueResolver> resolvers = new List<IValueResolver>();

        /// <summary>
        /// Registers a number of property resolvers.
        /// </summary>
        /// <param name="resolvers">The property resolvers to register.</param>
        public void AddResolver(params IValueResolver[] resolvers)
        {
            this.resolvers.AddRange(resolvers);
        }

        /// <summary>
        /// The implementation of <see cref="PropertyCollection.ResolveValue(string, object)"/>,
        /// calling all registered resolvers in the order they were registered.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The original value of the property.</param>
        /// <returns>The resolved or transformed value of the property.</returns>
        protected override object ResolveValue(string name, object value)
        {
            foreach (var r in resolvers)
            {
                value = r.ResolveValue(name, value);
            }
            return value;
        }
    }
}
