using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2AccessToken : IAuthenticator
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Uri _redirectUrl;
        private readonly string _code;
        private readonly string _scope;

        public OAuth2AccessToken(
            string clientId,
            string clientSecret,
            Uri redirectUrl,
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

            if (redirectUrl == null)
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
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            requestBuilder
                .Parameter(
                    OAuth2ResponseType.Code.EnumToString(),
                    _code)
                .Parameter(
                    OAuth2Parameter.RedirectUri.EnumToString(),
                    _redirectUrl.ToString())
                .Parameter(
                    OAuth2Parameter.ClientId.EnumToString(),
                    _clientId)
                .Parameter(
                    OAuth2Parameter.Scope.EnumToString(),
                    _scope)
                .Parameter(
                    OAuth2Parameter.GrantType.EnumToString(),
                    GrantType.AuthCode.EnumToString());

            if (_clientSecret != null)
            {
                requestBuilder
                    .Parameter(
                        OAuth2Parameter.ClientSecret.EnumToString(),
                        _clientSecret);
            }
        }
    }
}
