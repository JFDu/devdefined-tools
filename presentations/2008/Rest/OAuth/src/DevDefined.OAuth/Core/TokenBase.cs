using System.Text;

namespace DevDefined.OAuth.Core
{
    public class TokenBase
    {
        public string TokenSecret { get; set; }

        public string Token { get; set; }

        public string ConsumerKey { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Parameters.OAuth_Token).Append("=").Append(UriUtility.UrlEncode(Token))
                .Append("&").Append(Parameters.OAuth_Token_Secret).Append("=").Append(UriUtility.UrlEncode(TokenSecret));

            return builder.ToString();
        }
    }
}