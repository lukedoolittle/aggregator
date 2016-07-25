using System.Net;
using Foundations.Extensions;
using Material.Enums;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2Authenticator : RestSharp.Authenticators.IAuthenticator
    {
        private string _clientId;
        private string _clientSecret;
        private string _redirectUrl;
        private string _code;
        private string _accessToken;
        private string _accessTokenName;
        private string _refreshToken;
        private string _scope;
        private GrantTypeEnum _grantType;
        private AuthType _type;

        public static RestSharp.Authenticators.IAuthenticator ForAccessToken(
            string clientId,
            string clientSecret,
            string redirectUrl,
            string code,
            string scope)
        {
            return new OAuth2Authenticator
            {
                _clientId = clientId,
                _clientSecret = clientSecret,
                _redirectUrl = redirectUrl,
                _code = code,
                _scope = scope,
                _grantType = GrantTypeEnum.AuthCode,
                _type = AuthType.AccessToken
            };
        }

        public static RestSharp.Authenticators.IAuthenticator ForRefreshToken(
            string clientId,
            string clientSecret,
            string refreshToken)
        {
            return new OAuth2Authenticator
            {
                _clientId = clientId,
                _clientSecret = clientSecret,
                _refreshToken = refreshToken,
                _grantType = GrantTypeEnum.RefreshToken,
                _type = AuthType.RefreshToken
            };
        }

        public static RestSharp.Authenticators.IAuthenticator ForProtectedResource(
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
            RestSharp.IRestClient client,
            RestSharp.IRestRequest request)
        {
            switch (_type)
            {
                case AuthType.ProtectedResource:
                    if (_accessTokenName == OAuth2ParameterEnum.BearerHeader.EnumToString())
                    {
                        request.AddHeader(
                             HttpRequestHeader.Authorization.ToString(),
                             $"{OAuth2ParameterEnum.BearerHeader.EnumToString()} {_accessToken}");
                    }
                    else
                    {
                        request.AddParameter(_accessTokenName, _accessToken);
                    }

                    break;

                case AuthType.AccessToken:
                    request.AddParameter(
                        ResponseTypeEnum.Code.EnumToString(), 
                        _code);
                    request.AddParameter(
                        OAuth2ParameterEnum.RedirectUri.EnumToString(), 
                        _redirectUrl);
                    request.AddParameter(
                        OAuth2ParameterEnum.ClientId.EnumToString(), 
                        _clientId);
                    request.AddParameter(
                        OAuth2ParameterEnum.ClientSecret.EnumToString(), 
                        _clientSecret);
                    request.AddParameter(
                        OAuth2ParameterEnum.Scope.EnumToString(),
                        _scope);
                    request.AddParameter(
                        OAuth2ParameterEnum.GrantType.EnumToString(),
                        _grantType.EnumToString());
                    break;

                case AuthType.RefreshToken:
                    request.AddParameter(
                        OAuth2ParameterEnum.ClientId.EnumToString(), 
                        _clientId);
                    request.AddParameter(
                        OAuth2ParameterEnum.ClientSecret.EnumToString(), 
                        _clientSecret);
                    request.AddParameter(
                        OAuth2ParameterEnum.GrantType.EnumToString(), 
                        _grantType.EnumToString());
                    request.AddParameter(
                        GrantTypeEnum.RefreshToken.EnumToString(), 
                        _refreshToken);
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
