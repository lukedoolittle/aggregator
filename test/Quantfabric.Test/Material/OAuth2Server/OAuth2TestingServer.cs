using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Foundations;
using Foundations.Extensions;
using Foundations.Http;
using Foundations.HttpClient.Enums;
using Foundations.Cryptography;
using Foundations.Cryptography.StringCreation;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.OAuthServer;

namespace Quantfabric.Test.Integration
{

    //TODO:
    //implement JWT checking

    public class OAuth2TestingServer : IDisposable
    {
        private readonly HttpServer _server = new HttpServer();
        private readonly IDictionary<string, Action<IncommingMessage, ServerResponse>> _assertions = 
            new Dictionary<string, Action<IncommingMessage, ServerResponse>>();
        private readonly List<string> _validCodes = new List<string>();
        private readonly List<string> _validAccessTokens = new List<string>();
        private readonly List<string> _validRefreshTokens = new List<string>();

        private string _tokenPath;
        private string _authorizationPath;
        private string _clientId;
        private string _clientSecret;
        private string _redirectUri;
        private int _msUntilExpiry;
        private bool _requiresScope;

        #region Server

        public Task<Exception> Start(int port)
        {
#pragma warning disable 4014
            _server.CreateServer((message, response) =>
            {
                var path = message.Uri.AbsolutePath;

                if (_assertions.ContainsKey(path))
                {
                    _assertions[path](message, response);
                }

                if (path == _tokenPath)
                { 
                    var request = new HtmlSerializer()
                        .Deserialize<OAuth2TokenRequest>(
                            message.BodyAsString);

                    var credentials = HandleTokenRequest(request);

                    var responseBody = new JsonSerializer().Serialize(credentials);

                    response.WriteHead(HttpStatusCode.OK);
                    response.WriteHead(HttpRequestHeader.ContentType, MediaTypeEnum.Json);
                    response.Write(responseBody);
                    response.End();
                }
                else if (path == _authorizationPath)
                {
                    var serializer = new HtmlSerializer();

                    var request = serializer.Deserialize<OAuth2AuthorizationRequest>(
                        message.Uri.Query);

                    var credentials = HandleAuthorizationRequest(request);

                    var redirectUri = BuildRedirectUri(request.State, credentials);

                    response.Redirect(redirectUri);
                }
                else
                {
                    throw new Exception($"Request for {path} was not handled");
                }

            }).Listen(new Uri($"http://localhost:{port}"));
#pragma warning restore 4014


            var taskCompletion = new TaskCompletionSource<Exception>();

            _server.ServerException += (sender, args) =>
            {
                taskCompletion.SetResult(args.Exception);
            };

            return taskCompletion.Task;
        }

        public void Stop()
        {
            _server.Close();
        }

        #endregion Server

        public void Dispose()
        {
            (_server as IDisposable).Dispose();
        }

        #region Configuration

        public OAuth2TestingServer SetTokenPath(string tokenPath)
        {
            _tokenPath = tokenPath;

            return this;
        }

        public OAuth2TestingServer SetAuthorizationPath(string authorizationPath)
        {
            _authorizationPath = authorizationPath;

            return this;
        }

        public OAuth2TestingServer SetClientId(string clientId)
        {
            _clientId = clientId;

            return this;
        }

        public OAuth2TestingServer SetClientSecret(string clientSecret)
        {
            _clientSecret = clientSecret;

            return this;
        }

        public OAuth2TestingServer SetRedirectUri(string redirectUri)
        {
            _redirectUri = redirectUri;

            return this;
        }

        public OAuth2TestingServer SetCredentialsExpiration(int msUntilExpiry)
        {
            _msUntilExpiry = msUntilExpiry;

            return this;
        }

        public OAuth2TestingServer SetRequiresScope()
        {
            _requiresScope = true;

            return this;
        }

        #endregion Configuration

        #region Handlers

        private Uri BuildRedirectUri(string state, OAuth2Credentials credentials)
        {
            var querystring = new HttpValueCollection();
            querystring.Add("state", state);
            if (!string.IsNullOrEmpty(credentials.Code))
            {
                querystring.Add("code", credentials.Code);
            }
            if (!string.IsNullOrEmpty(credentials.AccessToken))
            {
                querystring.Add("access_token", credentials.AccessToken);
            }
            if (!string.IsNullOrEmpty(credentials.ExpiresIn))
            {
                querystring.Add("expires_in", credentials.ExpiresIn);
            }

            var builder = new UriBuilder(_redirectUri);

            builder.Query += querystring.ToString();

            return builder.Uri;
        }

