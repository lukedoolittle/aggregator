using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using Material.Contracts;
using Material.HttpClient.Cryptography.Enums;

namespace Material.Authentication.Validation
{
    public class JsonWebTokenAlgorithmValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly List<JsonWebTokenAlgorithm> _whitelistedAlgorithms;

        public JsonWebTokenAlgorithmValidator()
        {
            _whitelistedAlgorithms = QuantfabricConfiguration.WhitelistedAuthenticationAlgorithms;
        }

        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (!_whitelistedAlgorithms.Contains(token.Header.Algorithm))
            {
                return new TokenValidationResult(
                    false,
                    StringResources.InvalidJsonWebTokenAlgorithm);
            }

            return new TokenValidationResult(true);
        }
    }
}
