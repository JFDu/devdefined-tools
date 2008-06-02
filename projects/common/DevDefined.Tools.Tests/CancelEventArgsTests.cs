using System;
using NUnit.Framework;

namespace DevDefined.Tools.Tests
{
    [TestFixture]
    public class CancelEventArgsTests
    {
        [Test]
        public void CancelEventArgs()
        {
            CancelEventArgs args = new CancelEventArgs();
            Assert.IsFalse(args.Cancel);

            args.Cancel = true;
            Assert.IsTrue(args.Cancel);

            args = new CancelEventArgs(true);
            Assert.IsTrue(args.Cancel);
        }

        [Test]
        public void CancelEventArgsWithValue()
        {
            CancelEventArgs<string> args = new CancelEventArgs<string>();
            Assert.IsFalse(args.Cancel);
            Assert.IsNull(args.Value);

            args.Cancel = true;
            args.Value = "test";
            Assert.IsTrue(args.Cancel);
            Assert.AreEqual("test", args.Value);

            args = new CancelEventArgs<string>("test", true);
            Assert.IsTrue(args.Cancel);
            Assert.AreEqual("test", args.Value);
        }

        [Test]
        [ExpectedException(typeof (ApplicationException), "In the application's current environment you can not cancel this event")]
        public void CantCancelEventArgs()
        {
            CantCancelEventArgs args = new CantCancelEventArgs();
            args.Cancel = true;
        }

        [Test]
        [ExpectedException(typeof (ApplicationException),  "In the application's current environment you can not cancel this event")]
        public void CantCancelEventArgsWithValue()
        {
            CantCancelEventArgs<string> args = new CantCancelEventArgs<string>("test");
            Assert.AreEqual("test", args.Value);
            args.Cancel = true;
        }

        [Test]
        public void SetCantCancelEventArgsToFalse()
        {
            CantCancelEventArgs args = new CantCancelEventArgs();
            args.Cancel = false;
        }
    }
}