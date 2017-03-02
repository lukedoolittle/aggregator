using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class JsonWebTokenNonceValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly string _nonce;

        public JsonWebTokenNonceValidator(
            IOAuthSecurityStrategy securityStrategy,
            string userId)
        {
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            _nonce = securityStrategy.GetSecureParameter(
                userId,
                OAuth2Parameter.Nonce.EnumToString());
        }

        public JsonWebTokenNonceValidator(string nonce)
        {
            if (nonce == null) throw new ArgumentNullException(nameof(nonce));

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
