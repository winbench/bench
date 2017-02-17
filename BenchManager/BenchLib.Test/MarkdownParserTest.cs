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
        private static PropertyCollection ParseProperties(string md)
        {
            var properties = new PropertyCollection();
            var parser = new MarkdownPropertyParser { PropertyTarget = properties };
            using (var r = new StringReader(md)) { parser.Parse(r); }
            return properties;
        }

        [Test]
        public void SimpleProperty()
        {
            var md = "* Key: Value";
            var props = ParseProperties(md);

            Assert.AreEqual("Value", props.GetValue("Key"));
        }

        [Test]
        public void QuotedProperties()
        {
            var md = "* K1: `Value`\n* K2: <Value2>";
            var props = ParseProperties(md);

            Assert.AreEqual("Value", props.GetValue("K1"));
            Assert.AreEqual("Value2", props.GetValue("K2"));
        }

        [Test]
        public void OneLineListProperty()
        {
            var md = "* Key: `ABC`, `XYZ`, `123`";
            var props = ParseProperties(md);

            Assert.AreEqual(new[] { "ABC", "XYZ", "123" }, props.GetValue("Key"));
        }

        [Test]
        public void MultilineListProperty()
        {
            var md = "* Key:\n   + ABC\n   + XYZ\n   + 123";
            var props = ParseProperties(md);

            Assert.AreEqual(new[] { "ABC", "XYZ", "123" }, props.GetValue("Key"));
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
            var props = ParseProperties(md);

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
            }, props.GetValue("Key"));
        }

        [Test]
        public void YamlHeader1Test()
        {
            var md = string.Join(Environment.NewLine,
                "---",
                "name=value",
                "* A: Invalid",
                "---",
                "* B: Valid");
            var props = ParseProperties(md);

            Assert.False(props.ContainsValue("A"));
            Assert.AreEqual("Valid", props.GetValue("B"));
        }

        [Test]
        public void YamlHeader2Test()
        {
            var md = string.Join(Environment.NewLine,
                "---",
                "name=value",
                "* A: Invalid",
                "...",
                "* B: Valid");
            var props = ParseProperties(md);

            Assert.False(props.ContainsValue("A"));
            Assert.AreEqual("Valid", props.GetValue("B"));
        }

        [Test]
        public void SimpleCodeBlock1Test()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "```",
                "* B: Invalid",
                "```",
                "* C: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.AreEqual("Valid", props.GetValue("C"));
        }

        [Test]
        public void SimpleCodeBlock2Test()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "~~~syntax",
                "* B: Invalid",
                "~~~",
                "* C: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.AreEqual("Valid", props.GetValue("C"));
        }

        [Test]
        public void ExtendedCodeBlockTest()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "`````",
                "* B: Invalid",
                "```",
                "* C: Invalid",
                "```",
                "`````",
                "* D: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.False(props.ContainsValue("C"));
            Assert.AreEqual("Valid", props.GetValue("D"));
        }

        [Test]
        public void SimpleHtmlComment1Test()
        {
            var md = string.Join(Environment.NewLine,
                "<!--",
                "* A: Invalid",
                "-->",
                "* B: Valid");
            var props = ParseProperties(md);

            Assert.False(props.ContainsValue("A"));
            Assert.AreEqual("Valid", props.GetValue("B"));
        }

        [Test]
        public void SimpleHtmlComment2Test()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "<!--",
                "* B: Invalid",
                "-->",
                "* C: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.AreEqual("Valid", props.GetValue("C"));
        }

        [Test]
        public void HtmlComment1Test()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "<!--* B: Invalid -->",
                "* C: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.AreEqual("Valid", props.GetValue("C"));
        }

        [Test]
        public void HtmlComment2Test()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "<!--* B: Invalid",
                "-->",
                "* C: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.AreEqual("Valid", props.GetValue("C"));
        }

        [Test]
        public void HtmlComment3Test()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "<!--",
                "* B: Invalid-->",
                "* C: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.AreEqual("Valid", props.GetValue("C"));
        }

        [Test]
        public void HtmlComment4Test()
        {
            var md = string.Join(Environment.NewLine,
                "* A: Valid",
                "<!-- XYZ --> <!-- <!--",
                "* B: Invalid",
                "--> <!-- XYZ --> text",
                "<!-- --> -->",
                "* C: Valid");
            var props = ParseProperties(md);

            Assert.AreEqual("Valid", props.GetValue("A"));
            Assert.False(props.ContainsValue("B"));
            Assert.AreEqual("Valid", props.GetValue("C"));
        }

    }
}
