using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    /// <summary>
    /// <para>
    /// This resolver for group property values, resolves variable references in property values.
    /// </para>
    /// <para>
    /// The default syntax for a variable reference is an expression like <c>$GROUP:NAME$</c>.
    /// Every occurance of such an expression is replaced by the value of the referenced property
    /// from <see cref="ValueSource"/>.
    /// This syntax can be changed by setting a custom expression for <see cref="GroupVariablePattern"/>.
    /// </para>
    /// <para>
    /// If the referenced property does not exists, the expression is transformed
    /// by replacing it with <c>#GROUP:NAME#</c>.
    /// </para>
    /// </summary>
    public class GroupedVariableResolver : IGroupedValueResolver
    {
        private static readonly Regex DefaultGroupVariablePattern = new Regex("\\$(?<group>.*?):(?<name>.+?)\\$");

        /// <summary>
        /// A regular expression, that detects variable references.
        /// The defaut expression is <c>\$(?&lt;group&gt;.*?):(?&lt;name&gt;.+?)\$</c>
        /// </summary>
        /// <remarks>
        /// The regular expression needs two named capture groups called <c>group</c> and <c>name</c>.
        /// </remarks>
        public Regex GroupVariablePattern { get; set; }

        /// <summary>
        /// A property collection, which will be used as to retrieve the referenced property values.
        /// </summary>
        public IGroupedPropertyCollection ValueSource { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="GroupedVariableResolver"/>.
        /// </summary>
        public GroupedVariableResolver()
        {
            GroupVariablePattern = DefaultGroupVariablePattern;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GroupedVariableResolver"/>.
        /// </summary>
        /// <param name="valueSource">The value source for the referenced properties.</param>
        public GroupedVariableResolver(IGroupedPropertyCollection valueSource)
            : this()
        {
            ValueSource = valueSource;
        }

        /// <summary>
        /// Returns the resolved or transformed value of the specified property.
        /// </summary>
        /// <param name="group">The group of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The original value of the specified property.</param>
        /// <returns>The resolved or transformed value for the specified value.</returns>
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
