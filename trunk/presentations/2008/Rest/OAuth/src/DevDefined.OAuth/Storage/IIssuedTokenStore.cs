using DevDefined.OAuth.Core;

namespace DevDefined.OAuth.Storage
{
    public interface IIssuedTokenStore
    {
        bool HasToken(string token);
        IssuedToken GetToken(string token);
        bool IsUsable(IssuedToken token);
        IssuedToken IssueReusableToken(string consumerKey);
        IssuedToken IssueOneUseToken(string consumerKey);
    }
}