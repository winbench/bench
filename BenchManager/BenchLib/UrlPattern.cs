using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class helps to identify URLs which must be resolved
    /// before the download of an HTTP(S) resource is possible.
    /// </summary>
    public class UrlPattern
    {
        /// <summary>
        /// A regular expression wich must match the host part of an URL, or <c>null</c>.
        /// </summary>
        public Regex HostPattern { get; private set; }

        /// <summary>
        /// A regular expression which must match the path part of an URL, or <c>null</c>.
        /// </summary>
        public Regex PathPattern { get; private set; }

        /// <summary>
        /// A dictionary with regular expressions, which must match the query arguments of an URL, or <c>null</c>.
        /// The key in the dictionary is the name of a query argument.
        /// The value in the dictionary is the regular expression which must match the value of
        /// the corresponding query argument.
        /// </summary>
        public IDictionary<string, Regex> QueryPattern { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="UrlPattern"/>.
        /// </summary>
        /// <param name="host">A regular expression for the host part of an URL or <c>null</c>.</param>
        /// <param name="path">A regular expression for the path part of an URL or <c>null</c>.</param>
        /// <param name="query">A dictionary with regular expressions, which must match
        /// the query arguments of an URL, or <c>null</c>.</param>
        public UrlPattern(Regex host, Regex path, IDictionary<string, Regex> query)
        {
            HostPattern = host;
            PathPattern = path;
            QueryPattern = query;
        }

        /// <summary>
        /// Checks, whether the given URL is a match.
        /// </summary>
        /// <param name="url">The URL in question.</param>
        /// <returns><c>true</c> if the given URL matches this pattern; othwerwise <c>false</c>.</returns>
        public bool IsMatch(Uri url)
        {
            if (HostPattern != null && !HostPattern.IsMatch(url.Host)) return false;
            if (PathPattern != null && !PathPattern.IsMatch(url.AbsolutePath)) return false;
            if (QueryPattern != null)
            {
                if (string.IsNullOrEmpty(url.Query)) return false;
                var query = url.Query;
                if (query.StartsWith("?")) query = query.Substring(1);
                var pairs = query.Split('&');
                var args = new Dictionary<string, string>();
                foreach (var p in pairs)
                {
                    var kv = p.Split('=');
                    args[kv[0]] = kv.Length > 1 ? kv[1] : null;
                }
                foreach (var n in QueryPattern.Keys)
                {
                    if (!args.ContainsKey(n)) return false;
                    var vp = QueryPattern[n];
                    if (vp != null)
                    {
                        var v = args[n];
                        if (v == null) return false;
                        if (!vp.IsMatch(v)) return false;
                    }
                }
            }
            return true;
        }
    }
}
