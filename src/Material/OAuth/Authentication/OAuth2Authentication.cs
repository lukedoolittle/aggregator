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
using Material.Exceptions;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2Authentication : IOAuth2Authentication
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

            var response = await new HttpRequest(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2RefreshToken(
                    clientId,
                    clientSecret,
                    refreshToken)
                .Headers(headers)
                .ExecuteAsync()
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Reason));
            }

            var token = await response
                .ContentAsync<OAuth2Credentials>()
                .ConfigureAwait(false);

            token.TimestampToken();

            return token;
        }

        public async Task<OAuth2Credentials> GetClientAccessToken(
            Uri accessUrl,
            string clientId,
            string clientSecret)
        {
            var response = await new HttpRequest(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2ClientAccessToken(
                    clientId,
                    clientSecret)
                .ExecuteAsync()
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Reason));
            }

            var token = await response
                .ContentAsync<OAuth2Credentials>()
                .ConfigureAwait(false);

            token.TimestampToken();

            return token;
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

            var response = await new HttpRequest(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .ForOAuth2AccessToken(
                    clientId,
                    clientSecret,
                    callbackUrl.ToString(),
                    code,
                    scope)
                .Headers(headers)
                .ExecuteAsync()
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Reason));
            }

            var token = await response
                .ContentAsync<OAuth2Credentials>()
                .ConfigureAwait(false);

            token.TimestampToken();

            return token;
        }
    }
}
