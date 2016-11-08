using System;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JsonWebToken : IAuthenticator
    {
        private readonly OAuth2JsonWebTokenSigningTemplate _template;
        private readonly string _header;
        private readonly string _claims;
        private readonly JsonWebTokenAlgorithm _algorithm;
        private readonly string _privateKey;
        private readonly string _clientId;

        public OAuth2JsonWebToken(
            string header,
            string claims,
            JsonWebTokenAlgorithm algorithm,
            string privateKey,
            string clientId,
            IJsonWebTokenSigningFactory signingFactory)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (signingFactory == null) throw new ArgumentNullException(nameof(signingFactory));

            _header = header;
            _claims = claims;
            _algorithm = algorithm;
            _clientId = clientId;
            _privateKey = privateKey;

            _template = new OAuth2JsonWebTokenSigningTemplate(signingFactory);
        }

        public OAuth2JsonWebToken(
            string header,
            string claims,
            JsonWebTokenAlgorithm algorithm,
            string privateKey,
            string clientId) : this(
                header,
                claims,
                algorithm, 
                privateKey, 
                clientId, 
                new JsonWebTokenSignerFactory())
        {}

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            var signatureBase = _template.CreateSignatureBase(
                _header, 
                _claims);

            var signature = _template.CreateSignature(
                signatureBase,
                _algorithm,
                _privateKey);

            var token = StringExtensions.Concatenate(
                signatureBase, 
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
