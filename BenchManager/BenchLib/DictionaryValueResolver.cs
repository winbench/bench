using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    internal class DictionaryValueResolver : IValueResolver, IGroupedValueResolver
    {
        public object ResolveValue(string name, object value)
        {
            switch (name)
            {
                case ConfigPropertyKeys.KnownLicenses:
                    return ValueParser.ParseKeyValuePairs(value);
                case ConfigPropertyKeys.CustomEnvironment:
                    return ValueParser.ParseKeyValuePairs(value);
                default:
                    return value;
            }
        }

        public object ResolveGroupValue(string group, string name, object value)
        {
            switch (name)
            {
                case AppPropertyKeys.Docs:
                    return ValueParser.ParseKeyValuePairs(value);
                case AppPropertyKeys.DownloadHeaders:
                    return ValueParser.ParseKeyValuePairs(value);
                case AppPropertyKeys.DownloadCookies:
                    return ValueParser.ParseKeyValuePairs(value);
                case AppPropertyKeys.Environment:
                    return ValueParser.ParseKeyValuePairs(value);
                default:
                    return value;
            }
        }
    }
}
