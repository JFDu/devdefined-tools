using System;
using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DevDefined.Common.Extensions;

namespace DevDefined.Common.Tests.Extensions
{
    [TestFixture]
    public class CommonExtensionsTests
    {
        [Test]
        public void LoopTo()
        {
            10.LoopTo(20) (i => i.PrintLine());
        }

        [Test]
        public void ToProjectedDictionaryOfLists()
        {
            var postTag = new[] {new { PostUrl="/blog/firstpost/", Tag=".Net"},
                            new { PostUrl="/blog/firstpost/", Tag="Tools"},
                            new { PostUrl="/blog/secondpost/", Tag="Travel"}};


            var organisedByTag = postTag.ToProjectedDictionaryOfLists
                (
                    pt => pt.PostUrl,
                    pt => pt.Tag
                );

            Assert.AreEqual(2, organisedByTag["/blog/firstpost/"].Count);
            Assert.IsTrue(organisedByTag["/blog/firstpost/"].Contains(".Net"));
            Assert.IsTrue(organisedByTag["/blog/firstpost/"].Contains("Tools"));

            Assert.AreEqual(1, organisedByTag["/blog/secondpost/"].Count);
            Assert.IsTrue(organisedByTag["/blog/secondpost/"].Contains("Travel"));
        }
    }
}
