using System;
using System.Collections.Generic;

namespace Mastersign.Bench.Test
{
    class GroupedPropertyCollection : PropertyCollection, IGroupedPropertyCollection
    {
        private IDictionary<string, IDictionary<string, object>> groups;
        private IDictionary<string, string> groupCategories;

        public GroupedPropertyCollection(
            IDictionary<string, IDictionary<string, object>> groups = null,
            IDictionary<string, string> groupCategories = null,
            IDictionary<string, object> properties = null)
            : base(properties)
        {
            this.groups = groups ?? new Dictionary<string, IDictionary<string, object>>();
            this.groupCategories = groupCategories ?? new Dictionary<string, string>();
        }

        public bool CanGetGroupValue(string group, string name)
        {
            if (group == null) return CanGetValue(name);
            return ContainsGroupValue(group, name);
        }

        public bool ContainsGroup(string group)
        {
            return groups.ContainsKey(group);
        }

        public bool ContainsGroupValue(string group, string name)
        {
            if (group == null) return ContainsValue(name);
            return ContainsGroup(group) && groups[group].ContainsKey(name);
        }

        public string GetGroupCategory(string group)
        {
            string category;
            return groupCategories.TryGetValue(group, out category)
                ? category : null;
        }

        public object GetGroupValue(string group, string name)
        {
            if (group == null) return GetValue(name);
            return GetGroupValue(group, name, null);
        }

        public object GetGroupValue(string group, string name, object def)
        {
            if (group == null) return GetValue(name, def);
            IDictionary<string, object> properties;
            if (groups.TryGetValue(group, out properties))
            {
                object v;
                return properties.TryGetValue(name, out v)
                    ? v : def;
            }
            return def;
        }

        public IEnumerable<string> Groups()
        {
            return groups.Keys;
        }

        public IEnumerable<string> GroupsByCategory(string category)
        {
            foreach (var group in groupCategories.Keys)
            {
                if (groupCategories[group] == category) yield return group;
            }
        }

        public IEnumerable<string> PropertyNames(string group)
        {
            IDictionary<string, object> properties;
            return groups.TryGetValue(group, out properties)
                ? properties.Keys : null;
        }

        public void SetGroupCategory(string group, string category)
        {
            groupCategories[group] = category;
        }

        public void SetGroupValue(string group, string name, object value)
        {
            if (group == null)
            {
                SetValue(name, value);
            }
            else
            {
                IDictionary<string, object> properties;
                if (!groups.TryGetValue(group, out properties))
                {
                    properties.Add(group, properties = new Dictionary<string, object>());
                }
                properties[name] = value;
            }
        }
    }
}
