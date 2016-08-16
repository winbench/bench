using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Mastersign.Bench
{
    internal abstract class PageContentUrlResolver : IUrlResolver
    {
        public UrlPattern UrlPattern { get; private set; }

        protected PageContentUrlResolver(UrlPattern urlPattern)
        {
            UrlPattern = urlPattern;
        }

        /// <summary>
        /// Checks, whether this resolver recognizes this URL to be resolvable.
        /// </summary>
        /// <param name="url">The URL in question.</param>
        /// <returns><c>true</c> if this URL can be resolved by this resolver; otherwise <c>false</c>.</returns>
        public bool Matches(Uri url)
        {
            if (UrlPattern == null) return true;
            return UrlPattern.IsMatch(url);
        }

        /// <summary>
        /// Resolves the given URL, potentially by using the given <see cref="WebClient"/>
        /// to run additional HTTP(S) requests.
        /// </summary>
        /// <param name="url">The URL to resolve.</param>
        /// <param name="wc">A <see cref="WebClient"/> to execute HTTP(S) requests.</param>
        /// <returns></returns>
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