        private OAuth2Credentials HandleTokenRequest(OAuth2TokenRequest request)
        {
            var credentials = new OAuth2Credentials();

            CheckClientId(request.ClientId);

            switch (request.GrantType)
            {
                case GrantTypeEnum.AuthCode:
                    CheckClientSecret(request.ClientSecret);
                    CheckRedirectUrl(request.RedirectUri);
                    CheckAndRemoveCode(request.Code);
                    break;
                case GrantTypeEnum.ClientCredentials:
                    CheckClientSecret(request.ClientSecret);
                    break;
                case GrantTypeEnum.JsonWebToken:
                    CheckJsonWebToken(request.JsonWebToken);
                    break;
                case GrantTypeEnum.RefreshToken:
                    CheckRefreshToken(request.RefreshToken);
                    break;
                default:
                    throw new Exception($"Unexpected grant type {request.GrantType.ToString()} in request");
            }

            AddToken(credentials);
            AddExpiresIn(credentials);

            return credentials;
        }

        private OAuth2Credentials HandleAuthorizationRequest(
            OAuth2AuthorizationRequest request)
        {
            var credentials = new OAuth2Credentials();

            CheckClientId(request.ClientId);
            CheckRedirectUrl(request.RedirectUri);
            CheckScope(request.Scope);

            switch (request.ResponseType)
            {
                case ResponseTypeEnum.Code:
                    AddCode(credentials);
                    break;
                case ResponseTypeEnum.Token:
                    AddToken(credentials);
                    AddExpiresIn(credentials);
                    break;
                default:
                    throw new Exception($"Unexpected response type {request.ResponseType} in request");
            }

            return credentials;
        }

        #endregion Handlers

        #region Request Checking

        private void CheckAndRemoveCode(string code)
        {
            if (_validCodes.Contains(code))
            {
                _validCodes.Remove(code);
            }
            else
            {
                throw new Exception($"Given code {code} was not valid");
            }
        }

        private void CheckClientId(string clientId)
        {
            if (string.IsNullOrEmpty(_clientId))
            {
                throw new Exception("Server clientId not set");
            }
            if (string.IsNullOrEmpty(clientId))
            {
                throw new Exception("Missing clientId from request");
            }

            if (clientId != _clientId)
            {
                throw new Exception("Request clientId does not match server clientId");
            }
        }

        private void CheckClientSecret(string clientSecret)
        {
            if (string.IsNullOrEmpty(_clientSecret))
            {
                throw new Exception("Server clientSecret not set");
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new Exception("Missing clientSecret from request");
            }

            if (clientSecret != _clientSecret)
            {
                throw new Exception("Request clientSecret does not match server clientSecret");
            }
        }

        private void CheckRedirectUrl(string redirectUrl)
        {
            if (string.IsNullOrEmpty(_redirectUri))
            {
                throw new Exception("Server redirectUri not set");
            }
            if (string.IsNullOrEmpty(redirectUrl))
            {
                throw new Exception("Missing redirectUri from request");
            }

            if (redirectUrl != _redirectUri)
            {
                throw new Exception("Request redirectUri does not match server redirectUri");
            }
        }

        private void CheckRefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new Exception("Missing refresh token from request");
            }

            if (!_validRefreshTokens.Contains(refreshToken))
            {
                throw new Exception($"Given refresh token {refreshToken} was not valid");
            }
        }

        private void CheckScope(string scope)
        {
            if (_requiresScope && string.IsNullOrEmpty(scope))
            {
                throw new Exception("Missing scope from request");
            }
        }

        private void CheckJsonWebToken(string jsonWebToken)
        {
            //need to ingest the signing algorithm(???) and the public key
            throw new NotImplementedException();
        }

        #endregion Request Checking

        #region Response Building

        private void AddCode(OAuth2Credentials response)
        {
            var security = new CryptoStringGenerator();
            var code = security.CreateRandomString
                (32, 
                CryptoStringTypeEnum.Base64AlphaNumeric);

            _validCodes.Add(code);

            response.SetPropertyValue("Code", code);
        }

        private void AddToken(OAuth2Credentials response)
        {
            var security = new CryptoStringGenerator();
            var token = security.CreateRandomString(
                32, 
                CryptoStringTypeEnum.Base64AlphaNumeric);

            _validAccessTokens.Add(token);

            response.SetMemberValue("_accessToken", token);
        }

        private void AddExpiresIn(OAuth2Credentials response)
        {
            if (_msUntilExpiry > 0)
            {
                response.SetMemberValue("_expiresIn", _msUntilExpiry.ToString());
            }
        }

        #endregion Response Building
    }
}
