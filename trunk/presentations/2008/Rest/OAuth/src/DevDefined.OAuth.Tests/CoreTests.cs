using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Core;
using NUnit.Framework;
using DevDefined.OAuth.Provider;

namespace DevDefined.OAuth.Tests
{
    [TestFixture]
    public class CoreTests
    {
        /*[Test]
        public void Go()
        {
            var factory = new OAuthContextFactory();
            var context = factory.FromUri("GET", new Uri("http://localhost.:888/api/contacts/list.rails"));
            
            OAuthConsumer consumer = new OAuthConsumer("http://localhost/rt", "http://localhost/at");
            consumer.SignatureMethod = SignatureMethod.RsaSha1;
            consumer.ConsumerKey = "key";
            consumer.ConsumerSecret = "secret";
            consumer.Key = TestCertificates.OAuthTestCertificate().PrivateKey;
            consumer.SignContext(context, new TokenBase { ConsumerKey="key", Token = "accesskey", TokenSecret = "accesssecret" });
            
            OAuthContext context2 = factory.FromUri("GET", context.GenerateUri());

            Assert.AreEqual(context.Signature, context2.Signature);

            OAuthProvider provider = new OAuthProvider();
            provider.TestAlgorithm = TestCertificates.OAuthTestCertificate().PublicKey.Key;
            var outcome = provider.VerifyProtectedResourceRequest(context2);

            Assert.IsTrue(outcome.Granted, outcome.AdditionalInfo);            
        }*/

        public void Test()
        {
            string signature = "RJfuUrAU7qFkBfk9nR02qw9YsdlJzaT2tYINzZ8b+H5dQLfcZqUjmgEjOvfqBo6HcDg3tFKcZgxsUF+zw5Tv4KWTuhPaAl7SFOmpvVzAA5Pn0pKSkivEl6oAvKQW7JP00/KQoR9gD7HrspFvzYsoqv3DlfGgLF7VYx62JzUPYr4=";

            OAuthContext context = new OAuthContext() { RequestMethod = "GET",  RawUri = new Uri("http://localhost/service") };

            context.Signature = signature;

            Console.WriteLine(context.GenerateUri().ToString());
        }
    }
}
