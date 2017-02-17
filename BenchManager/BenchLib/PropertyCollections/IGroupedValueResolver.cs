using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This interface describes an object, that can resolve or transform a the values of group properties.
    /// </summary>
    public interface IGroupedValueResolver
    {
        /// <summary>
        /// Returns the resolved or transformed value of the specified property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The original value of the specified property.</param>
        /// <returns>The resolved or transformed value for the specified value.</returns>
        object ResolveGroupValue(string group, string name, object value);
    }
}
