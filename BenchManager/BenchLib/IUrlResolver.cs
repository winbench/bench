using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This interface describes the capability to resolve an URL.
    /// This can e.g. mean to parse a HTML page and extract the URL of a redirection meta tag.
    /// </summary>
    internal interface IUrlResolver
    {
        /// <summary>
        /// Checks, whether this resolver recognizes this URL to be resolvable.
        /// </summary>
        /// <param name="url">The URL in question.</param>
        /// <returns><c>true</c> if this URL can be resolved by this resolver; otherwise <c>false</c>.</returns>
        bool Matches(Uri url);

        /// <summary>
        /// Resolves the given URL, potentially by using the given <see cref="WebClient"/>
        /// to run additional HTTP(S) requests.
        /// </summary>
        /// <param name="url">The URL to resolve.</param>
        /// <param name="wc">A <see cref="WebClient"/> to execute HTTP(S) requests.</param>
        /// <returns></returns>
        Uri Resolve(Uri url, WebClient wc);
    }
}
