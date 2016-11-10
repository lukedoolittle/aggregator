using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authorization
{
    public class OAuth2AuthorizationAdapter : IOAuth2AuthorizationAdapter
    {
        public Uri GetAuthorizationUri(
            Uri authorizeUrl, 
            string clientId, 
            string scope,
            Uri redirectUri, 
            string state,
            OAuth2ResponseType responseType, 
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
                {OAuth2Parameter.RedirectUri.EnumToString(), redirectUri.ToString()},
                {OAuth2Parameter.ClientId.EnumToString(), clientId},
                {OAuth2Parameter.State.EnumToString(), state},
                {OAuth2Parameter.ResponseType.EnumToString(), responseType.EnumToString()}
            };

            if (!string.IsNullOrEmpty(scope))
            {
                query.Add(OAuth2Parameter.Scope.EnumToString(), scope);
            }

            if (queryParameters != null)
            {
                foreach (var parameter in queryParameters)
                {
                    query.Add(
                        parameter.Key,
                        parameter.Value);
                }
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

            return (await new HttpRequestBuilder(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2RefreshToken(
                    clientId,
                    clientSecret,
                    refreshToken)
                .Headers(headers)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ResultAsync<OAuth2Credentials>()
                .ConfigureAwait(false));
        }

        public async Task<OAuth2Credentials> GetClientAccessToken(
            Uri accessUri,
            string clientId,
            string clientSecret)
        {
            if (accessUri == null)
            {
                throw new ArgumentNullException(nameof(accessUri));
            }
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            return (await new HttpRequestBuilder(accessUri.NonPath())
                .PostTo(accessUri.AbsolutePath)
                .ForOAuth2ClientAccessToken(
                    clientId,
                    clientSecret)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ResultAsync<OAuth2Credentials>()
                .ConfigureAwait(false));
        }

        public async Task<OAuth2Credentials> GetJsonWebToken(
            Uri accessUrl,
            JsonWebToken jsonWebToken,
            string privateKey,
            string clientId)
        {
            if (accessUrl == null)
            {
                throw new ArgumentNullException(nameof(accessUrl));
            }
            if (jsonWebToken == null)
            {
                throw new ArgumentNullException(nameof(jsonWebToken));
            }
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

                var serializer = new JsonSerializer();

            return (await new HttpRequestBuilder(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2JsonWebToken(
                    serializer.Serialize(jsonWebToken.Header),
                    serializer.Serialize(jsonWebToken.Claims),
                    jsonWebToken.Header.Algorithm,
                    privateKey,
                    clientId)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ResultAsync<OAuth2Credentials>()
                .ConfigureAwait(false));
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

            return (await new HttpRequestBuilder(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2AccessToken(
                    clientId,
                    clientSecret,
                    callbackUrl,
                    code,
                    scope)
                .Headers(headers)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ResultAsync<OAuth2Credentials>()
                .ConfigureAwait(false));
        }
    }
}
