using System;
using Material.Domain.Credentials;
using Material.Contracts;

namespace Material.Authentication.Validation
{
    public class JsonWebTokenAudienceValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly string _clientId;

        public JsonWebTokenAudienceValidator(string clientId)
        {
            _clientId = clientId;
        }

        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (token.Claims.Audience != _clientId)
            {
                return new TokenValidationResult(
                    false,
                    StringResources.InvalidJsonWebTokenAudience);
            }

            return new TokenValidationResult(true);
        }
    }
}
