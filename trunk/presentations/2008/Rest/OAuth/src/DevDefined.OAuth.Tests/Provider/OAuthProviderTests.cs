using System;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Core;
using DevDefined.OAuth.Provider;
using NUnit.Framework;

namespace DevDefined.OAuth.Tests.Provider
{
    [TestFixture]
    public class OAuthProviderTests
    {
        private readonly OAuthProvider provider = new OAuthProvider
                                                      {
                                                          TestAlgorithm =
                                                              TestCertificates.OAuthTestCertificate().PublicKey.Key
                                                      };

        private OAuthConsumer CreateConsumer(string signatureMethod)
        {
            return new OAuthConsumer("http://localhost/requesttoken.rails", "http://localhost/accesstoken.rails")
                       {
                           SignatureMethod = signatureMethod,
                           ConsumerKey = "key",
                           ConsumerSecret = "secret",
                           Key = TestCertificates.OAuthTestCertificate().PrivateKey
                       };
        }

        [Test]
        public void AccessProtectedResource()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.RsaSha1);
            var contextFactory = new OAuthContextFactory();
            OAuthContext context = contextFactory.FromUri("GET", new Uri("http://localhost/protected.rails"));
            consumer.SignContext(context,
                                 new TokenBase {ConsumerKey = "key", Token = "accesskey", TokenSecret = "accesssecret"});
            AccessOutcome outcome = provider.VerifyProtectedResourceRequest(context);
            Assert.IsTrue(outcome.Granted, outcome.AdditionalInfo);
            Assert.IsNotNull(outcome.AccessToken);
        }

        [Test]
        public void ExchangeRequestTokenForAccessToken()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.RsaSha1);
            OAuthContext context =
                consumer.BuildExchangeRequestTokenForAccessTokenContext(
                    new TokenBase {ConsumerKey = "key", Token = "requestkey", TokenSecret = "requestsecret"}, null);
            TokenBase accessToken = provider.ExchangeRequestTokenForAccessToken(context);
            Assert.AreEqual("accesskey", accessToken.Token);
            Assert.AreEqual("accesssecret", accessToken.TokenSecret);
        }

        [Test]
        public void ExchangeRequestTokenForAccessTokenPlainText()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.PlainText);
            OAuthContext context =
                consumer.BuildExchangeRequestTokenForAccessTokenContext(
                    new TokenBase {ConsumerKey = "key", Token = "requestkey", TokenSecret = "requestsecret"}, null);
            TokenBase accessToken = provider.ExchangeRequestTokenForAccessToken(context);
            Assert.AreEqual("accesskey", accessToken.Token);
            Assert.AreEqual("accesssecret", accessToken.TokenSecret);
        }

        [Test]
        public void RequestTokenWithHmacSha1()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.HmacSha1);
            OAuthContext context = consumer.BuildRequestTokenContext(null);
            TokenBase token = provider.GrantRequestToken(context);
            Assert.AreEqual("requestkey", token.Token);
            Assert.AreEqual("requestsecret", token.TokenSecret);
        }

        [Test]
        [ExpectedException]
        public void RequestTokenWithHmacSha1WithInvalidSignatureThrows()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.HmacSha1);
            OAuthContext context = consumer.BuildRequestTokenContext(null);
            context.Signature = "wrong";
            provider.GrantRequestToken(context);
        }

        [Test]
        [ExpectedException]
        public void RequestTokenWithInvalidConsumerKeyThrowsException()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.PlainText);
            consumer.ConsumerKey = "invalid";
            OAuthContext context = consumer.BuildRequestTokenContext(null);
            provider.GrantRequestToken(context);
        }

        [Test]
        public void RequestTokenWithPlainText()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.PlainText);
            OAuthContext context = consumer.BuildRequestTokenContext(null);
            TokenBase token = provider.GrantRequestToken(context);
            Assert.AreEqual("requestkey", token.Token);
            Assert.AreEqual("requestsecret", token.TokenSecret);
        }

        [Test]
        public void RequestTokenWithRsaSha1()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.RsaSha1);
            OAuthContext context = consumer.BuildRequestTokenContext(null);
            TokenBase token = provider.GrantRequestToken(context);
            Assert.AreEqual("requestkey", token.Token);
            Assert.AreEqual("requestsecret", token.TokenSecret);
        }

        [Test]
        [ExpectedException]
        public void RequestTokenWithRsaSha1WithInvalidSignatureThrows()
        {
            OAuthConsumer consumer = CreateConsumer(SignatureMethod.RsaSha1);
            OAuthContext context = consumer.BuildRequestTokenContext(null);
            context.Signature =
                "eeh8hLNIlNNq1Xrp7BOCc+xgY/K8AmjxKNM7UdLqqcvNSmJqcPcf7yQIOvu8oj5R/mDvBpSb3+CEhxDoW23gggsddPIxNdOcDuEOenugoCifEY6nRz8sbtYt3GHXsDS2esEse/N8bWgDdOm2FRDKuy9OOluQuKXLjx5wkD/KYMY=";
            provider.GrantRequestToken(context);
        }
    }
}