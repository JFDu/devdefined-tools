using DevDefined.OAuth.Core;

namespace DevDefined.OAuth.Provider
{
    public interface IOAuthProvider
    {
        TokenBase GrantRequestToken(OAuthContext context);
        TokenBase ExchangeRequestTokenForAccessToken(OAuthContext context);
        AccessOutcome VerifyProtectedResourceRequest(OAuthContext context);
    }
}