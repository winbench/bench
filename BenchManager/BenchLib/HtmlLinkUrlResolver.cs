using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public class HtmlLinkUrlResolver : PageContentUrlResolver
    {
        public UrlPattern HrefPattern { get; private set; }

        private static Regex HtmlLinkPattern = new Regex(@"\<a\s[^\>]*?href=""(?<url>.+?)""");

        public HtmlLinkUrlResolver(UrlPattern urlPattern, UrlPattern hrefPattern)
            : base(urlPattern)
        {
            HrefPattern = hrefPattern;
        }

        protected override Uri ExtractUrl(Uri baseUrl, string text)
        {
            Debug.WriteLine("Extracting link from HTML ...");
            var matches = HtmlLinkPattern.Matches(text);
            Debug.WriteLine("Found " + matches.Count + " links.");
            foreach (Match m in matches)
            {
                var urlStr = m.Groups["url"].Value;
                Debug.WriteLine("Raw URL: " + urlStr);
                var url = Uri.IsWellFormedUriString(urlStr, UriKind.Absolute)
                    ? new Uri(urlStr)
                    : Uri.IsWellFormedUriString(urlStr, UriKind.Relative)
                        ? new Uri(baseUrl, urlStr)
                        : null;
                Debug.WriteLine("Absolut: " + url);
                if (url != null && (HrefPattern == null || HrefPattern.IsMatch(url)))
                {
                    Debug.WriteLine("First matching URL: " + url);
                    return url;
                }
            }
            Debug.WriteLine("Extracting link from HTML failed.");
            return null;
        }
    }
}
