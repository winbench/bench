using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This interace describes the capability of storing property values.
    /// The properties are name with a unique strings.
    /// </summary>
    public interface IPropertyTarget
    {
        /// <summary>
        /// Sets the value of the specified property.
        /// If the property did exist until now, it is created.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new value of the property.</param>
        void SetValue(string name, object value);
    }
}
