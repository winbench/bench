using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Mastersign.Bench
{
    public class SchemeDispatchProxy : IWebProxy
    {
        private readonly IDictionary<string, IWebProxy> proxyTable;

        public SchemeDispatchProxy(IDictionary<string, IWebProxy> proxyTable)
        {
            this.proxyTable = proxyTable;
        }

        public ICredentials Credentials { get; set; }

        public bool IsBypassed(Uri host)
        {
            Debug.WriteLine("Check bypass for " + host);
            var def = true;
            IWebProxy proxy;
            return proxyTable.TryGetValue(host.Scheme.ToLowerInvariant(), out proxy)
                ? proxy != null ? proxy.IsBypassed(host) : def
                : def;
        }

        public Uri GetProxy(Uri destination)
        {
            Debug.WriteLine("Get proxy for " + destination);
            var def = destination;
            IWebProxy proxy;
            return proxyTable.TryGetValue(destination.Scheme.ToLowerInvariant(), out proxy)
                ? proxy != null ? proxy.GetProxy(destination) : def
                : def;
        }
    }
}
