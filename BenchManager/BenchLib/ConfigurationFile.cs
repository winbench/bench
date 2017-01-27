using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// A descriptor of a configuration or app library file.
    /// </summary>
    public class ConfigurationFile
    {
        /// <summary>
        /// The kind of file.
        /// </summary>
        public ConfigurationFileType Type { get; private set; }

        /// <summary>
        /// A number describing the load order of the configuration files.
        /// </summary>
        public int OrderIndex { get; private set; }

        /// <summary>
        /// The absolute path of the configuration file.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigurationFile"/>.
        /// </summary>
        /// <param name="type">The kind of file</param>
        /// <param name="orderIndex">A number describing the load order of the configuration files</param>
        /// <param name="path">The absolute path of the configuration file</param>
        public ConfigurationFile(ConfigurationFileType type, int orderIndex, string path)
        {
            Type = type;
            OrderIndex = orderIndex;
            Path = path;
        }
    }
}
