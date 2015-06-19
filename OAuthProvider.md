# Introduction #

An OAuth Provider protects resources, forcing consumers to require an access token and to sign all requests as per the OAuth spec and selected signature method.

Implementing an OAuth provider with the library is more complex then a consumer.

# Overview #

The Provider implementation revolves around the OAuthProvider class - this class implements this interface:

```
public interface IOAuthProvider
{
    IToken GrantRequestToken(OAuthContext context);
    IToken ExchangeRequestTokenForAccessToken(OAuthContext context);
    void AccessProtectedResourceRequest(OAuthContext context);
}
```

The default provider implementation is initiated with a collection of inspectors, each inspector implements this interface:

```
public interface IContextInspector
{
    void InspectContext(OAuthContext context);
}
```

The inspectors take care of applying various concerns, out of the box are these inspectors:

  * ConsumerValidationInspector - validates a requests consumer key against the IConsumerStore.
  * NonceStoreInspector - validates a requests nonce against the INonceStore.
  * SignatureValidationInspector - validates a requests signature.
  * TimestampRangeInspector - validates the timestamp in a request against a preset time range from DateTime.Now() on the server.

For a production OAuth provider implementation you'd probably need to add some other inspectors to take care of concerns like blacklisted/whitelisted IP addresses or restrictions on which signature methods are permitted with which schemes (you probably want to prevent use of the plain text signature method without https).

# Implementation Steps #

  * 3 interfaces need to be implemented, for the 3 different stores required by the default provider implementation.
    * IConsumerStore - a store for consumer details.
    * ITokenStore - a store for request and access tokens.
    * INonceStore - a store for nonces, unique strings supplied in requests - the provider checks to ensure requests aren't replayed by storing previous nonces in the nonce store.
  * Construct an OAuthProvider instance, providing a set of inspectors.
  * Wire up the provider to pages/controller actions/WCF operations etc.

# Constructor Example #

```
var provider = new OAuthProvider(tokenStore,
    // list of inspectors
    new SignatureValidationInspector(consumerStore),
    new NonceStoreInspector(nonceStore),
    new TimestampRangeInspector(new TimeSpan(1, 0, 0)),
    new ConsumerValidationInspector(consumerStore)); 
```

# Accessing Protected Resource Example #

```
// Request is a System.Web.Http request property of an ASPX page in this case.

OAuthContext context = new OAuthContextBuilder().FromHttpRequest(Request);

try
{
    provider.AccessProtectedResourceRequest(context);
    // return the protected resource in some kind of representation.
}
catch (OAuthException authEx)
{
    Response.StatusCode = 403;

    // the OAuthException's Report property is of the type "OAuthProblemReport", it's ToString()
    // implementation is overloaded to return a problem report string as per
    // the error reporting OAuth extension: http://wiki.oauth.net/ProblemReporting

    Response.Write(authEx.Report.ToString());
    Response.End();
}
```