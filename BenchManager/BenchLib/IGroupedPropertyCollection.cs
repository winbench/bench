using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This interface describes an object, which stores grouped properties.
    /// It is a combination of <see cref="IGroupedPropertySource"/> and <see cref="IGroupedPropertyTarget"/>.
    /// </summary>
    public interface IGroupedPropertyCollection : IGroupedPropertySource, IGroupedPropertyTarget
    {
        /// <summary>
        /// Gets the groups in this collection.
        /// </summary>
        /// <returns>An enumeration of group names.</returns>
        IEnumerable<string> Groups();

        /// <summary>
        /// Gets all groups, marked with the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>An enumeration of group names.</returns>
        IEnumerable<string> GroupsByCategory(string category);

        /// <summary>
        /// Gets the property names in the specified group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <returns>An enumeration of property names.</returns>
        IEnumerable<string> PropertyNames(string group);

        /// <summary>
        /// Gets the value of the specified property, or a given default value,
        /// in case the specified property does not exist.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>The value of the specified property, or <paramref name="def"/>
        /// in case the specified value does not exist.</returns>
        object GetGroupValue(string group, string name, object def);

        /// <summary>
        /// Checks, whether this collection contains properties in the specified group.
        /// </summary>
        /// <param name="group">The name of the group.</param>
        /// <returns><c>true</c> if properties in the specified group exists;
        /// otherwise <c>false</c>.</returns>
        /// <seealso cref="IPropertySource.CanGetValue(string)"/>
        bool ContainsGroup(string group);

        /// <summary>
        /// Checks, whether this collection contains the specified property in the specified group.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns><c>true</c> if this collection contains the specified property;
        /// otherwise <c>false</c>.</returns>
        /// <seealso cref="IPropertySource.CanGetValue(string)"/>
        bool ContainsGroupValue(string group, string name);
    }
}
