using System;
using System.Collections;
using Castle.MonoRail.Framework;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Core;
using DevDefined.OAuth;
using DevDefined.OAuth.Core;

namespace MonoRailsOAuth.Web.Controllers
{
    public class OAuthController : SmartDispatcherController
    {
        private readonly IOAuthService _oAuthService;
        
        private readonly TokenBase _testRequestToken = new TokenBase
        {
            ConsumerKey = "key",
            Token = "requestkey",
            TokenSecret = "requestsecret"
        };

        private readonly TokenBase _testAccessToekn = new TokenBase
        {
            ConsumerKey = "key",
            Token = "accesskey",
            TokenSecret = "accesssecret"
        };

        public OAuthController(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        public void RequestToken()
        {
            RenderText(_oAuthService.GrantRequestToken(Request).ToString());
        }

        public void AccessToken()
        {            
            RenderText(_oAuthService.ExchangeRequestTokenForAccessToken(Request).ToString());
        }

        public void Index(string signatureMethod)
        {
            // just populate the propertybag with a few uri's for testing purposes.
            
            signatureMethod = signatureMethod ?? SignatureMethod.PlainText;

            PropertyBag["signatureMethod"] = signatureMethod;

            OAuthConsumer consumer = _oAuthService.CreateConsumer(signatureMethod);

            PropertyBag["requestTokenUri"] = consumer.BuildRequestTokenContext(null).GenerateUrl();
            PropertyBag["exchangeRequestTokenForAccessTokenUri"] = consumer.BuildExchangeRequestTokenForAccessTokenContext(_testRequestToken, null).GenerateUrl();
            PropertyBag["accessProtectedResourceUri"] = CreateApiResourceRequest(consumer, "/api/contacts/");
            PropertyBag["accessOneProtectedResourceUri"] = CreateApiResourceRequest(consumer, "/api/contacts/1");
        }

        private string CreateApiResourceRequest(IOAuthConsumer consumer, string relativePath)
        {
            // to request a protected resource via GET we:

            // 1. Construct the uri we want to access
            // 2. create an "OAuthContext" representing the request.
            // 3. Sign the context using our OAuth consumer implementation (calculates and sets the signature etc.)
            // 4. Generate a Uri from the OAuthContext context and return it.

            
            var absoloutePath = new Uri(Request.Uri, relativePath);

            var factory = new OAuthContextFactory();

            OAuthContext context = factory.FromUri("GET", absoloutePath);

            consumer.SignContext(context, _testAccessToekn);

            return context.GenerateUrl();
        }
    }
}