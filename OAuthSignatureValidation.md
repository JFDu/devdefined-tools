# Introduction #

If you are rolling your own custom OAuth provider you might not want to leverage the canned provider implementation that's in the library.  In this case you'll want to verify a signed request yourself - which is pretty easy to do.

# Details #

The steps are:

  * Create an OAuthContext instance from the request.
  * Create an OAuthSigningContext instance and assign it's ConsumerKey or Algorithm properties if required for the current consumer.
  * Create an OAuthContextSigner instance, which will be used to validate the signature.
  * Invoke the ValidateSignature method of the OAuthContextSigner class, passing in the context and signer context, this will return true if the signature is valid.

```
System.Web.HttpRequest request; // assuming request is for asp.net

OAuthContext context = new OAuthContextBuilder().FromHttpRequest(request);                        

OAuthContextSigner signer = new OAuthContextSigner();

SigningContext signingContext = new SigningContext();

// use context.ConsumerKey to fetch information required for signature validation for this consumer.

signingContext.Algorithm = ...; // if a certificate is associated with the consumer (for RSA-SHA1 etc.)
signingContext.ConsumerSecret = ...; // if there is a consumer secret

if (signer.ValidateSignature(context, signingContext))
{
    // signature was valid.
}
```