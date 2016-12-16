using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2Assertion : IAuthenticatorParameter
    {
        private readonly JsonWebToken _jsonWebToken;
        private readonly CryptoKey _privateKey;
        private readonly IJsonWebTokenSigningFactory _signingFactory;

        public string Name => OAuth2Parameter.Assertion.EnumToString();
        public string Value => GetSignedToken();
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2Assertion(
            JsonWebToken jsonWebToken, 
            CryptoKey privateKey, 
            IJsonWebTokenSigningFactory signingFactory)
        {
            if (jsonWebToken == null) throw new ArgumentNullException(nameof(jsonWebToken));
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (signingFactory == null) throw new ArgumentNullException(nameof(signingFactory));

            _jsonWebToken = jsonWebToken;
            _privateKey = privateKey;
            _signingFactory = signingFactory;
        }

        private string GetSignedToken()
        {
            _jsonWebToken.Sign(_signingFactory, _privateKey);

            return _jsonWebToken.Signature;
        }
    }
}
