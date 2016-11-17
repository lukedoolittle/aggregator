using System;
using System.Collections.Generic;
using Foundations.HttpClient.Cryptography.Enums;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class JsonWebTokenAlgorithmValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly List<JsonWebTokenAlgorithm> _whitelistedAlgorithms;

        public JsonWebTokenAlgorithmValidator()
        {
            _whitelistedAlgorithms = AuthenticationConfiguration.WhitelistedAlgorithms;
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
