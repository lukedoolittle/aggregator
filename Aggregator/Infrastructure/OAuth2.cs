using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BatmansBelt.Extensions;
using Aggregator.Framework;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Exceptions;
using Aggregator.Framework.Extensions;
using Aggregator.Infrastructure.Credentials;
using RestSharp;
using RestSharp.Authenticators;

namespace Aggregator.Infrastructure.Authentication
{
    public class OAuth2 : IOAuth2
    {
        public Uri GetAuthorizationPath(
            Uri authorizeUrl,
            string clientId,
            string scope,
            Uri redirectUri,
            string state,
            ResponseTypeEnum responseType,
            Dictionary<string, string> parameters)
        {
            if (authorizeUrl == null)
            {
                throw new ArgumentNullException(nameof(authorizeUrl));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            if (redirectUri == null)
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }

            var client = new RestClient(authorizeUrl.NonPath());

            var request = new RestRequest(authorizeUrl.AbsolutePath);

            request.AddRedirectUriParameter(redirectUri.ToString());
            request.AddClientIdParameter(clientId);
            request.AddScopeParameter(scope);
            request.AddStateParameter(state);
            request.AddResponseTypeParameter(responseType);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    request.AddParameter(
                        parameter.Key, 
                        parameter.Value);
                }
            }

            return client.BuildUri(request);
        }

        public Task<OAuth2Credentials> GetRefreshToken(
            Uri accessUrl,
            string clientId,
            string clientSecret,
            string refreshToken,
            bool hasBasicAuthorization)
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

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var authenticator = OAuth2Authenticator.ForRefreshToken(
                clientId,
                clientSecret,
                refreshToken,
                hasBasicAuthorization);

            return GetAccessToken(
                accessUrl,
                authenticator);
        }

        public Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl,
            string clientId,
            string clientSecret,
            Uri callbackUrl,
            string code,
            string scope,
            bool hasBasicAuthorization)
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

            if (callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            var authenticator = OAuth2Authenticator.ForAccessToken(
                clientId,
                clientSecret,
                callbackUrl.ToString(),
                code,
                scope,
                hasBasicAuthorization);

            return GetAccessToken(accessUrl, authenticator);
        }

        private async Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl,
            IAuthenticator authenticator)
        {
            var client = new RestClient(accessUrl.NonPath());

            var request = new RestRequest(
                accessUrl.AbsolutePath,
                Method.POST);

            client.Authenticator = authenticator;

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException();
            }

            var response = await client.ExecuteTaskAsync(request)
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new BadHttpRequestException(response.Content);
            }

            var token = response.ParseToken<OAuth2Credentials>();
            token.TimestampToken();

            return token;
        }
    }
}
