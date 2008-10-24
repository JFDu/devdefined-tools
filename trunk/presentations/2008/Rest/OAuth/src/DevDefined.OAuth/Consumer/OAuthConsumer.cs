using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using DevDefined.OAuth.Core;

namespace DevDefined.OAuth.Consumer
{
    public class OAuthConsumer : IOAuthConsumer
    {
        public OAuthConsumer(Uri requestTokenUri, Uri accessTokenUri)
        {
            RequestTokenUri = requestTokenUri;
            AccessTokenUri = accessTokenUri;
            SignatureMethod = Core.SignatureMethod.PlainText;
        }

        public OAuthConsumer(string requestTokenUrl, string accessTokenUrl)
            : this(new Uri(requestTokenUrl), new Uri(accessTokenUrl))
        {
        }

        #region IOAuthConsumer Members

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string SignatureMethod { get; set; }
        public AsymmetricAlgorithm Key { get; set; }

        public Uri RequestTokenUri { get; private set; }
        public Uri AccessTokenUri { get; private set; }

        public bool UseHeaderForOAuthParameters { get; set; }

        public TokenBase RequestToken(NameValueCollection additionalQueryParameters)
        {
            OAuthContext context = BuildRequestTokenContext(additionalQueryParameters);

            TokenBase token = context.InvokeHttpWebRequest(collection =>
                                                           new TokenBase
                                                               {
                                                                   ConsumerKey = ConsumerKey,
                                                                   Token =
                                                                       ParseResponseParameter(collection,
                                                                                              Parameters.OAuth_Token),
                                                                   TokenSecret =
                                                                       ParseResponseParameter(collection,
                                                                                              Parameters.
                                                                                                  OAuth_Token_Secret)
                                                               });

            return token;
        }

        public TokenBase ExchangeRequestTokenForAccessToken(TokenBase requestToken,
                                                            NameValueCollection additionalQueryParameters)
        {
            OAuthContext context = BuildExchangeRequestTokenForAccessTokenContext(requestToken,
                                                                                  additionalQueryParameters);

            TokenBase accessToken = context.InvokeHttpWebRequest(collection =>
                                                                 new TokenBase
                                                                     {
                                                                         ConsumerKey = requestToken.ConsumerKey,
                                                                         Token =
                                                                             ParseResponseParameter(collection,
                                                                                                    Parameters.
                                                                                                        OAuth_Token),
                                                                         TokenSecret =
                                                                             ParseResponseParameter(collection,
                                                                                                    Parameters.
                                                                                                        OAuth_Token_Secret)
                                                                     });

            return accessToken;
        }

        public HttpWebResponse GetResponse(OAuthContext context, TokenBase accessToken)
        {
            SignContext(context, accessToken);

            Uri uri = context.GenerateUri();

            Console.WriteLine("Uri: {0}", uri);

            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = context.RequestMethod;

            if ((context.FormEncodedParameters != null) && (context.FormEncodedParameters.Count > 0))
            {
                request.ContentType = "application/x-www-form-urlencoded";
                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(UriUtility.FormatQueryString(context.FormEncodedParameters));
                }
            }

            if (UseHeaderForOAuthParameters)
            {
                request.Headers[Parameters.OAuth_Authorization_Header] = context.GenerateOAuthParametersForHeader();
            }

            return (HttpWebResponse) request.GetResponse();
        }

        public OAuthContext BuildRequestTokenContext(NameValueCollection additionalQueryParameters)
        {
            EnsureStateIsValid();

            var auth = new NonceGenerator();

            var factory = new OAuthContextFactory();
            var signer = new OAuthContextSigner();
            OAuthContext context = factory.FromUri("GET", RequestTokenUri);

            if (additionalQueryParameters != null) context.QueryParameters.Add(additionalQueryParameters);

            context.ConsumerKey = ConsumerKey;
            context.RequestMethod = "GET";
            context.SignatureMethod = SignatureMethod;
            context.Timestamp = DateTime.Now.EpocString();
            context.Nonce = auth.GenerateNonce();
            context.Version = "1.0";

            string signatureBase = context.GenerateSignatureBase();

            Console.WriteLine("signature_base: {0}", signatureBase);

            signer.SignContext(context,
                               new SigningContext
                                   {Algorithm = Key, SignatureBase = signatureBase, ConsumerSecret = ConsumerSecret});

            Console.WriteLine("oauth_singature: {0}", context.Signature);

            Uri uri = context.GenerateUri();

            Console.WriteLine("Uri: {0}", uri);

            return context;
        }

