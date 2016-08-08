using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2RefreshToken : IAuthenticator
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private readonly GrantTypeEnum _grantType;

        public OAuth2RefreshToken(
            string clientId, 
            string clientSecret, 
            string refreshToken)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            _grantType = GrantTypeEnum.RefreshToken;
        }

        public void Authenticate(HttpRequest request)
        {
            request.
                Parameter(
                    OAuth2ParameterEnum.ClientId.EnumToString(),
                    _clientId)
                .Parameter(
                    OAuth2ParameterEnum.ClientSecret.EnumToString(),
                    _clientSecret)
                .Parameter(
                    OAuth2ParameterEnum.GrantType.EnumToString(),
                    _grantType.EnumToString())
                .Parameter(
                    GrantTypeEnum.RefreshToken.EnumToString(),
                    _refreshToken);
        }
    }
}
