# Introduction #

A consumer is a client library which communicates with http resources secured with OAuth.  The Consumer classes can be found in the DevDefined.OAuth.Consumer namespace.

# Workflow #

To consume a service, such as google contacts you generally follow these steps:

  1. Create an OAuthConsumerContext - this holds information about the consumer, such as:
    * ConsumerKey
    * ConsumerSecret
    * Key (i.e. the PrivateKey of an x509 Certificate)
    * SignatureMethod (the method used to sign requests)
  1. Create an OAuthSession associated with the OAuthConsumerContext which includes information related to the service being consumed, this is normally the:
    * RequestTokenUri (the address you can get a Request token from)
    * UserAuthorizeUri (the address you send a user to on the provider site, so they can grant the consumer access)
    * AccessTokenUri (the address you can submit a request token, to exchange it for an access token).
  1. Set some parameters that should be applied to all requests made with the session, for a google service this would normally be the "scope" parameter.
  1. Call Request() on the session to retrieve a request token.
  1. Call GetUserAuthorizationUrlForToken to create a url for user authorization, redirect the user to the url.
  1. Upon return, exchange the request token for an access token using the ExchangeRequestTokenForAccessToken method.
  1. For all subsequent requests set the AccessToken property of the session and then make requests using the Request() method.

# Example #

```
string requestUrl = "https://www.google.com/accounts/OAuthGetRequestToken";
string userAuthorizeUrl = "https://www.google.com/accounts/accounts/OAuthAuthorizeToken";
string accessUrl = "https://www.google.com/accounts/OAuthGetAccessToken";
string callBackUrl = "http://www.mysite.com/callback";

var consumerContext = new OAuthConsumerContext
{
    ConsumerKey = "weitu.googlepages.com",
    SignatureMethod = SignatureMethod.RsaSha1,
    Key = certificate.PrivateKey
};

var session = new OAuthSession(consumerContext, requestUrl, userAuthorizeUrl, accessUrl)           
    .WithQueryParameters(new { scope = "http://www.google.com/m8/feeds" });

// get a request token from the provider
IToken requestToken = session.GetRequestToken();

// generate a user authorize url for this token (which you can use in a redirect from the current site)
string authorizationLink = session.GetUserAuthorizationUrlForToken(requestToken, callBackUrl);

// exchange a request token for an access token
IToken accessToken = session.ExchangeRequestTokenForAccessToken(requestToken);

// make a request for a protected resource
string responseText = session.Request().Get().ForUrl("http://www.google.com/m8/feeds/contacts/default/base").ToString();
```