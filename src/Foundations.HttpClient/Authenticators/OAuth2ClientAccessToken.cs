using System;
using System.Net;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2ClientAccessToken : IAuthenticator
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public OAuth2ClientAccessToken(
            string clientId,
            string clientSecret)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public void Authenticate(HttpRequest request)
        {
            request
                .Header(
                    HttpRequestHeader.Authorization, 
                    CreateHeader())
                .Parameter(
                    OAuth2ParameterEnum.GrantType.EnumToString(),
                    GrantTypeEnum.ClientCredentials.EnumToString());
        }

        public string CreateHeader()
        {
            var key = $"{_clientId}:{_clientSecret}".ToBase64String();
            return $"{OAuth2ParameterEnum.BasicHeader.EnumToString()} {key}";
        }
    }
}