        public OAuthContext BuildExchangeRequestTokenForAccessTokenContext(TokenBase requestToken,
                                                                           NameValueCollection additionalQueryParameters)
        {
            EnsureStateIsValid();

            if (requestToken.ConsumerKey != ConsumerKey)
                throw Error.SuppliedTokenWasNotIssuedToThisConsumer(ConsumerKey, requestToken.ConsumerKey);

            var auth = new NonceGenerator();

            var factory = new OAuthContextFactory();
            var signer = new OAuthContextSigner();
            OAuthContext context = factory.FromUri("GET", AccessTokenUri);

            if (additionalQueryParameters != null)
                context.QueryParameters.Add(additionalQueryParameters);

            context.ConsumerKey = ConsumerKey;
            context.Token = requestToken.Token;
            context.TokenSecret = requestToken.TokenSecret;
            context.RequestMethod = "GET";
            context.SignatureMethod = SignatureMethod;
            context.Timestamp = DateTime.Now.EpocString();
            context.Nonce = auth.GenerateNonce();
            context.Version = "1.0";

            string signatureBase = context.GenerateSignatureBase();

            Console.WriteLine("signature_base: {0}", signatureBase);

            signer.SignContext(context,
                               new SigningContext
                                   {Algorithm = Key, SignatureBase = signatureBase, ConsumerSecret = ConsumerSecret});

            Console.WriteLine("oauth_singature: {0}", context.Signature);

            Uri uri = context.GenerateUri();

            Console.WriteLine("Uri: {0}", uri);

            return context;
        }

        public void SignContext(OAuthContext context, TokenBase accessToken)
        {
            EnsureStateIsValid();

            if (accessToken.ConsumerKey != ConsumerKey)
                throw Error.SuppliedTokenWasNotIssuedToThisConsumer(ConsumerKey, accessToken.ConsumerKey);

            var signer = new OAuthContextSigner();
            var auth = new NonceGenerator();

            context.UseAuthorizationHeader = UseHeaderForOAuthParameters;
            context.ConsumerKey = accessToken.ConsumerKey;
            context.Token = accessToken.Token;
            context.TokenSecret = accessToken.TokenSecret;
            context.SignatureMethod = SignatureMethod;
            context.Timestamp = DateTime.Now.EpocString();
            context.Nonce = auth.GenerateNonce();
            context.Version = "1.0";

            string signatureBase = context.GenerateSignatureBase();

            Console.WriteLine("signature_base: {0}", signatureBase);

            signer.SignContext(context,
                               new SigningContext
                                   {Algorithm = Key, SignatureBase = signatureBase, ConsumerSecret = ConsumerSecret});

            Console.WriteLine("oauth_singature: {0}", context.Signature);
        }

        #endregion

        private void EnsureStateIsValid()
        {
            if (string.IsNullOrEmpty(ConsumerKey)) throw Error.InvalidConsumerKey(ConsumerKey);
            if (string.IsNullOrEmpty(SignatureMethod)) throw Error.UnknownSignatureMethod(SignatureMethod);
            if ((SignatureMethod == Core.SignatureMethod.RsaSha1)
                && (Key == null)) throw Error.ForRsaSha1SignatureMethodYouMustSupplyAssymetricKeyParameter();
        }

        private static string ParseResponseParameter(NameValueCollection collection, string parameter)
        {
            string value = (collection[parameter] ?? "").Trim();
            return (value.Length > 0) ? value : null;
        }
    }
}