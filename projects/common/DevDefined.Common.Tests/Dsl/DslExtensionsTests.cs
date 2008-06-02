using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DevDefined.Common.Dsl;

namespace DevDefined.Common.Tests.Dsl
{
    [TestFixture]
    public class DslExtensionsTests
    {
        [Test]
        public void IgnoreBatchSupport()
        {
            Batch batch = delegate { return null; };
            Assert.IsFalse(batch.IsIgnored());
            batch.Ignore();
            Assert.IsTrue(batch.IsIgnored());
        }
    }
}
