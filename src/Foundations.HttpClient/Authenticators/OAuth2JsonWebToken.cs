using System;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JsonWebToken : IAuthenticator
    {
        private readonly JsonWebTokenSigningTemplate _template;
        private readonly string _signatureBase;
        private readonly JsonWebTokenAlgorithm _algorithm;
        private readonly CryptoKey _privateKey;
        private readonly string _clientId;

        public OAuth2JsonWebToken(
            string signatureBase,
            JsonWebTokenAlgorithm algorithm,
            CryptoKey privateKey,
            string clientId,
            IJsonWebTokenSigningFactory signingFactory)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (signingFactory == null) throw new ArgumentNullException(nameof(signingFactory));

            _signatureBase = signatureBase;
            _algorithm = algorithm;
            _clientId = clientId;
            _privateKey = privateKey;

            _template = new JsonWebTokenSigningTemplate(signingFactory);
        }

        public OAuth2JsonWebToken(
            string signatureBase,
            JsonWebTokenAlgorithm algorithm,
            CryptoKey privateKey,
            string clientId) : this(
                signatureBase,
                algorithm, 
                privateKey, 
                clientId, 
                new JsonWebTokenSignerFactory())
        {}

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            var signature = _template.CreateSignature(
                _signatureBase,
                _algorithm,
                _privateKey);

            var token = StringExtensions.Concatenate(
                _signatureBase, 
                signature,
                ".");

            requestBuilder
                .Parameter(
                    OAuth2Parameter.Assertion.EnumToString(),
                    token)
                 .Parameter(
                    OAuth2Parameter.GrantType.EnumToString(),
                    GrantType.JsonWebToken.EnumToString());

            if (_clientId != null)
            {
                requestBuilder.Parameter(
                    OAuth2Parameter.ClientId.EnumToString(),
                    _clientId);
            }
        }
    }
}
