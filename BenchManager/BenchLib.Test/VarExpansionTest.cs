using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Mastersign.Bench.Test
{
    [TestFixture]
    public class VarExpansionTest
    {
        [Test]
        public void SimpleVariableExpansion()
        {
            var propCollection = new PropertyCollection(new Dictionary<string, object>
            {
                {"a", "123"},
                {"b", "456"},
            });
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new VariableResolver(propCollection));

            cfg.SetValue("v1", "$a$");
            cfg.SetValue("v2", "pre$a$");
            cfg.SetValue("v3", "$a$post");
            cfg.SetValue("v4", "pre$a$post");
            cfg.SetValue("v5", "$a$$a$$a$");
            cfg.SetValue("v6", "$a$$b$");

            Assert.AreEqual("123", cfg.GetValue("v1"));
            Assert.AreEqual("pre123", cfg.GetValue("v2"));
            Assert.AreEqual("123post", cfg.GetValue("v3"));
            Assert.AreEqual("pre123post", cfg.GetValue("v4"));
            Assert.AreEqual("123123123", cfg.GetValue("v5"));
            Assert.AreEqual("123456", cfg.GetValue("v6"));
        }

        [Test]
        public void InvalidVariableReference()
        {
            var propCollection = new PropertyCollection(new Dictionary<string, object>
            {
                {"a", "123"},
            });
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new VariableResolver(propCollection));

            cfg.SetValue("v1", " $a ");
            cfg.SetValue("v2", " a$ ");
            cfg.SetValue("v3", " $a $ ");
            cfg.SetValue("v4", " $x$ ");

            Assert.AreEqual(" $a ", cfg.GetValue("v1"));
            Assert.AreEqual(" a$ ", cfg.GetValue("v2"));
            Assert.AreEqual(" #a # ", cfg.GetValue("v3"));
            Assert.AreEqual(" #x# ", cfg.GetValue("v4"));
        }

        [Test]
        public void InvalidVariableValue()
        {
            var propCollection = new PropertyCollection(new Dictionary<string, object>
            {
                {"a", 123},
                {"b", true},
            });
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new VariableResolver(propCollection));

            cfg.SetValue("v1", "$a$");
            cfg.SetValue("v2", "$b$");

            Assert.AreEqual("#a#", cfg.GetValue("v1"));
            Assert.AreEqual("#b#", cfg.GetValue("v2"));
        }

        //[Test]
        //public void RecursiveExpansion()
        //{
        //    var cfg = new ResolvingPropertyCollection();
        //    cfg.AddResolver(new VariableResolver(cfg));

        //    cfg.SetValue("x", "$x$");
        //    cfg.SetValue("y1", "$y2$");
        //    cfg.SetValue("y2", "$y1$");
        //    cfg.SetValue("z1", "$z2$");
        //    cfg.SetValue("z2", "$z3$");
        //    cfg.SetValue("z3", "$z1$");

        //    Assert.AreEqual("#x#", cfg.GetValue("x"));
        //    Assert.AreEqual("#y1#", cfg.GetValue("y1"));
        //    Assert.AreEqual("#z1#", cfg.GetValue("z1"));
        //}

        [Test]
        public void TransitiveVariableExpansion()
        {
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new VariableResolver(cfg));

            cfg.SetValue("x1", "$x2$");
            cfg.SetValue("x2", "123");
            cfg.SetValue("y1", "$y2$");
            cfg.SetValue("y2", "$y3$");
            cfg.SetValue("y3", "123");

            Assert.AreEqual("123", cfg.GetValue("x1"));
            Assert.AreEqual("123", cfg.GetValue("y1"));
        }

        [Test]
        public void GroupedVariableExpansion()
        {
            var groupedProperties = new GroupedPropertyCollection(new Dictionary<string, IDictionary<string, object>>
            {
                { "A", new Dictionary<string, object> {
                    { "x", "123" },
                    { "y", "456" }
                } },
                { "B", new Dictionary<string, object>
                {
                    { "x", "UVW" }
                } },
            });
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new GroupedVariableResolver(groupedProperties));

            cfg.SetValue("v1", "$A:x$");
            cfg.SetValue("v2", "pre$A:x$");
            cfg.SetValue("v3", "$A:x$post");
            cfg.SetValue("v4", "pre$A:x$post");
            cfg.SetValue("v5", "$A:x$$A:x$$A:x$");
            cfg.SetValue("v6", "$A:x$$A:y$$B:x$");

            Assert.AreEqual("123", cfg.GetValue("v1"));
            Assert.AreEqual("pre123", cfg.GetValue("v2"));
            Assert.AreEqual("123post", cfg.GetValue("v3"));
            Assert.AreEqual("pre123post", cfg.GetValue("v4"));
            Assert.AreEqual("123123123", cfg.GetValue("v5"));
            Assert.AreEqual("123456UVW", cfg.GetValue("v6"));
        }

        [Test]
        public void InvalidGroupedVariableReference()
        {
            var groupedProperties = new GroupedPropertyCollection(new Dictionary<string, IDictionary<string, object>>
            {
                { "A", new Dictionary<string, object> {
                    { "x", "123" },
                } },
            });
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new GroupedVariableResolver(groupedProperties));

            cfg.SetValue("v1", " $A:x ");
            cfg.SetValue("v2", " A:x$ ");
            cfg.SetValue("v3", " $A :x$ ");
            cfg.SetValue("v4", " $A: x$ ");
            cfg.SetValue("v5", " $ A:x$ ");
            cfg.SetValue("v6", " $A:x $ ");
            cfg.SetValue("v7", " $A:y$ ");
            cfg.SetValue("v8", " $B:x$ ");

            Assert.AreEqual(" $A:x ", cfg.GetValue("v1"));
            Assert.AreEqual(" A:x$ ", cfg.GetValue("v2"));
            Assert.AreEqual(" #A :x# ", cfg.GetValue("v3"));
            Assert.AreEqual(" #A: x# ", cfg.GetValue("v4"));
            Assert.AreEqual(" # A:x# ", cfg.GetValue("v5"));
            Assert.AreEqual(" #A:x # ", cfg.GetValue("v6"));
            Assert.AreEqual(" #A:y# ", cfg.GetValue("v7"));
            Assert.AreEqual(" #B:x# ", cfg.GetValue("v8"));
        }

        [Test]
        public void TransitiveGroupedVariableExpansion()
        {
            var groupedProperties = new GroupedPropertyCollection(new Dictionary<string, IDictionary<string, object>>
            {
                { "A", new Dictionary<string, object> {
                    { "x1", "$A:x2$" },
                    { "x2", "123" },
                    { "y1", "$B:y2$" }
                } },
                { "B", new Dictionary<string, object>
                {
                    { "y2", "$B:y3$" },
                    { "y3", "456" },
                } },
            });
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new GroupedVariableResolver(cfg));

            cfg.SetGroupValue("A", "x1", "$A:x2$");
            cfg.SetGroupValue("A", "x2", "$B:x3$");
            cfg.SetGroupValue("B", "x3", "$B:x4$");
            cfg.SetGroupValue("B", "x4", "123");

            Assert.AreEqual("123", cfg.GetGroupValue("A", "x1"));
        }

        [Test]
        public void InvalidGroupedVariableValue()
        {
            var groupedProperties = new GroupedPropertyCollection(new Dictionary<string, IDictionary<string, object>>
            {
                { "A", new Dictionary<string, object> {
                    { "x", 123 },
                    { "y", true },
                } },
            });
            var cfg = new ResolvingPropertyCollection();
            cfg.AddResolver(new GroupedVariableResolver(groupedProperties));
            cfg.SetValue("int", "$A:x$");
            cfg.SetValue("bool", "$A:y$");

            Assert.AreEqual("#A:x#", cfg.GetValue("int"));
            Assert.AreEqual("#A:y#", cfg.GetValue("bool"));
        }
    }
}
