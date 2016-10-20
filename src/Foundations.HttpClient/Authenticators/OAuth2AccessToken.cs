using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2AccessToken : IAuthenticator
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUrl;
        private readonly string _code;
        private readonly string _scope;

        public OAuth2AccessToken(
            string clientId,
            string clientSecret,
            string redirectUrl,
            string code,
            string scope)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                throw new ArgumentNullException(nameof(redirectUrl));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUrl = redirectUrl;
            _code = code;
            _scope = scope;
        }

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            requestBuilder
                .Parameter(
                    ResponseTypeEnum.Code.EnumToString(),
                    _code)
                .Parameter(
                    OAuth2ParameterEnum.RedirectUri.EnumToString(),
                    _redirectUrl)
                .Parameter(
                    OAuth2ParameterEnum.ClientId.EnumToString(),
                    _clientId)
                .Parameter(
                    OAuth2ParameterEnum.Scope.EnumToString(),
                    _scope)
                .Parameter(
                    OAuth2ParameterEnum.GrantType.EnumToString(),
                    GrantTypeEnum.AuthCode.EnumToString());

            if (_clientSecret != null)
            {
                requestBuilder
                    .Parameter(
                        OAuth2ParameterEnum.ClientSecret.EnumToString(),
                        _clientSecret);
            }
        }
    }
}
