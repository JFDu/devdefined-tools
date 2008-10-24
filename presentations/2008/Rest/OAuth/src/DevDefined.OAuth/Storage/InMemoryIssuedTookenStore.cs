using System;
using System.Collections.Generic;
using DevDefined.OAuth.Core;

namespace DevDefined.OAuth.Storage
{
    public class InMemoryIssuedTookenStore : IIssuedTokenStore
    {
        private readonly Dictionary<string, IssuedToken> _tokens = new Dictionary<string, IssuedToken>();

        #region IIssuedTokenStore Members

        public virtual bool HasToken(string token)
        {
            return _tokens.ContainsKey(token);
        }

        public virtual IssuedToken GetToken(string token)
        {
            IssuedToken tokenData = _tokens[token];

            if (!IsUsable(tokenData))
            {
                throw Error.TokenCanNoLongerBeUsed(token);
            }

            if (tokenData.AllowableUses != AllowableTokenUses.Unlimited)
            {
                _tokens.Remove(token);
            }

            return tokenData;
        }

        public virtual bool IsUsable(IssuedToken token)
        {
            return (!token.Expires.HasValue || (token.Expires.Value >= DateTime.Now))
                   && (token.AllowableUses != AllowableTokenUses.None);
        }

        public IssuedToken IssueReusableToken(string consumerKey)
        {
            IssuedToken tokenData = IssueToken(consumerKey);
            tokenData.AllowableUses = AllowableTokenUses.Unlimited;
            return tokenData;
        }

        public IssuedToken IssueOneUseToken(string consumerKey)
        {
            IssuedToken tokenData = IssueToken(consumerKey);
            tokenData.AllowableUses = AllowableTokenUses.Unlimited;
            return tokenData;
        }

        #endregion

        public virtual IssuedToken IssueToken(string consumerKey)
        {
            string token = Guid.NewGuid().ToString();

            var tokenData = new IssuedToken
                                {
                                    Token = token,
                                    TokenSecret = Guid.NewGuid().ToString(),
                                    ConsumerKey = consumerKey
                                };

            _tokens[token] = tokenData;

            return tokenData;
        }
    }
}