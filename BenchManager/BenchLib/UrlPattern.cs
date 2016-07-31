using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public class UrlPattern
    {
        public Regex HostPattern { get; private set; }

        public Regex PathPattern { get; private set; }

        public IDictionary<string, Regex> QueryPattern { get; private set; }

        public UrlPattern(Regex host, Regex path, IDictionary<string, Regex> query)
        {
            HostPattern = host;
            PathPattern = path;
            QueryPattern = query;
        }

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
