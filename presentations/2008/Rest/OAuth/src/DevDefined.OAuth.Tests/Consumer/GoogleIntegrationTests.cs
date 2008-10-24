using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Core;
using NUnit.Framework;

namespace DevDefined.OAuth.Tests.Consumer
{
    [TestFixture]
    public class GoogleIntegrationTests
    {
        private readonly X509Certificate2 certificate = TestCertificates.OAuthTestCertificate();

        [Test]
        public void RequestTokenForRsaSha1()
        {
            var consumer = new OAuthConsumer("https://www.google.com/accounts/OAuthGetRequestToken",
                                             "http://term.ie/oauth/example/access_token.php")
                               {
                                   ConsumerKey = "weitu.googlepages.com",
                                   SignatureMethod = SignatureMethod.RsaSha1,
                                   Key = certificate.PrivateKey
                               };

            var parameters = new NameValueCollection
                                 {
                                     {"scope", "http://www.google.com/m8/feeds"}
                                 };

            TokenBase token = consumer.RequestToken(parameters);
            Assert.AreEqual("weitu.googlepages.com", token.ConsumerKey);
            Assert.IsTrue(token.Token.Length > 0);
            Assert.IsNull(token.TokenSecret);
        }
    }
}