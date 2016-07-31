using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This interface describes the capability of reading grouped properties.
    /// The properties are identified by a group and a name unique in this group.
    /// </summary>
    public interface IGroupedPropertySource
    {
        /// <summary>
        /// Gets the category of the specified group, or <c>null</c> if the group has no category.
        /// </summary>
        /// <param name="group">The group in question.</param>
        /// <returns>The category name of the given group, or <c>null</c>.</returns>
        string GetGroupCategory(string group);

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the specified property, or <c>null</c> 
        /// if the property does not exist.</returns>
        object GetGroupValue(string group, string name);

        /// <summary>
        /// Checks, whether this object can retrieve the value for the specified property, or not.
        /// </summary>
        /// <param name="group">The group of the property in question.</param>
        /// <param name="name">The name of the property in question.</param>
        /// <returns><c>true</c> if this object can get the value for specified property;
        /// otherwise <c>false</c>.</returns>
        /// <remarks>
        /// Even when this method returns <c>true</c>,
        /// it may be the case, that <see cref="GetGroupValue(string,string)"/> returns <c>null</c>,
        /// because the property exists, but the value of the property is <c>null</c>.
        /// </remarks>
        bool CanGetGroupValue(string group, string name);
    }
}
