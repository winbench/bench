using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    internal class DictionaryValueResolver : IGroupedValueResolver
    {
        private static readonly char[] KeyValueSeparator = new char[] { ':' };

        public IGroupedPropertySource AppIndex { get; set; }

        public DictionaryValueResolver()
        {
        }

        public DictionaryValueResolver(IGroupedPropertySource appIndex)
            : this()
        {
            AppIndex = appIndex;
        }

        public object ResolveGroupValue(string group, string name, object value)
        {
            if (string.IsNullOrEmpty(group))
            {
                switch (name)
                {
                    case ConfigPropertyKeys.KnownLicenses:
                        return ParseKeyValuePairs(value);
                    case ConfigPropertyKeys.CustomEnvironment:
                        return ParseKeyValuePairs(value);
                    default:
                        return value;
                }
            }
            else
            {
                switch (name)
                {
                    case AppPropertyKeys.Docs:
                        return ParseKeyValuePairs(value);
                    case AppPropertyKeys.DownloadHeaders:
                        return ParseKeyValuePairs(value);
                    case AppPropertyKeys.DownloadCookies:
                        return ParseKeyValuePairs(value);
                    case AppPropertyKeys.Environment:
                        return ParseKeyValuePairs(value);
                    default:
                        return value;
                }
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

        public static KeyValuePair<string, string> ParseKeyValuePair(string value)
        {
            if (value != null && value.Contains(":"))
            {
                var p = value.Split(KeyValueSeparator, 2);
                return new KeyValuePair<string, string>(p[0].Trim(), p[1].Trim());
            }
            else
            {
                return new KeyValuePair<string, string>();
            }
        }
    }
}
