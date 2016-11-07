using System;
using System.Net;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2ClientAccessToken : IAuthorizer
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

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            requestBuilder
                .Header(
                    HttpRequestHeader.Authorization, 
                    CreateHeader())
                .Parameter(
                    OAuth2Parameter.GrantType.EnumToString(),
                    GrantType.ClientCredentials.EnumToString());
        }

        public string CreateHeader()
        {
            var key = StringExtensions.Concatenate(
                _clientId, 
                _clientSecret,
                ":").ToBase64String();

            return StringExtensions.Concatenate(
                OAuth2Parameter.BasicHeader.EnumToString(),
                key,
                " ");
        }
    }
}
