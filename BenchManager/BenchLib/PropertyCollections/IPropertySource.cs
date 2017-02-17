using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This interface describes the capability of reading properties.
    /// The properties are named with a unique string.
    /// </summary>
    public interface IPropertySource
    {
        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the specified property, or <c>null</c> 
        /// if the property does not exist.</returns>
        object GetValue(string name);

        /// <summary>
        /// Checks, whether this object can retrieve the value for the specified property, or not.
        /// </summary>
        /// <param name="name">The name of the property in question.</param>
        /// <returns><c>true</c> if this object can get the value for specified property;
        /// otherwise <c>false</c>.</returns>
        /// <remarks>
        /// Even when this method returns <c>true</c>,
        /// it may be the case, that <see cref="GetValue(string)"/> returns <c>null</c>,
        /// because the property exists, but the value of the property is <c>null</c>.
        /// </remarks>
        bool CanGetValue(string name);
    }
}
