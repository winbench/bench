using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Mastersign.Bench.Test
{
    [TestFixture]
    public class UrlPatternTest
    {
        [Test]
        public void MatchWithoutPatterns()
        {
            var up = new UrlPattern(null, null, null);

            Assert.IsTrue(up.IsMatch(new Uri("http://host/path?query=1")));
        }

        [Test]
        public void MatchWithHost()
        {
            var up = new UrlPattern(new Regex("^host$"), null, null);

            Assert.IsTrue(up.IsMatch(new Uri("http://host")));
            Assert.IsTrue(up.IsMatch(new Uri("http://host/")));
            Assert.IsTrue(up.IsMatch(new Uri("http://host:123")));
            Assert.IsTrue(up.IsMatch(new Uri("http://host/path")));
            Assert.IsTrue(up.IsMatch(new Uri("http://host/?query=1")));

            Assert.IsFalse(up.IsMatch(new Uri("http://unknown")));
            Assert.IsFalse(up.IsMatch(new Uri("http://host1")));
            Assert.IsFalse(up.IsMatch(new Uri("http://nohost")));
            Assert.IsFalse(up.IsMatch(new Uri("http://unknown/")));
            Assert.IsFalse(up.IsMatch(new Uri("http://unknown/path")));
        }

        [Test]
        public void MatchWithPath()
        {
            var up = new UrlPattern(null, new Regex(@"^/path/file\.txt$"), null);

            Assert.IsTrue(up.IsMatch(new Uri("http://localhost/path/file.txt")));
            Assert.IsTrue(up.IsMatch(new Uri("http://localhost/path/file.txt?query=1")));

            Assert.IsFalse(up.IsMatch(new Uri("http://host/xyz/path/file.txt")));
            Assert.IsFalse(up.IsMatch(new Uri("http://host/path/file.txt/abc")));
            Assert.IsFalse(up.IsMatch(new Uri("http://host/path/file.txt/")));
        }

        [Test]
        public void MatchWithQuery()
        {
            var up1 = new UrlPattern(null, null, new Dictionary<string, Regex>
            {
                 {"abc", null }
            });

            Assert.IsTrue(up1.IsMatch(new Uri("http://host/path?abc")));
            Assert.IsTrue(up1.IsMatch(new Uri("http://host/path?abc=")));
            Assert.IsTrue(up1.IsMatch(new Uri("http://host/path?abc=456")));
            Assert.IsTrue(up1.IsMatch(new Uri("http://host/path?xyz=123&abc")));
            Assert.IsTrue(up1.IsMatch(new Uri("http://host/path?xyz=123&abc=")));
            Assert.IsTrue(up1.IsMatch(new Uri("http://host/path?xyz=123&abc=456")));

            Assert.IsFalse(up1.IsMatch(new Uri("http://host/path")));
            Assert.IsFalse(up1.IsMatch(new Uri("http://host/path?abcd")));
            Assert.IsFalse(up1.IsMatch(new Uri("http://host/path?xyz=123")));
            Assert.IsFalse(up1.IsMatch(new Uri("http://host/path?xyz=123&abcd")));
        }
    }
}
