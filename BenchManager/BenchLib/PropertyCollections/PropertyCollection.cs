using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mastersign.Bench.PropertyCollections
{
    /// <summary>
    /// This class is the default implementation for <see cref="IConfiguration"/>.
    /// It can be cascaded by setting a <see cref="DefaultValueSource"/> which is used in case
    /// this instance does not contain a specified property.
    /// </summary>
    public class PropertyCollection : IConfiguration
    {
        private readonly List<string> propertyNames = new List<string>(); // ordered list for group names
        private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        /// <summary>
        /// The backup value source for ungrouped properties.
        /// </summary>
        public IPropertySource DefaultValueSource { get; set; }

        /// <summary>
        /// Deletes all properties in this collection.
        /// </summary>
        public void Clear()
        {
            propertyNames.Clear();
            properties.Clear();
        }

        /// <summary>
        /// Checks, whether this collection contains the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns><c>true</c> if the property exists; otherwise <c>false</c>.</returns>
        /// <seealso cref="IPropertySource.CanGetValue(string)"/>
        public bool ContainsValue(string name) => properties.ContainsKey(name ?? string.Empty);


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
        public bool CanGetValue(string name) => ContainsValue(name)
                || DefaultValueSource != null && DefaultValueSource.CanGetValue(name);

        /// <summary>
        /// Sets a string value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string value for the property.</param>
        public void SetValue(string name, string value) => InternalSetValue(name, value);

        /// <summary>
        /// Sets a string array value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string array value for the property.</param>
        public void SetValue(string name, string[] value) => InternalSetValue(name, value);

        /// <summary>
        /// Sets a boolean value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new boolean value for the property.</param>
        public void SetValue(string name, bool value) => InternalSetValue(name, value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Sets an integer value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new integer value for the property.</param>
        public void SetValue(string name, int value) => InternalSetValue(name, value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Sets the value of the specified property.
        /// If the property did exist until now, it is created.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new value of the property.</param>
        public void SetValue(string name, object value) => InternalSetValue(name, value);

        /// <summary>
        /// Resets the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public void ResetValue(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (name.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "The property name must not be empty.");
            }
            if (properties.ContainsKey(name))
            {
                propertyNames.Remove(name);
                properties.Remove(name);
            }
        }

        private void InternalSetValue(string propertyName, object value)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), "The property name must not be empty.");
            }
            if (properties.ContainsKey(propertyName))
            {
                properties[propertyName] = value;
            }
            else
            {
                propertyNames.Add(propertyName);
                properties.Add(propertyName, value);
            }
        }

        private object InternalGetRawValue(string propertyName, out bool found)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), "The property name must not be empty.");
            }
            object value;
            if (properties.TryGetValue(propertyName, out value))
            {
                found = true;
                return value;
            }
            found = false;
            return null;
        }

        private object InternalGetValue(string propertyName, out bool found, object def = null)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), "The property name must not be empty.");
            }
            object value;
            if (properties.TryGetValue(propertyName, out value))
            {
                if (value != null)
                {
                    found = true;
                    return ResolveValue(propertyName, value);
                }
            }
            found = false;
            if (DefaultValueSource != null &&
                DefaultValueSource.CanGetValue(propertyName))
            {
                return ResolveValue(propertyName,
                    DefaultValueSource.GetValue(propertyName));
            }
            return def;
        }

        /// <summary>
        /// Gets the unresolved and untransformed value of a property in this collection,
        /// without looking up the property in <see cref="DefaultValueSource"/>.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the specified property or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Is thrown
        /// if <c>null</c> is passed for <paramref name="name"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown 
        /// if an empty string is passed as <paramref name="name"/>.</exception>
        public object GetRawValue(string name)
        {
            bool found;
            return InternalGetRawValue(name, out found);
        }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the specified property, or <c>null</c> 
        /// if the property does not exist.</returns>
        public object GetValue(string name) => GetValue(name, null);

        /// <summary>
        /// Gets the value of the specified property, or a given default value,
        /// in case the specified property does not exist.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>The value of the specified property, or <paramref name="def"/>
        /// in case the specified value does not exist.</returns>
        public object GetValue(string name, object def)
        {
            bool found;
            return InternalGetValue(name, out found, def);
        }

        /// <summary>
        /// Gets the value of a property as a string.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string, or <c>null</c> if the property does not exist
        /// or its value can not be properly converted.</returns>
        public string GetStringValue(string name) => GetStringValue(name, null);

        /// <summary>
        /// Gets the value of a property as a string, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string GetStringValue(string name, string def)
        {
            bool found;
            return InternalGetValue(name, out found, def) as string;
        }

        /// <summary>
        /// Gets the value of a property as a string array.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string array, that may be empty, if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string[] GetStringListValue(string name) => GetStringListValue(name, null);

        /// <summary>
        /// Gets the value of a property as a string array, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string array or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string[] GetStringListValue(string name, string[] def)
        {
            bool found;
            var value = InternalGetValue(name, out found, def);
            if (value is string) value = new string[] { (string)value };
            return (value as string[]) ?? new string[0];
        }

        /// <summary>
        /// Gets the value of a property as a boolean.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A boolean, or <c>false</c> if the property does not exist
        /// or its value can not be properly converted.</returns>
        public bool GetBooleanValue(string name) => GetBooleanValue(name, false);

        /// <summary>
        /// Gets the value of a property as a boolean, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A boolean or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public bool GetBooleanValue(string name, bool def)
        {
            bool found;
            var value = InternalGetValue(name, out found, def);
            if (value is bool)
            {
                return (bool)value;
            }
            if (value is string)
            {
                bool result;
                if (Boolean.TryParse((string)value, out result))
                {
                    return result;
                }
            }
            return def;
        }

        /// <summary>
        /// Gets the value of a property as an integer.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>An integer, or <c>0</c> if the property does not exist
        /// or its value can not be properly converted.</returns>
        public int GetInt32Value(string name) => GetInt32Value(name, 0);

        /// <summary>
        /// Gets the value of a property as an integer, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>An integer or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public int GetInt32Value(string name, int def)
        {
            bool found;
            var value = InternalGetValue(name, out found, def);
            if (value is string)
            {
                int result;
                if (Int32.TryParse((string)value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }
            }
            return def;
        }

        /// <summary>
        /// This method is a hook for child classes, 
        /// to implement some kind of value resolution or transformation for ungrouped properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The original value of the property.</param>
        /// <returns>The resolved or transformed value.</returns>
        protected virtual object ResolveValue(string name, object value) => value;

        /// <summary>
        /// Gets the names from all existing properties.
        /// </summary>
        /// <returns>An enumeration of strings.</returns>
        public IEnumerable<string> PropertyNames() => propertyNames;

        /// <summary>
        /// Returns a string represenation of this property collection.
        /// </summary>
        /// <returns>A string containing all properties and thier values.</returns>
        public override string ToString() => ToString(true);

        /// <summary>
        /// Returns a string represenation of this property collection.
        /// </summary>
        /// <param name="resolve">A flag, controlling the resolution of property values.</param>
        /// <returns>A string containing all properties and thier values.</returns>
        public string ToString(bool resolve)
        {
            var sb = new StringBuilder();
            foreach (var k in propertyNames)
            {
                sb.AppendLine(string.Format("* {0}: {1}", k, FormatValue(k, properties[k], resolve)));
            }
            return sb.ToString().TrimStart();
        }

        private string FormatValue(string name, object val, bool resolve = true)
        {
            if (resolve)
            {
                val = ResolveValue(name, val);
            }
            if (val == null)
            {
                return "null";
            }
            if (val is bool)
            {
                return (bool)val ? "`true`" : "`false`";
            }
            if (val is string)
            {
                return string.Format("`{0}`", val);
            }
            if (val.GetType().IsArray)
            {
                var a = (Array)val;
                var f = new string[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    f[i] = FormatValue(name, a.GetValue(i), false);
                }
                return "List( " + string.Join(", ", f) + " )";
            }
            if (val is IDictionary)
            {
                var d = (IDictionary)val;
                var l = new List<string>(d.Count);
                foreach (var k in d.Keys)
                {
                    l.Add(string.Format("`{0}: {1}`", k, d[k]));
                }
                return "Dict( " + string.Join(", ", l.ToArray()) + " )";
            }
            return "Object( " + val.ToString() + " )";
        }

    }
}
