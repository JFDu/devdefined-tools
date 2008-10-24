using System;
using Castle.MonoRail.Framework;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Core;
using DevDefined.OAuth.Provider;

namespace DevDefined.OAuth
{
    public interface IOAuthService
    {
        Uri RequestTokenUrl { get; }
        Uri AccessTokenUrl { get; }
        TokenBase GrantRequestToken(IRequest request);
        TokenBase ExchangeRequestTokenForAccessToken(IRequest request);
        OAuthConsumer CreateConsumer(string signatureMethod);
        AccessOutcome AccessProtectedResource(IRequest request);
    }
}