using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This interface describes an object, which stores properties.
    /// It is a combination of <see cref="IPropertySource"/> and <see cref="IPropertyTarget"/>.
    /// </summary>
    public interface IPropertyCollection : IPropertySource, IPropertyTarget
    {
        /// <summary>
        /// Gets the names from all existing properties.
        /// </summary>
        /// <returns>An enumeration of strings.</returns>
        IEnumerable<string> PropertyNames();

        /// <summary>
        /// Gets the value of the specified property, or a given default value,
        /// in case the specified property does not exist.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>The value of the specified property, or <paramref name="def"/>
        /// in case the specified value does not exist.</returns>
        object GetValue(string name, object def);

        /// <summary>
        /// Checks, whether this collection contains the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns><c>true</c> if the property exists; otherwise <c>false</c>.</returns>
        /// <seealso cref="IPropertySource.CanGetValue(string)"/>
        bool ContainsValue(string name);
    }
}
