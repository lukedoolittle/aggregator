using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2AuthenticationAdapter : IOAuth2AuthenticationAdapter
    {
        public Uri GetAuthorizationUri(
            Uri authorizeUrl, 
            string clientId, 
            string scope,
            Uri redirectUri, 
            string state,
            ResponseTypeEnum responseType, 
            Dictionary<string, string> queryParameters)
        {
            if (authorizeUrl == null)
            {
                throw new ArgumentNullException(nameof(authorizeUrl));
            }
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (redirectUri == null)
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(nameof(state));
            }

            var builder = new UriBuilder(authorizeUrl.NonPath());
            builder.Path += authorizeUrl.AbsolutePath.TrimStart('/');

            var query = new HttpValueCollection
            {
                {OAuth2ParameterEnum.RedirectUri.EnumToString(), redirectUri.ToString()},
                {OAuth2ParameterEnum.ClientId.EnumToString(), clientId},
                {OAuth2ParameterEnum.State.EnumToString(), state},
                {OAuth2ParameterEnum.ResponseType.EnumToString(), responseType.EnumToString()}
            };

            if (!string.IsNullOrEmpty(scope))
            {
                query.Add(OAuth2ParameterEnum.Scope.EnumToString(), scope);
            }

            foreach (var parameter in queryParameters)
            {
                query.Add(
                    parameter.Key, 
                    parameter.Value);
            }

            builder.Query += query.ToString();

            return builder.Uri;
        }

        public async Task<OAuth2Credentials> GetRefreshToken(
            Uri accessUrl, 
            string clientId, 
            string clientSecret, 
            string refreshToken, 
            Dictionary<HttpRequestHeader, string> headers)
        {
            if (accessUrl == null)
            {
                throw new ArgumentNullException(nameof(accessUrl));
            }
            if (clientSecret == null)
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }
            if (refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            return (await (await new HttpRequest(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2RefreshToken(
                    clientId,
                    clientSecret,
                    refreshToken)
                .Headers(headers)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ExecuteAsync()
                .ConfigureAwait(false))
                .ContentAsync<OAuth2Credentials>()
                .ConfigureAwait(false))
                .TimestampToken();
        }

        public async Task<OAuth2Credentials> GetClientAccessToken(
            Uri accessUrl,
            string clientId,
            string clientSecret)
        {
            if (accessUrl == null)
            {
                throw new ArgumentNullException(nameof(accessUrl));
            }
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            return (await (await new HttpRequest(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2ClientAccessToken(
                    clientId,
                    clientSecret)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ExecuteAsync()
                .ConfigureAwait(false))
                .ContentAsync<OAuth2Credentials>()
                .ConfigureAwait(false)).TimestampToken();
        }

        public async Task<OAuth2Credentials> GetJsonWebToken(
            Uri accessUrl,
            JsonWebToken jwt,
            string privateKey,
            string clientId)
        {
            if (accessUrl == null)
            {
                throw new ArgumentNullException(nameof(accessUrl));
            }
            if (jwt == null)
            {
                throw new ArgumentNullException(nameof(jwt));
            }
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            return (await (await new HttpRequest(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2JsonWebToken(
                    jwt,
                    privateKey,
                    clientId)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ExecuteAsync()
                .ConfigureAwait(false)).ContentAsync<OAuth2Credentials>()
                .ConfigureAwait(false))
                .TimestampToken();
        }

        public async Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl, 
            string clientId, 
            string clientSecret,
            Uri callbackUrl,
            string code, 
            string scope,
            Dictionary<HttpRequestHeader, string> headers)
        {
            if (accessUrl == null)
            {
                throw new ArgumentNullException(nameof(accessUrl));
            }
            if (callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            return (await (await new HttpRequest(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2AccessToken(
                    clientId,
                    clientSecret,
                    callbackUrl.ToString(),
                    code,
                    scope)
                .Headers(headers)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ExecuteAsync()
                .ConfigureAwait(false)).ContentAsync<OAuth2Credentials>()
                .ConfigureAwait(false))
                .TimestampToken();
        }
    }
}
