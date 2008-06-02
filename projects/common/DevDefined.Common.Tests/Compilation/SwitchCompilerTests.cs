using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DevDefined.Common.Compilation;

namespace DevDefined.Common.Tests.Compilation
{
    [TestFixture]
    public class SwitchCompilerTests
    {
        [Test]
        public void SimpleTestFromJomoFishersSite()
        {
            var dict = new Dictionary<string, int> {

                    {"happy", 9}, {"sneezy", 2},

                    {"doc", 7}, {"sleepy", 8},

                    {"dopey", 9}, {"grumpy", 2},

                    {"bashful", 6}

                };

            var Lookup = SwitchCompiler.CreateSwitch(dict);

            Assert.AreEqual(7, Lookup("doc"));
        }
    }
}
