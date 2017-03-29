using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using Material.Contracts;

namespace Material.Authentication.Validation
{
    public class JsonWebTokenIssuerValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly List<string> _issuingAuthorities;

        public JsonWebTokenIssuerValidator(List<string> issuingAuthorities)
        {
            _issuingAuthorities = issuingAuthorities;
        }

        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (_issuingAuthorities != null)
            {
                if (!_issuingAuthorities.Contains(token.Claims.Issuer))
                {
                    return new TokenValidationResult(
                        false,
                        StringResources.InvalidJsonWebTokenIssuer);
                }
            }

            return new TokenValidationResult(true);
        }
    }
}
