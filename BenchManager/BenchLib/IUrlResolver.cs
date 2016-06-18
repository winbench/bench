using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mastersign.Bench
{
    public interface IUrlResolver
    {
        bool Matches(Uri url);

        Uri Resolve(Uri url, WebClient wc);
    }
}
