using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class is the default implementation for <see cref="IConfiguration"/>.
    /// It can be cascaded by setting a <see cref="DefaultValueSource"/>
    /// and a <see cref="GroupedDefaultValueSource"/> which are used in case
    /// this instance does not contain a specified property.
    /// </summary>
    public class GroupedPropertyCollection : IConfiguration
    {
        private readonly List<string> groupNames = new List<string>(); // ordered list for group names
        private readonly Dictionary<string, string> groupCategories = new Dictionary<string, string>();
        private readonly Dictionary<string, object> groupMetadata = new Dictionary<string, object>();
        private readonly Dictionary<string, List<string>> groupKeys = new Dictionary<string, List<string>>(); // ordered lists for property names
        private readonly Dictionary<string, Dictionary<string, object>> groups = new Dictionary<string, Dictionary<string, object>>();

        /// <summary>
        /// The backup value source for ungrouped properties.
        /// </summary>
        public IPropertySource DefaultValueSource { get; set; }

        /// <summary>
        /// The back value source for group properties.
        /// </summary>
        public IGroupedPropertySource GroupedDefaultValueSource { get; set; }

        /// <summary>
        /// Deletes all properties in this collection.
        /// </summary>
        public void Clear()
        {
            groupNames.Clear();
            groupCategories.Clear();
            groupKeys.Clear();
            groups.Clear();
        }

        /// <summary>
        /// Marks a group with a category.
        /// </summary>
        /// <param name="group">The group to be marked.</param>
        /// <param name="category">The new category for the group.</param>
        public void SetGroupCategory(string group, string category)
        {
            group = group ?? string.Empty;
            groupCategories[group] = category;
        }

        /// <summary>
        /// Gets the category of the specified group, or <c>null</c> if the group has no category.
        /// </summary>
        /// <param name="group">The group in question.</param>
        /// <returns>The category name of the given group, or <c>null</c>.</returns>
        public string GetGroupCategory(string group)
        {
            string category;
            return groupCategories.TryGetValue(group, out category) ? category : null;
        }

        /// <summary>
        /// Attaches a metadata object to a group.
        /// </summary>
        /// <param name="group">The group to attach the metadata to.</param>
        /// <param name="metadata">The metadata object.</param>
        public void SetGroupMetadata(string group, object metadata)
        {
            group = group ?? string.Empty;
            groupMetadata[group] = metadata;
        }

        /// <summary>
        /// Gets the metadata object, attached to the specified group, 
        /// or <c>null</c> if the group has no metadata attached.
        /// </summary>
        /// <param name="group">The group in question.</param>
        /// <returns>The metadata object attached to the given group, or <c>null</c>.</returns>
        public object GetGroupMetadata(string group)
        {
            object metadata;
            return groupMetadata.TryGetValue(group, out metadata) ? metadata : null;
        }

        /// <summary>
        /// Checks, whether this collection contains properties in the specified group.
        /// </summary>
        /// <param name="group">The name of the group.</param>
        /// <returns><c>true</c> if properties in the specified group exists;
        /// otherwise <c>false</c>.</returns>
        /// <seealso cref="IPropertySource.CanGetValue(string)"/>
        public bool ContainsGroup(string group)
        {
            group = group ?? string.Empty;
            return groups.ContainsKey(group);
        }

        /// <summary>
        /// Checks, whether this collection contains the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns><c>true</c> if the property exists; otherwise <c>false</c>.</returns>
        /// <seealso cref="IPropertySource.CanGetValue(string)"/>
        public bool ContainsValue(string name) { return ContainsGroupValue(null, name); }

        /// <summary>
        /// Checks, whether this collection contains the specified property in the specified group.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns><c>true</c> if this collection contains the specified property;
        /// otherwise <c>false</c>.</returns>
        /// <seealso cref="IPropertySource.CanGetValue(string)"/>
        public bool ContainsGroupValue(string group, string name)
        {
            group = group ?? string.Empty;
            Dictionary<string, object> g;
            return groups.TryGetValue(group, out g) && g.ContainsKey(name);
        }

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
        public bool CanGetValue(string name)
        {
            return ContainsGroupValue(null, name)
                || DefaultValueSource != null && DefaultValueSource.CanGetValue(name);
        }

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
        public bool CanGetGroupValue(string group, string name)
        {
            return ContainsGroupValue(group, name)
                || GroupedDefaultValueSource != null && GroupedDefaultValueSource.CanGetGroupValue(group, name);
        }

        /// <summary>
        /// Sets a string value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string value for the property.</param>
        public void SetValue(string name, string value) { SetGroupValue(null, name, value); }

        /// <summary>
        /// Sets a string array value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string array value for the property.</param>
        public void SetValue(string name, string[] value) { SetGroupValue(null, name, value); }

        /// <summary>
        /// Sets a boolean value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new boolean value for the property.</param>
        public void SetValue(string name, bool value) { SetGroupValue(null, name, value.ToString(CultureInfo.InvariantCulture)); }

        /// <summary>
        /// Sets an integer value for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new integer value for the property.</param>
        public void SetValue(string name, int value) { SetGroupValue(null, name, value.ToString(CultureInfo.InvariantCulture)); }

        /// <summary>
        /// Sets the value of the specified property.
        /// If the property did exist until now, it is created.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new value of the property.</param>
        public void SetValue(string name, object value) { SetGroupValue(null, name, value); }

        /// <summary>
        /// Sets a string value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string value for the property.</param>
        public void SetGroupValue(string group, string name, string value) { InternalSetValue(group, name, value); }

        /// <summary>
        /// Sets a string array value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new string array value for the property.</param>
        public void SetGroupValue(string group, string name, string[] value) { InternalSetValue(group, name, value); }

        /// <summary>
        /// Sets a boolean value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new boolean value for the property.</param>
        public void SetGroupValue(string group, string name, bool value) { InternalSetValue(group, name, value.ToString(CultureInfo.InvariantCulture)); }

        /// <summary>
        /// Sets an integer value for the specified group property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new integer value for the property.</param>
        public void SetGroupValue(string group, string name, int value) { InternalSetValue(group, name, value.ToString(CultureInfo.InvariantCulture)); }

        /// <summary>
        /// Sets the value of the specified property.
        /// If the property did exist until now, it is created.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new value for the property.</param>
        public void SetGroupValue(string group, string name, object value)
        {
            InternalSetValue(group, name, value);
        }

        private void InternalSetValue(string groupName, string propertyName, object value)
        {
            groupName = groupName ?? string.Empty;
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentOutOfRangeException("propertyName", "The property name must not be null or empty.");
            }
            List<string> keys;
            Dictionary<string, object> group;
            if (groups.ContainsKey(groupName))
            {
                keys = groupKeys[groupName];
                group = groups[groupName];
            }
            else
            {
                groupNames.Add(groupName);
                keys = new List<string>();
                groupKeys.Add(groupName, keys);
                group = new Dictionary<string, object>();
                groups.Add(groupName, group);
            }
            if (group.ContainsKey(propertyName))
            {
                group[propertyName] = value;
            }
            else
            {
                keys.Add(propertyName);
                group.Add(propertyName, value);
            }
        }

        private object InternalGetRawValue(string groupName, string propertyName, out bool found)
        {
            groupName = groupName ?? string.Empty;
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentOutOfRangeException("propertyName", "The property name must not be empty.");
            }
            Dictionary<string, object> group;
            if (groups.TryGetValue(groupName, out group))
            {
                object value;
                if (group.TryGetValue(propertyName, out value))
                {
                    found = true;
                    return value;
                }
            }
            found = false;
            return null;
        }

        private object InternalGetValue(string groupName, string propertyName, out bool found, object def = null)
        {
            groupName = groupName ?? string.Empty;
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentOutOfRangeException("propertyName", "The property name must not be empty.");
            }
            Dictionary<string, object> group;
            if (groups.TryGetValue(groupName, out group))
            {
                object value;
                if (group.TryGetValue(propertyName, out value))
                {
                    if (value != null)
                    {
                        found = true;
                        return ResolveGroupValue(groupName, propertyName, value);
                    }
                }
            }
            found = false;
            if (groupName == string.Empty)
            {
                if (DefaultValueSource != null &&
                    DefaultValueSource.CanGetValue(propertyName))
                {
                    return ResolveValue(propertyName,
                        DefaultValueSource.GetValue(propertyName));
                }
            }
            else
            {
                if (GroupedDefaultValueSource != null &&
                    GroupedDefaultValueSource.CanGetGroupValue(groupName, propertyName))
                {
                    return ResolveGroupValue(groupName, propertyName,
                        GroupedDefaultValueSource.GetGroupValue(groupName, propertyName));
                }
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
        public object GetRawValue(string name) { return GetRawGroupValue(null, name); }

        /// <summary>
        /// Gets the unresolved and untransformed value of a group property in this collection,
        /// without looking up the property in <see cref="GroupedDefaultValueSource"/>.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the specified property or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Is thrown
        /// if <c>null</c> is passed for <paramref name="name"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown 
        /// if an empty string is passed as <paramref name="name"/>.</exception>
        public object GetRawGroupValue(string group, string name)
        {
            bool found;
            return InternalGetRawValue(group, name, out found);
        }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the specified property, or <c>null</c> 
        /// if the property does not exist.</returns>
        public object GetValue(string name) { return GetGroupValue(null, name, null); }

        /// <summary>
        /// Gets the value of the specified property, or a given default value,
        /// in case the specified property does not exist.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>The value of the specified property, or <paramref name="def"/>
        /// in case the specified value does not exist.</returns>
        public object GetValue(string name, object def) { return GetGroupValue(null, name, def); }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the specified property, or <c>null</c> 
        /// if the property does not exist.</returns>
        public object GetGroupValue(string group, string name) { return GetGroupValue(group, name, null); }

        /// <summary>
        /// Gets the value of the specified property, or a given default value,
        /// in case the specified property does not exist.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>The value of the specified property, or <paramref name="def"/>
        /// in case the specified value does not exist.</returns>
        public object GetGroupValue(string group, string name, object def)
        {
            bool found;
            return InternalGetValue(group, name, out found, def);
        }

        /// <summary>
        /// Gets the value of a property as a string.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string, or <c>null</c> if the property does not exist
        /// or its value can not be properly converted.</returns>
        public string GetStringValue(string name) { return GetStringGroupValue(null, name, null); }

        /// <summary>
        /// Gets the value of a property as a string, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string GetStringValue(string name, string def) { return GetStringGroupValue(null, name, def); }

        /// <summary>
        /// Gets the value of a group property as a string.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string or <c>null</c> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string GetStringGroupValue(string group, string name) { return GetStringGroupValue(group, name, null); }

        /// <summary>
        /// Gets the value of a group property as a string, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string GetStringGroupValue(string group, string name, string def)
        {
            bool found;
            return InternalGetValue(group, name, out found, def) as string;
        }

        /// <summary>
        /// Gets the value of a property as a string array.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string array, that may be empty, if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string[] GetStringListValue(string name) { return GetStringListGroupValue(null, name, null); }

        /// <summary>
        /// Gets the value of a property as a string array, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string array or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string[] GetStringListValue(string name, string[] def) { return GetStringListGroupValue(null, name, def); }

        /// <summary>
        /// Gets the value of a group property as a string array.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>A string array, that may be empty if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string[] GetStringListGroupValue(string group, string name) { return GetStringListGroupValue(group, name, null); }

        /// <summary>
        /// Gets the value of a group property as a string array, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A string array or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public string[] GetStringListGroupValue(string group, string name, string[] def)
        {
            bool found;
            var value = InternalGetValue(group, name, out found, def);
            if (value is string) value = new string[] { (string)value };
            return (value as string[]) ?? new string[0];
        }

        /// <summary>
        /// Gets the value of a property as a boolean.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>A boolean, or <c>false</c> if the property does not exist
        /// or its value can not be properly converted.</returns>
        public bool GetBooleanValue(string name) { return GetBooleanGroupValue(null, name); }

        /// <summary>
        /// Gets the value of a property as a boolean, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A boolean or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public bool GetBooleanValue(string name, bool def) { return GetBooleanGroupValue(null, name, def); }

        /// <summary>
        /// Gets the value of a group property as a boolean.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>A boolean or <c>false</c> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public bool GetBooleanGroupValue(string group, string name) { return GetBooleanGroupValue(group, name, false); }

        /// <summary>
        /// Gets the value of a group property as a boolean, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>A boolean or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public bool GetBooleanGroupValue(string group, string name, bool def)
        {
            bool found;
            var value = InternalGetValue(group, name, out found, def);
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
        public int GetInt32Value(string name) { return GetInt32GroupValue(null, name); }

        /// <summary>
        /// Gets the value of a property as an integer, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>An integer or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public int GetInt32Value(string name, int def) { return GetInt32GroupValue(null, name, def); }

        /// <summary>
        /// Gets the value of a group property as an integer.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>An integer or <c>0</c> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public int GetInt32GroupValue(string group, string name) { return GetInt32GroupValue(group, name, 0); }

        /// <summary>
        /// Gets the value of a group property as an integer, or a default value if the
        /// specified property does not exist or its value can not be properly converted.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="def">The default value.</param>
        /// <returns>An integer or <paramref name="def"/> if the property does not exist,
        /// or its value can not be properly converted.</returns>
        public int GetInt32GroupValue(string group, string name, int def)
        {
            bool found;
            var value = InternalGetValue(group, name, out found, def);
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
        protected virtual object ResolveValue(string name, object value)
        {
            return value;
        }

        /// <summary>
        /// This method is a hook for child classes, 
        /// to implement some kind of value resolution or transformation for grouped properties.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The original value of the property.</param>
        /// <returns>The resolved or transformed value.</returns>
        protected virtual object ResolveGroupValue(string group, string name, object value)
        {
            return value;
        }

        /// <summary>
        /// Gets the groups in this collection.
        /// </summary>
        /// <returns>An enumeration of group names.</returns>
        public IEnumerable<string> Groups()
        {
            foreach (var gk in groupNames)
            {
                if (gk.Length > 0) yield return gk;
            }
        }

        /// <summary>
        /// Gets all groups, marked with the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>An enumeration of group names.</returns>
        public IEnumerable<string> GroupsByCategory(string category)
        {
            foreach (var gk in groupNames)
            {
                if (gk != string.Empty && GetGroupCategory(gk) == category)
                {
                    yield return gk;
                }
            }
        }

        /// <summary>
        /// Gets the names from all existing properties.
        /// </summary>
        /// <returns>An enumeration of strings.</returns>
        public IEnumerable<string> PropertyNames()
        {
            List<string> keys;
            return groupKeys.TryGetValue(string.Empty, out keys) ? keys : (IEnumerable<string>)new string[0];
        }

        /// <summary>
        /// Gets the property names in the specified group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <returns>An enumeration of property names.</returns>
        public IEnumerable<string> PropertyNames(string group)
        {
            group = group ?? string.Empty;
            List<string> keys;
            return groupKeys.TryGetValue(group, out keys) ? keys : (IEnumerable<string>)new string[0];
        }

        private string FormatValue(string group, string name, object val, bool resolve = true)
        {
            if (resolve)
            {
                val = ResolveGroupValue(group, name, val);
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
                    f[i] = FormatValue(group, name, a.GetValue(i), false);
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

        /// <summary>
        /// Returns a string represenation of this property collection.
        /// </summary>
        /// <returns>A string containing all properties and thier values.</returns>
        public override string ToString() { return ToString(true); }

        /// <summary>
        /// Returns a string represenation of this property collection.
        /// </summary>
        /// <param name="resolve">A flag, controlling the resolution of property values.</param>
        /// <returns>A string containing all properties and thier values.</returns>
        public string ToString(bool resolve)
        {
            var sb = new StringBuilder();
            string lastCategory = null;
            foreach (var gk in groupNames)
            {
                if (gk.Length > 0)
                {
                    var c = GetGroupCategory(gk);
                    if (c != null)
                    {
                        if (c != lastCategory)
                        {
                            lastCategory = c;
                            sb.AppendLine();
                            sb.AppendLine("## " + c);
                        }
                        sb.AppendLine();
                        sb.AppendLine("### " + gk);
                    }
                    else
                    {
                        sb.AppendLine();
                        sb.AppendLine("### " + gk);
                    }
                }
                var group = groups[gk];
                foreach (var k in groupKeys[gk])
                {
                    sb.AppendLine(string.Format("* {0}: {1}", k, FormatValue(gk, k, group[k], resolve)));
                }
            }
            return sb.ToString().TrimStart();
        }
    }
}
