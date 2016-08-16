using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// An enumeration with all supported major Python versions.
    /// </summary>
    public enum PythonVersion
    {
        /// <summary>
        /// Python &gt;= 2.7, and &lt; 3.0
        /// </summary>
        Python2,

        /// <summary>
        /// Python &gt;= 3.0
        /// </summary>
        Python3,
    }
}
