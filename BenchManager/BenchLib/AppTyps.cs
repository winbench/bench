using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This static class contains string constants for the app types known to Bench.
    /// </summary>
    public static class AppTyps
    {
        /// <summary>The name of the default app typ.</summary>
        public const string Default = "default";

        /// <summary>The name of the meta app typ for custom apps and groups.</summary>
        public const string Meta = "meta";

        /// <summary>The name of the app typ for Node.js packages, managed by npm.</summary>
        public const string NodePackage = "node-package";

        /// <summary>The name of the app typ for Ruby gems.</summary>
        public const string RubyPackage = "ruby-package";

        /// <summary>The name of the app typ for generic Python packages, managed by PIP.</summary>
        public const string PythonPackage = "python-package";

        /// <summary>The name of the app typ for Python 2 packages, managed by PIP.</summary>
        public const string Python2Package = "python2-package";

        /// <summary>The name of the app typ for Python 3 packages, managed by PIP.</summary>
        public const string Python3Package = "python3-package";

        /// <summary>The name of the app typ for NuGet packages.</summary>
        public const string NuGetPackage = "nuget-package";
    }
}
