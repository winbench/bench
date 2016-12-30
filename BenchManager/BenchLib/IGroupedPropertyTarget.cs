using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This interace describes the capability of storing values for grouped properties.
    /// The properties are identified by a group and a name unique in this group.
    /// </summary>
    public interface IGroupedPropertyTarget
    {
        /// <summary>
        /// Marks a group with a category.
        /// </summary>
        /// <param name="group">The group to be marked.</param>
        /// <param name="category">The new category for the group.</param>
        void SetGroupCategory(string group, string category);

        /// <summary>
        /// Attaches a metadata object to a group.
        /// </summary>
        /// <param name="group">The group to attach the metadata to.</param>
        /// <param name="metadata">The metadata object.</param>
        void SetGroupMetadata(string group, object metadata);

        /// <summary>
        /// Sets the value of the specified property.
        /// If the property did exist until now, it is created.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new value for the property.</param>
        void SetGroupValue(string group, string name, object value);
    }
}
