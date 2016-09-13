using System;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class OAuth2ProtectedResource : IAuthenticator
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

        public void Authenticate(HttpRequest request)
        {
            if (String.Equals(
                    _accessTokenName, 
                    OAuth2ParameterEnum.BearerHeader.EnumToString(), 
                    StringComparison.CurrentCultureIgnoreCase))
            {
                request.Bearer(_accessToken);
            }
            else
            {
                request.Parameter(
                    _accessTokenName,
                    _accessToken);
            }
        }
    }
}
