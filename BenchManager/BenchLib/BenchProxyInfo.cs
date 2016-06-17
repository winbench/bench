using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class BenchProxyInfo : IConfigurationPart
    {
        private const string DefaultExternalHttpTarget = "http://www.some-external-server-for-bench.com/";

        public bool UseProxy { get; set; }

        public string HttpProxyAddress { get; set; }

        public string HttpsProxyAddress { get; set; }

        public BenchProxyInfo()
        {
        }

        public BenchProxyInfo(string httpProxyAddress, string httpsProxyAddress)
        {
            UseProxy = true;
            HttpProxyAddress = httpProxyAddress;
            HttpsProxyAddress = httpsProxyAddress;
        }

        public void Transfer(IDictionary<string, string> dict)
        {
            dict[PropertyKeys.UseProxy] = UseProxy ? "true" : "false";
            if (UseProxy)
            {
                dict[PropertyKeys.HttpProxy] = HttpProxyAddress;
                dict[PropertyKeys.HttpsProxy] = HttpsProxyAddress;
            }
        }

        public override string ToString()
        {
            return string.Format("UseProxy={0}, HTTP='{1}', HTTPS='{2}'",
                UseProxy, HttpProxyAddress, HttpsProxyAddress);
        }

        public static BenchProxyInfo SystemDefault
        {
            get
            {
                var proxy = System.Net.WebRequest.DefaultWebProxy;
                if (proxy != null)
                {
                    // currently no way to determine different address for HTTPS connections
                    var httpAddress = proxy.GetProxy(new Uri(DefaultExternalHttpTarget)).ToString();
                    if (httpAddress != DefaultExternalHttpTarget)
                    {
                        return new BenchProxyInfo(httpAddress, httpAddress);
                    }
                }
                return new BenchProxyInfo();
            }
        }
    }
}
