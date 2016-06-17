using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public abstract class PageContentUrlResolver : IUrlResolver
    {
        public UrlPattern UrlPattern { get; private set; }

        protected PageContentUrlResolver(UrlPattern urlPattern)
        {
            UrlPattern = urlPattern;
        }

        public bool Matches(Uri url)
        {
            if (UrlPattern == null) return true;
            return UrlPattern.IsMatch(url);
        }

        public virtual Uri Resolve(Uri url, WebClient wc)
        {
            Debug.WriteLine("Downloading page " + url + " for URL resolution...");
            string page;
            try
            {
                page = wc.DownloadString(url);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error during page download: " + e);
                return null;
            }
            try
            {
                return ExtractUrl(url, page);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error while extracting the URL from the page: " + e);
                return null;
            }
        }

        protected abstract Uri ExtractUrl(Uri baseUrl, string text);
    }
}
