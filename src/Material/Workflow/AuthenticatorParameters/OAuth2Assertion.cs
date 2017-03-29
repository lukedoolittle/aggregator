using System;
using Material.Domain.Credentials;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Keys;

namespace Material.Workflow.AuthenticatorParameters
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

            return _jsonWebToken.EncodedToken;
        }
    }
}
