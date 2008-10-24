using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using DevDefined.OAuth.Core;

namespace DevDefined.OAuth.Consumer
{
    public interface IOAuthConsumer
    {
        string ConsumerKey { get; set; }
        string ConsumerSecret { get; set; }
        string SignatureMethod { get; set; }
        AsymmetricAlgorithm Key { get; set; }
        Uri RequestTokenUri { get; }
        Uri AccessTokenUri { get; }
        bool UseHeaderForOAuthParameters { get; set; }
        TokenBase RequestToken(NameValueCollection additionalQueryParameters);

        TokenBase ExchangeRequestTokenForAccessToken(TokenBase requestToken,
                                                     NameValueCollection additionalQueryParameters);

        HttpWebResponse GetResponse(OAuthContext context, TokenBase accessToken);
        OAuthContext BuildRequestTokenContext(NameValueCollection additionalQueryParameters);

        OAuthContext BuildExchangeRequestTokenForAccessTokenContext(TokenBase requestToken,
                                                                    NameValueCollection additionalQueryParameters);

        void SignContext(OAuthContext context, TokenBase accessToken);
    }
}