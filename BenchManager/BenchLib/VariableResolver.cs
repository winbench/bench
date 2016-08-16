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
    /// The default syntax for a variable reference is an expression like <c>$NAME$</c>.
    /// Every occurance of such an expression is replaced by the value of the referenced property
    /// from <see cref="ValueSource"/>.
    /// This syntax can be changed by setting a custom expression for <see cref="VariablePattern"/>.
    /// </para>
    /// <para>
    /// If the referenced property does not exists, the expression is transformed
    /// by replacing it with <c>#NAME#</c>.
    /// </para>
    /// </summary>
    public class VariableResolver : IGroupedValueResolver
    {
        private static readonly Regex DefaultVariablePattern = new Regex("\\$(?<name>.+?)\\$");

        /// <summary>
        /// A regular expression, that detects variable references.
        /// The defaut expression is <c>\$(?&lt;name&gt;.+?)\$</c>
        /// </summary>
        /// <remarks>
        /// The regular expression needs a named capture group called <c>name</c>'.
        /// </remarks>
        public Regex VariablePattern { get; set; }

        /// <summary>
        /// A property collection, which will be used as to retrieve the referenced property values.
        /// </summary>
        public IPropertyCollection ValueSource { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="VariableResolver"/>.
        /// </summary>
        public VariableResolver()
        {
            VariablePattern = DefaultVariablePattern;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VariableResolver"/>.
        /// </summary>
        /// <param name="valueSource">The value source for the referenced variables.</param>
        public VariableResolver(IPropertyCollection valueSource)
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
