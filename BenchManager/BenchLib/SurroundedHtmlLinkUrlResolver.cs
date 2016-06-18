using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public class SurroundedHtmlLinkUrlResolver : HtmlLinkUrlResolver
    {
        public Regex SurroundingPattern { get; private set; }

        public SurroundedHtmlLinkUrlResolver(
            UrlPattern urlPattern,
            Regex surrounding, UrlPattern hrefPattern = null)
            : base(urlPattern, hrefPattern)
        {
            SurroundingPattern = surrounding;
        }

        protected override Uri ExtractUrl(Uri baseUrl, string text)
        {
            Debug.WriteLine("Searching for surrounding markup in HTML ...");
            var m = SurroundingPattern.Match(text);
            if (m.Success)
            {
                Debug.WriteLine("Found surrounding markup.");
                var snippet = m.Groups[1].Value;
                return base.ExtractUrl(baseUrl, snippet);
            }
            Debug.WriteLine("Did not find surrounding markup.");
            return null;
        }
    }
}
