using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2ProtectedResource : IAuthorizer
    {
        private readonly string _accessToken;
        private readonly string _accessTokenName;

        public OAuth2ProtectedResource(
            string accessToken, 
            string accessTokenName)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            if (string.IsNullOrEmpty(accessTokenName))
            {
                throw new ArgumentNullException(nameof(accessTokenName));
            }

            _accessToken = accessToken;
            _accessTokenName = accessTokenName;
        }

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            if (string.Equals(
                    _accessTokenName, 
                    OAuth2Parameter.BearerHeader.EnumToString(), 
                    StringComparison.CurrentCultureIgnoreCase))
            {
                requestBuilder.Bearer(_accessToken);
            }
            else
            {
                requestBuilder.Parameter(
                    _accessTokenName,
                    _accessToken);
            }
        }
    }
}
