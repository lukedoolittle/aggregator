using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
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
            OAuth2ResponseType responseType,
            IDictionary<string, string> securityParameters,
            IDictionary<string, string> queryParameters)
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
            if (securityParameters == null)
            {
                throw new ArgumentNullException(nameof(securityParameters));
            }

            var builder = new UriBuilder(authorizeUrl.NonPath());
            builder.Path += authorizeUrl.AbsolutePath.TrimStart('/');

            var query = new HttpValueCollection
            {
                {OAuth2Parameter.RedirectUri.EnumToString(), redirectUri.ToString()},
                {OAuth2Parameter.ClientId.EnumToString(), clientId},
                {OAuth2Parameter.ResponseType.EnumToString(), responseType.EnumToString()}
            };

            if (!string.IsNullOrEmpty(scope))
            {
                query.Add(OAuth2Parameter.Scope.EnumToString(), scope);
            }

            foreach (var parameter in securityParameters)
            {
                query.Add(
                    parameter.Key,
                    parameter.Value);
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
            CryptoKey privateKey,
            string clientId)
        {
            if (accessUrl == null) throw new ArgumentNullException(nameof(accessUrl));
            if (jsonWebToken == null) throw new ArgumentNullException(nameof(jsonWebToken));
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));

            return (await new HttpRequestBuilder(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2JsonWebToken(
                    jsonWebToken.ToEncodedWebToken(false),
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
