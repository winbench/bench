using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class AppIndexValueResolver : IGroupedValueResolver
    {
        private static readonly char[] KeyValueSeparator = new char[] { ':' };

        public IGroupedPropertySource AppIndex { get; set; }

        public AppIndexValueResolver()
        {
        }

        public AppIndexValueResolver(IGroupedPropertySource appIndex)
            : this()
        {
            AppIndex = appIndex;
        }

        public object ResolveGroupValue(string group, string name, object value)
        {
            switch (name)
            {
                case PropertyKeys.AppDownloadHeaders:
                    return ParseKeyValuePairs(value);
                case PropertyKeys.AppDownloadCookies:
                    return ParseKeyValuePairs(value);
                case PropertyKeys.AppEnvironment:
                    return ParseKeyValuePairs(value);
                default:
                    return value;
            }
        }

        private Dictionary<string, string> ParseKeyValuePairs(object value)
        {
            var d = new Dictionary<string, string>();
            if (value is string)
            {
                var kvp = ParseKeyValuePair((string)value);
                if (!string.IsNullOrEmpty(kvp.Key))
                {
                    d.Add(kvp.Key, kvp.Value);
                }
            }
            if (value is string[])
            {
                foreach (var v in (string[])value)
                {
                    var kvp = ParseKeyValuePair(v);
                    if (!string.IsNullOrEmpty(kvp.Key))
                    {
                        d[kvp.Key] = kvp.Value;
                    }

                }
            }
            return d;
        }

        private KeyValuePair<string, string> ParseKeyValuePair(string header)
        {
            if (header != null && header.Contains(":"))
            {
                var p = header.Split(KeyValueSeparator, 2);
                return new KeyValuePair<string, string>(p[0].Trim(), p[1].Trim());
            }
            else
            {
                return new KeyValuePair<string, string>();
            }
        }
    }
}
