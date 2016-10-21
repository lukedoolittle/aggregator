using System;
using System.Text;
using Foundations.Cryptography.JsonWebToken;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JsonWebToken : IAuthenticator
    {
        private readonly OAuth2JsonWebTokenSigningTemplate _template;
        private readonly string _privateKey;
        private readonly string _clientId;

        public OAuth2JsonWebToken(
            JsonWebToken token,
            string privateKey,
            string clientId,
            IJsonWebTokenSigningFactory signingFactory)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (signingFactory == null) throw new ArgumentNullException(nameof(signingFactory));

            _clientId = clientId;
            _privateKey = privateKey;

            _template = new OAuth2JsonWebTokenSigningTemplate(
                token, 
                signingFactory);
        }

        public OAuth2JsonWebToken(
            JsonWebToken token,
            string privateKey,
            string clientId) : this(
                token, 
                privateKey, 
                clientId, 
                new JsonWebTokenSignerFactory())
        {}

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            var signatureBase = _template.CreateSignatureBase();
            var signature = _template.CreateSignature(
                signatureBase,
                _privateKey);

            var assertion = StringExtensions.Concatenate(
                signatureBase, 
                signature, 
                ".");

            requestBuilder
                .Parameter(
                    OAuth2Parameter.Assertion.EnumToString(),
                    assertion)
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
