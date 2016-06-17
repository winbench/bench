using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public class GroupedVariableResolver : IGroupedValueResolver
    {
        private static readonly Regex DefaultGroupVariablePattern = new Regex("\\$(?<group>.*?):(?<name>.+?)\\$");

        /// <remarks>
        /// The regular expression needs two named groups with names <c>group</c> and <c>name</c>.
        /// </remarks>
        public Regex GroupVariablePattern { get; set; }

        public IGroupedPropertyCollection ValueSource { get; set; }

        public GroupedVariableResolver()
        {
            GroupVariablePattern = DefaultGroupVariablePattern;
        }

        public GroupedVariableResolver(IGroupedPropertyCollection valueSource)
            : this()
        {
            ValueSource = valueSource;
        }

        public object ResolveGroupValue(string group, string name, object value)
        {
            if (value == null) return null;
            if (value is string[])
            {
                return Array.ConvertAll((string[])value, v => (string)ResolveGroupValue(group, name, v));
            }
            if (value is string && ValueSource != null && GroupVariablePattern != null)
            {
                value = GroupVariablePattern.Replace((string)value, m =>
                {
                    var g = m.Groups["group"].Value;
                    if (string.IsNullOrEmpty(g)) { g = group; }
                    var n = m.Groups["name"].Value;
                    return (ValueSource.GetGroupValue(g, n) as string) ?? string.Format("#{0}:{1}#", g, n);
                });
            }
            return value;
        }
    }
}
