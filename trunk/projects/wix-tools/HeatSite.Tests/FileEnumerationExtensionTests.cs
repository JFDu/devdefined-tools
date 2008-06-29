using System.Linq;
using NUnit.Framework;

namespace HeatSite.Tests
{
    [TestFixture]
    public class FileEnumerationExtensionTests
    {
        [Test]
        public void ToRelative_WhenPathHasNoTrailingSpace_CorrectlySetsPath()
        {
            var files = new[] {"C:\\temp\\test.html"};
            Assert.AreEqual("test.html", files.ToRelative("C:\\temp").First());
        }

        [Test]
        public void ToRelative_WhenPathHasTrailingSpace_CorrectlySetsPath()
        {
            var files = new[] {"C:\\temp\\test.html"};
            Assert.AreEqual("test.html", files.ToRelative("C:\\temp\\").First());
        }
    }
}