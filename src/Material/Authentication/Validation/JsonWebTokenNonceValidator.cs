using System;
using Material.Domain.Credentials;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace Material.Authentication.Validation
{
    public class JsonWebTokenNonceValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public JsonWebTokenNonceValidator(
            IOAuthSecurityStrategy securityStrategy,
            string userId)
        {
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            _securityStrategy = securityStrategy;
            _userId = userId;
        }

        public TokenValidationResult IsTokenValid(
            JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var nonce = _securityStrategy.GetSecureParameter(
                _userId,
                OAuth2Parameter.Nonce.EnumToString());

            if (token.Claims.Nonce != nonce)
            {
                return new TokenValidationResult(
                    false,
                    StringResources.InvalidJsonWebTokenNonce);
            }

            return new TokenValidationResult(true);
        }
    }
}
