using System;

namespace DevDefined.OAuth.Core
{
    public class IssuedToken : TokenBase
    {
        public IssuedToken()
        {
            Issued = DateTime.Now;
        }

        public AllowableTokenUses AllowableUses { get; set; }

        public DateTime Issued { get; set; }

        public DateTime? Expires { get; set; }
    }
}