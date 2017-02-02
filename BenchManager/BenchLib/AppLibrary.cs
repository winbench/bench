using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class represents an app library.
    /// It is initialized with an ID and an URL, and holds a reference to the <see cref="BenchConfiguration"/>.
    /// </summary>
    public class AppLibrary
    {
        private readonly BenchConfiguration config;

        /// <summary>
        /// The ID of this app library.
        /// The ID is unique in the Bench environment.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// The absolute URL to the app library.
        /// The URL must have a HTTP, HTTPS or FILE scheme.
        /// The referred resource must be a ZIP file, containing the <c>apps.md</c>
        /// and optionally the custom scripts.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="AppLibrary"/>.
        /// </summary>
        /// <param name="config">The Bench configuration.</param>
        /// <param name="id">The uniqe ID of the app library.</param>
        /// <param name="url">The URL of the app library.</param>
        public AppLibrary(BenchConfiguration config, string id, Uri url)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (!string.Equals("http", url.Scheme, StringComparison.InvariantCultureIgnoreCase) &&
                !string.Equals("https", url.Scheme, StringComparison.InvariantCultureIgnoreCase) &&
                !string.Equals("file", url.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("The URL scheme is not supported.", nameof(url));
            }
            this.config = config;
            ID = id;
            Url = url;
        }

        /// <summary>
        /// Gets an absolute path to the base directory of the app library.
        /// </summary>
        public string BaseDir => Path.Combine(
            config.GetStringValue(ConfigPropertyKeys.AppLibsInstallDir),
            ID.ToLowerInvariant());
    }
}
