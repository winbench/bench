using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This interface describes an object which is capable of storing properties.
    /// Additionally it provides helper methods, to support type safe access to a limited number of simple types.
    /// </summary>
    public interface IConfiguration : IPropertyCollection
    {
        /// <summary>
        /// Sets a string value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string value for the property.</param>
        void SetValue(string name, string value);

        /// <summary>
        /// Sets a string array value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string array value for the property.</param>
        void SetValue(string name, string[] value);

        /// <summary>
        /// Sets a boolean value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new boolean value for the property.</param>
        void SetValue(string name, bool value);

        /// <summary>
        /// Sets an integer value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new integer value for the property.</param>
        void SetValue(string name, int value);

        /// <summary>
        /// Gets the value of a property as a string.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string or <c>null</c> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string GetStringValue(string name);

        /// <summary>
        /// Gets the value of a property as a string, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string GetStringValue(string name, string def);

        /// <summary>
        /// Gets the value of a property as a string array.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string array, that may be empty, if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string[] GetStringListValue(string name);

        /// <summary>
        /// Gets the value of a property as a string array, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string array or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        string[] GetStringListValue(string name, string[] def);

        /// <summary>
        /// Gets the value of a property as a boolean.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A boolean, or <c>false</c> if the property does not exist
        /// or its value can not be properly converted.</returns>
        bool GetBooleanValue(string name);

        /// <summary>
        /// Gets the value of a property as a boolean, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A boolean or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        bool GetBooleanValue(string name, bool def);

        /// <summary>
        /// Gets the value of a property as an integer.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>An integer, or <c>0</c> if the property does not exist
        /// or its value can not be properly converted.</returns>
        int GetInt32Value(string name);

        /// <summary>
        /// Gets the value of a property as an integer, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>An integer or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        int GetInt32Value(string name, int def);
    }
}
