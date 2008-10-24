using System;
using System.Security.Cryptography;
using DevDefined.OAuth.Core;

namespace DevDefined.OAuth.Provider
{
    public class OAuthProvider : IOAuthProvider
    {
        private readonly OAuthContextSigner _signer = new OAuthContextSigner();
        public AsymmetricAlgorithm TestAlgorithm { get; set; }

        #region IOAuthProvider Members

        public TokenBase GrantRequestToken(OAuthContext context)
        {
            SigningContext signingContext = CreateSignatureContextForAuthContext(context);

            if (!_signer.ValidateSignature(context, signingContext))
            {
                throw Error.FailedToValidateSignature();
            }

            return new TokenBase
                       {
                           ConsumerKey = context.ConsumerKey,
                           Token = "requestkey",
                           TokenSecret = "requestsecret"
                       };
        }

        public TokenBase ExchangeRequestTokenForAccessToken(OAuthContext context)
        {
            SigningContext signingContext = CreateSignatureContextForAuthContext(context);

            if (!_signer.ValidateSignature(context, signingContext))
            {
                throw Error.FailedToValidateSignature();
            }

            if (context.Token != "requestkey")
            {
                throw Error.InvalidRequestToken(context.Token);
            }

            if (context.ConsumerKey != "key")
            {
                throw Error.InvalidConsumerKey(context.ConsumerKey);
            }

            return new TokenBase {ConsumerKey = context.ConsumerKey, Token = "accesskey", TokenSecret = "accesssecret"};
        }

        public AccessOutcome VerifyProtectedResourceRequest(OAuthContext context)
        {
            var outcome = new AccessOutcome {Context = context};
            
            SigningContext signingContext = null;
            
            try
            {
                signingContext = CreateSignatureContextForAuthContext(context);
            }
            catch (Exception ex)
            {
                outcome.AdditionalInfo = "Failed to parse request for context info";
                return outcome;
            }

            if (!_signer.ValidateSignature(context, signingContext))
            {
                outcome.AdditionalInfo = "Failed to validate signature";
                return outcome;
            }

            if (context.Token != "accesskey")
            {
                outcome.AdditionalInfo = "Invalid access token";
                return outcome;
            }

            if (context.ConsumerKey != "key")
            {
                outcome.AdditionalInfo = "Invalid consumer key";
                return outcome;
            }

            outcome.Granted = true;
            outcome.AccessToken = new TokenBase {ConsumerKey = "key", TokenSecret = "accesssecret", Token = "accesskey"};

            return outcome;
        }

        #endregion

        private SigningContext CreateSignatureContextForAuthContext(OAuthContext context)
        {
            return new SigningContext
                       {
                           ConsumerSecret = GetConsumerSecretForKey(context.ConsumerKey),
                           Algorithm = GetAlgorithmForConsumer(context.ConsumerKey)
                       };
        }

        private string GetConsumerSecretForKey(string consumerKey)
        {
            if (consumerKey == "key")
            {
                return "secret";
            }
            else
            {
                throw Error.InvalidConsumerKey(consumerKey);
            }
        }

        private AsymmetricAlgorithm GetAlgorithmForConsumer(string consumerKey)
        {
            if (consumerKey == "key")
            {
                return TestAlgorithm;
            }
            else
            {
                throw Error.InvalidConsumerKey(consumerKey);
            }
        }
    }
}