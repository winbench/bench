using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mastersign.Bench
{
    public class GroupedPropertyCollection : IConfiguration
    {
        private readonly List<string> groupNames = new List<string>(); // ordered list for group names
        private readonly Dictionary<string, string> groupCategories = new Dictionary<string, string>();
        private readonly Dictionary<string, List<string>> groupKeys = new Dictionary<string, List<string>>(); // ordered lists for property names
        private readonly Dictionary<string, Dictionary<string, object>> groups = new Dictionary<string, Dictionary<string, object>>();

        public IPropertySource DefaultValueSource { get; set; }

        public IGroupedPropertySource GroupedDefaultValueSource { get; set; }

        public void Clear()
        {
            groupNames.Clear();
            groupCategories.Clear();
            groupKeys.Clear();
            groups.Clear();
        }

        public void SetGroupCategory(string group, string category)
        {
            group = group ?? string.Empty;
            groupCategories[group] = category;
        }

        public string GetGroupCategory(string group)
        {
            string category;
            return groupCategories.TryGetValue(group, out category) ? category : null;
        }

        public bool ContainsGroup(string group)
        {
            group = group ?? string.Empty;
            return groups.ContainsKey(group);
        }

        public bool ContainsValue(string name) { return ContainsGroupValue(null, name); }

        public bool ContainsGroupValue(string group, string name)
        {
            group = group ?? string.Empty;
            Dictionary<string, object> g;
            return groups.TryGetValue(group, out g) && g.ContainsKey(name);
        }

        public bool CanGetValue(string name)
        {
            return ContainsGroupValue(null, name)
                || DefaultValueSource != null && DefaultValueSource.CanGetValue(name);
        }

        public bool CanGetGroupValue(string group, string name)
        {
            return ContainsGroupValue(group, name)
                || GroupedDefaultValueSource != null && GroupedDefaultValueSource.CanGetGroupValue(group, name);
        }

        public void SetValue(string name, string value) { SetGroupValue(null, name, value); }

        public void SetValue(string name, string[] value) { SetGroupValue(null, name, value); }

        public void SetValue(string name, bool value) { SetGroupValue(null, name, value.ToString(CultureInfo.InvariantCulture)); }

        public void SetValue(string name, int value) { SetGroupValue(null, name, value.ToString(CultureInfo.InvariantCulture)); }

        public void SetValue(string name, object value) { SetGroupValue(null, name, value); }

        public void SetGroupValue(string group, string name, string value) { InternalSetValue(group, name, value); }

        public void SetGroupValue(string group, string name, string[] value) { InternalSetValue(group, name, value); }

        public void SetGroupValue(string group, string name, bool value) { InternalSetValue(group, name, value.ToString(CultureInfo.InvariantCulture)); }

        public void SetGroupValue(string group, string name, int value) { InternalSetValue(group, name, value.ToString(CultureInfo.InvariantCulture)); }

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
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentOutOfRangeException("propertyName", "The property name must not be null or empty.");
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
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentOutOfRangeException("propertyName", "The property name must not be null or empty.");
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

        public object GetRawValue(string name) { return GetRawGroupValue(null, name); }

        public object GetRawGroupValue(string group, string name)
        {
            bool found;
            return InternalGetRawValue(group, name, out found);
        }

        public object GetValue(string name) { return GetGroupValue(null, name, null); }

        public object GetValue(string name, object def) { return GetGroupValue(null, name, def); }

        public object GetGroupValue(string group, string name) { return GetGroupValue(group, name, null); }

        public object GetGroupValue(string group, string name, object def)
        {
            bool found;
            return InternalGetValue(group, name, out found, def);
        }

        public string GetStringValue(string name) { return GetStringGroupValue(null, name, null); }

        public string GetStringValue(string name, string def) { return GetStringGroupValue(null, name, def); }

        public string GetStringGroupValue(string group, string name) { return GetStringGroupValue(group, name, null); }

        public string GetStringGroupValue(string group, string name, string def)
        {
            bool found;
            return InternalGetValue(group, name, out found, def) as string;
        }

        public string[] GetStringListValue(string name) { return GetStringListGroupValue(null, name, null); }

        public string[] GetStringListValue(string name, string[] def) { return GetStringListGroupValue(null, name, def); }

        public string[] GetStringListGroupValue(string group, string name) { return GetStringListGroupValue(group, name, null); }

        public string[] GetStringListGroupValue(string group, string name, string[] def)
        {
            bool found;
            var value = InternalGetValue(group, name, out found, def);
            if (value is string) value = new string[] { (string)value };
            return (value as string[]) ?? new string[0];
        }

        public bool GetBooleanValue(string name) { return GetBooleanGroupValue(null, name); }

        public bool GetBooleanValue(string name, bool def) { return GetBooleanGroupValue(null, name, def); }

        public bool GetBooleanGroupValue(string group, string name) { return GetBooleanGroupValue(group, name, false); }

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

        public int GetInt32Value(string name) { return GetInt32GroupValue(null, name); }

        public int GetInt32Value(string name, int def) { return GetInt32GroupValue(null, name, def); }

        public int GetInt32GroupValue(string group, string name) { return GetInt32GroupValue(group, name, 0); }

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

        protected virtual object ResolveValue(string name, object value)
        {
            return value;
        }

        protected virtual object ResolveGroupValue(string group, string name, object value)
        {
            return value;
        }

        public IEnumerable<string> Groups()
        {
            foreach (var gk in groupNames)
            {
                if (gk.Length > 0) yield return gk;
            }
        }

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

        public IEnumerable<string> PropertyNames()
        {
            List<string> keys;
            return groupKeys.TryGetValue(string.Empty, out keys) ? keys : (IEnumerable<string>)new string[0];
        }

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

        public override string ToString() { return ToString(true); }

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
