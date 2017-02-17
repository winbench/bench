using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This interface describes an object which is capable of storing grouped properties.
    /// Additionally it provides helper methods, to support type safe access to a limited number of simple types.
    /// </summary>
    public interface IObjectLibrary : IGroupedPropertyCollection
    {
        /// <summary>
        /// Sets a string value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string value for the property.</param>
        void SetGroupValue(string group, string name, string value);

        /// <summary>
        /// Sets a string array value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string array value for the property.</param>
        void SetGroupValue(string group, string name, string[] value);

        /// <summary>
        /// Sets a boolean value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new boolean value for the property.</param>
        void SetGroupValue(string group, string name, bool value);

        /// <summary>
        /// Sets an integer value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new integer value for the property.</param>
        void SetGroupValue(string group, string name, int value);

        /// <summary>
        /// Gets the value of a group property as a string.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string or <c>null</c> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string GetStringGroupValue(string group, string name);

        /// <summary>
        /// Gets the value of a group property as a string, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string GetStringGroupValue(string group, string name, string def);

        /// <summary>
        /// Gets the value of a group property as a string array.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string array, that may be empty if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string[] GetStringListGroupValue(string group, string name);

        /// <summary>
        /// Gets the value of a group property as a string array, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string array or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string[] GetStringListGroupValue(string group, string name, string[] def);

        /// <summary>
        /// Gets the value of a group property as a boolean.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>A boolean or <c>false</c> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        bool GetBooleanGroupValue(string group, string name);

        /// <summary>
        /// Gets the value of a group property as a boolean, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A boolean or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        bool GetBooleanGroupValue(string group, string name, bool def);

        /// <summary>
        /// Gets the value of a group property as an integer.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>An integer or <c>0</c> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        int GetInt32GroupValue(string group, string name);

        /// <summary>
        /// Gets the value of a group property as an integer, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>An integer or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        int GetInt32GroupValue(string group, string name, int def);
    }
}
