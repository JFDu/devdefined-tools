using System;

namespace DevDefined.OAuth
{
    public class NonceGenerator
    {
        protected Random random = new Random();

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateNonce()
        {
            // Just a simple implementation of a random number between 123400 and 9999999
            return random.Next(123400, 9999999).ToString();
        }
    }
}