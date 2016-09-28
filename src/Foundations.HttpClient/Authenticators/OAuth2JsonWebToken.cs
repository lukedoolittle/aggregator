using Foundations.Cryptography.JsonWebToken;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2JsonWebToken : IAuthenticator
    {
        private readonly OAuth2JwtSigningTemplate _template;
        private readonly string _privateKey;
        private readonly string _clientId;

        public OAuth2JsonWebToken(
            JsonWebToken token,
            string privateKey,
            string clientId,
            IJwtSigningFactory signingFactory = null)
        {
            _clientId = clientId;
            _privateKey = privateKey;

            var factory = signingFactory ?? new JwtSignerFactory();
            _template = new OAuth2JwtSigningTemplate(token, factory);
        }

        public void Authenticate(HttpRequest request)
        {
            var signatureBase = _template.CreateSignatureBase();
            var signature = _template.CreateSignature(
                signatureBase,
                _privateKey);
            var assertion = $"{signatureBase}.{signature}";

            request
                .Parameter(
                    OAuth2ParameterEnum.Assertion.EnumToString(),
                    assertion)
                 .Parameter(
                    OAuth2ParameterEnum.GrantType.EnumToString(),
                    GrantTypeEnum.JsonWebToken.EnumToString());

            if (_clientId != null)
            {
                request.Parameter(
                    OAuth2ParameterEnum.ClientId.EnumToString(),
                    _clientId);
            }
        }
    }
}
