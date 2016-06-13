using Aggregator.Framework;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Extensions;
using RestSharp;
using RestSharp.Authenticators;

namespace Aggregator.Infrastructure.Authentication
{
    public class OAuth2Authenticator : IAuthenticator
    {
        public const string BEARER = "Bearer";

        private string _clientId;
        private string _clientSecret;
        private string _redirectUrl;
        private string _code;
        private string _accessToken;
        private string _accessTokenName;
        private string _refreshToken;
        private string _scope;
        private GrantTypeEnum _grantType;
        private bool _hasBasicAuthorization;
        private AuthType _type;

        public static IAuthenticator ForAccessToken(
            string clientId,
            string clientSecret,
            string redirectUrl,
            string code,
            string scope,
            bool hasBasicAuthorization)
        {
            return new OAuth2Authenticator
            {
                _clientId = clientId,
                _clientSecret = clientSecret,
                _redirectUrl = redirectUrl,
                _code = code,
                _scope = scope,
                _hasBasicAuthorization = hasBasicAuthorization,
                _grantType = GrantTypeEnum.AuthCode,
                _type = AuthType.AccessToken
            };
        }

        public static IAuthenticator ForRefreshToken(
            string clientId,
            string clientSecret,
            string refreshToken,
            bool hasBasicAuthorization)
        {
            return new OAuth2Authenticator
            {
                _clientId = clientId,
                _clientSecret = clientSecret,
                _refreshToken = refreshToken,
                _grantType = GrantTypeEnum.RefreshToken,
                _type = AuthType.RefreshToken,
                _hasBasicAuthorization = hasBasicAuthorization
            };
        }

        public static IAuthenticator ForProtectedResource(
            string accessToken,
            string accessTokenName)
        {
            return new OAuth2Authenticator
            {
                _accessToken = accessToken,
                _accessTokenName = accessTokenName,
                _type = AuthType.ProtectedResource,
            };
        }

        public void Authenticate(
            IRestClient client, 
            IRestRequest request)
        {
            switch (_type)
            {
                case AuthType.ProtectedResource:
                    if (_accessTokenName == BEARER)
                    {
                        request.AddBearerHeader(_accessToken);
                    }
                    else
                    {
                        request.AddParameter(_accessTokenName, _accessToken);
                    }

                    break;

                case AuthType.AccessToken:
                    request.AddCodeParamter(_code);
                    request.AddRedirectUriParameter(_redirectUrl);
                    request.AddClientIdParameter(_clientId);
                    request.AddClientSecretParameter(_clientSecret);
                    request.AddScopeParameter(_scope);
                    request.AddGrantTypeParameter(_grantType);
                    if (_hasBasicAuthorization)
                    {
                        request.AddBasicAuthorizationHeader(
                            _clientId,
                            _clientSecret);
                    }

                    break;

                case AuthType.RefreshToken:
                    request.AddClientIdParameter(_clientId);
                    request.AddClientSecretParameter(_clientSecret);
                    request.AddGrantTypeParameter(_grantType);
                    request.AddRefreshTokenParameter(_refreshToken);
                    if (_hasBasicAuthorization)
                    {
                        request.AddBasicAuthorizationHeader(
                            _clientId,
                            _clientSecret);
                    }
                    break;


            }
        }

        private enum AuthType
        {
            AccessToken,
            RefreshToken,
            ProtectedResource
        }
    }
}
