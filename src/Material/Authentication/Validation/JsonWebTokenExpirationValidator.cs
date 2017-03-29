using System;
using Material.Domain.Credentials;
using Material.Contracts;

namespace Material.Authentication.Validation
{
    public class JsonWebTokenExpirationValidator : IJsonWebTokenAuthenticationValidator
    {
        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (DateTime.Now > token.Claims.ExpirationTime)
            {
                return new TokenValidationResult(
                    false,
                    StringResources.WebTokenExpired);
            }

            return new TokenValidationResult(true);
        }
    }
}
