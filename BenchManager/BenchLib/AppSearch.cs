using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mastersign.Bench
{
    public static class AppSearch
    {
        private static readonly Regex tokenPattern = new Regex("\"[^\"]+\"|[^\"\\s]+");

        /// <summary>
        /// Normalizes a string in a way it can be compared easily during a search.
        /// </summary>
        /// <param name="value">The string to normlize.</param>
        /// <returns>A normalized version of the string.</returns>
        public static string NormalizeForSearch(string value)
            => value?.Trim().ToLowerInvariant();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static string[] TokenizeSearchString(string searchString)
        {
            var matches = tokenPattern.Matches(searchString);
            var result = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                result[i] = matches[i].Value;
            }
            return result;
        }
    }
}
