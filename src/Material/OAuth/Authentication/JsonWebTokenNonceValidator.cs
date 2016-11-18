using System;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class JsonWebTokenNonceValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly string _nonce;

        public JsonWebTokenNonceValidator(string nonce)
        {
            _nonce = nonce;
        }

        public TokenValidationResult IsTokenValid(
            JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (token.Claims.Nonce != _nonce)
            {
                return new TokenValidationResult(
                    false,
                    StringResources.InvalidJsonWebTokenNonce);
            }

            return new TokenValidationResult(true);
        }
    }
}
