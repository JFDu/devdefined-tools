using System;
using System.Collections;
using Castle.MonoRail.Framework;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Core;
using DevDefined.OAuth;
using DevDefined.OAuth.Core;
using DevDefined.OAuth.Provider;
using DevDefined.OAuth.Tests;

namespace MonoRailsOAuth.Web.Services
{
    public class TestOAuthService : IOAuthService
    {
        public OAuthContextFactory _contextFactory = new OAuthContextFactory();

        public OAuthProvider _provider = new OAuthProvider { TestAlgorithm = TestCertificates.OAuthTestCertificate().PublicKey.Key };

        #region IOAuthService Members

        public TokenBase GrantRequestToken(IRequest request)
        {
            OAuthContext context = _contextFactory.FromMonoRailRequest(request);
            return _provider.GrantRequestToken(context);
        }

        public TokenBase ExchangeRequestTokenForAccessToken(IRequest request)
        {
            OAuthContext context = _contextFactory.FromMonoRailRequest(request);
            return _provider.ExchangeRequestTokenForAccessToken(context);
        }

        public Uri RequestTokenUrl
        {
            get { return GetUriForAction("RequestToken"); }
        }

        public Uri AccessTokenUrl
        {
            get { return GetUriForAction("AccessToken"); }
        }

        public OAuthConsumer CreateConsumer(string signatureMethod)
        {
            return new OAuthConsumer(GetUriForAction("RequestToken"), GetUriForAction("AccessToken"))
                       {
                           ConsumerKey = "key",
                           ConsumerSecret = "secret",
                           Key = TestCertificates.OAuthTestCertificate().PrivateKey,
                           SignatureMethod = signatureMethod
                       };
        }

        public AccessOutcome AccessProtectedResource(IRequest request)
        {
            OAuthContext context = _contextFactory.FromMonoRailRequest(request);
            return _provider.VerifyProtectedResourceRequest(context);
        }

        #endregion

        private static Uri GetUriForAction(string actionName)
        {
            var context = MonoRailHttpHandlerFactory.CurrentEngineContext;

            string relative = context.Services.UrlBuilder.BuildUrl(context.UrlInfo,
                new Hashtable { { "controller", "oauth" }, { "action", actionName } },
                null);

            return new Uri(context.Request.Uri, relative);
        }
    }
}