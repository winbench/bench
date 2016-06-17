using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.Bench.Markdown;
using NUnit.Framework;

namespace Mastersign.Bench.Test
{
    [TestFixture]
    public class MarkdownParserTest
    {
        [Test]
        public void SimpleProperty()
        {
            var md = "* Key: Value";
            var cfg = new GroupedPropertyCollection();
            var parser = new MarkdownPropertyParser(cfg);
            using (var r = new StringReader(md)) { parser.Parse(r); }

            Assert.AreEqual("Value", cfg.GetValue("Key"));
        }

        [Test]
        public void QuotedProperties()
        {
            var md = "* K1: `Value`\n* K2: <Value2>";
            var cfg = new GroupedPropertyCollection();
            var parser = new MarkdownPropertyParser(cfg);
            using (var r = new StringReader(md)) { parser.Parse(r); }

            Assert.AreEqual("Value", cfg.GetValue("K1"));
            Assert.AreEqual("Value2", cfg.GetValue("K2"));
        }

        [Test]
        public void OneLineListProperty()
        {
            var md = "* Key: `ABC`, `XYZ`, `123`";
            var cfg = new GroupedPropertyCollection();
            var parser = new MarkdownPropertyParser(cfg);
            using (var r = new StringReader(md)) { parser.Parse(r); }

            Assert.AreEqual(new[] { "ABC", "XYZ", "123" }, cfg.GetValue("Key"));
        }

        [Test]
        public void MultilineListProperty()
        {
            var md = "* Key:\n   + ABC\n   + XYZ\n   + 123";
            var cfg = new GroupedPropertyCollection();
            var parser = new MarkdownPropertyParser(cfg);
            using (var r = new StringReader(md)) { parser.Parse(r); }

            Assert.AreEqual(new[] { "ABC", "XYZ", "123" }, cfg.GetValue("Key"));
        }

        [Test]
        public void MultilineListQuotedProperty()
        {
            var md = string.Join(Environment.NewLine,
                "* Key:",
                "  + `SK1`",
                "    + <sk2>",
                "  + SK3 : v3",
                "  + SK4: `v4`",
                "  + `SK5`:`v5`",
                "  + <SK6>: `v6`",
                "  + `SK7`:<v7>",
                "  + `$abc:123$`");
            var cfg = new GroupedPropertyCollection();
            var parser = new MarkdownPropertyParser(cfg);
            using (var r = new StringReader(md)) { parser.Parse(r); }

            Assert.AreEqual(new[]
            {
                "SK1",
                "sk2",
                "SK3 : v3",
                "SK4: v4",
                "SK5:v5",
                "SK6: v6",
                "SK7:v7",
                "$abc:123$"
            }, cfg.GetValue("Key"));
        }
    }
}
