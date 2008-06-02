using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DevDefined.Common.Dsl;

namespace DevDefined.Common.Tests.Dsl
{
    [TestFixture]
    public class NodeWriterTests
    {
        [Test]
        public void TestNodeWriter()
        {
            NodeWriter writer = new NodeWriter();

            writer.WriteStartNode(new NamedNode("table"))
                .WriteStartNode(new NamedNode("tr"))
                    .WriteStartNode(new NamedNode("td"))
                        .WriteNode(new TextNode("column1"))
                    .WriteEndNode()
                .WriteEndNode()
            .WriteEndNode();

            string expected = @"<?xml version=""1.0"" encoding=""utf-16""?><table><tr><td>column1</td></tr></table>";

            Assert.AreEqual(expected, DslToXml.ToXml(writer));
        }
    }
}
