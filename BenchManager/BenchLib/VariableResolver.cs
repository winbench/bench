using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public class VariableResolver : IGroupedValueResolver
    {
        private static readonly Regex DefaultVariablePattern = new Regex("\\$(?<name>.+?)\\$");

        /// <remarks>
        /// The regular expression needs a named group with name <c>name</c>'.
        /// </remarks>
        public Regex VariablePattern { get; set; }

        public IPropertyCollection ValueSource { get; set; }

        public VariableResolver()
        {
            VariablePattern = DefaultVariablePattern;
        }

        public VariableResolver(IPropertyCollection valueSource)
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
            if (value is string && ValueSource != null && VariablePattern != null)
            {
                value = VariablePattern.Replace((string)value, m =>
                {
                    var n = m.Groups["name"].Value;
                    return (ValueSource.GetValue(n) as string) ?? string.Format("#{0}#", n);
                });
            }
            return value;
        }
    }
}
