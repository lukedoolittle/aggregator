using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations;
using Foundations.Extensions;
using Foundations.Http;
using Foundations.Serialization;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;
using RestSharp;
using RestSharp.Authenticators;

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

            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(nameof(state));
            }

            var client = new RestClient(authorizeUrl.NonPath());
            var request = new RestRequest(authorizeUrl.AbsolutePath);

            request.AddParameter(
                OAuth2ParameterEnum.RedirectUri.EnumToString(),
                redirectUri.ToString());
            request.AddParameter(
                OAuth2ParameterEnum.ClientId.EnumToString(), 
                clientId);
            request.AddParameter(
                OAuth2ParameterEnum.Scope.EnumToString(), 
                scope);
            request.AddParameter(
                OAuth2ParameterEnum.State.EnumToString(), 
                state);
            request.AddParameter(
                OAuth2ParameterEnum.ResponseType.EnumToString(), 
                responseType.EnumToString());

            foreach (var parameter in parameters)
            {
                request.AddParameter(
                    parameter.Key, 
                    parameter.Value);
            }

            return client.BuildUri(request);
        }

        public virtual Task<OAuth2Credentials> GetRefreshToken(
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
                refreshToken);

            return GetAccessToken(
                accessUrl,
                authenticator,
                headers);
        }

        public virtual Task<OAuth2Credentials> GetAccessToken(
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
                scope);

            return GetAccessToken(
                accessUrl, 
                authenticator,
                headers);
        }

        private async Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl,
            IAuthenticator authenticator,
            Dictionary<HttpRequestHeader, string> headers)
        {
            var client = new RestClient(accessUrl.NonPath())
            {
                Authenticator = authenticator
            };

            var request = new RestRequest(
                accessUrl.AbsolutePath,
                Method.POST);


            foreach (var header in headers)
            {
                request.AddHeader(
                    header.Key.ToString(), 
                    header.Value);
            }

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            var response = await client.ExecuteTaskAsync(request)
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new BadHttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Content));
            }

            var token = ParseToken(response);
            token.TimestampToken();

            return token;
        }

        //TODO: remove this duplicate code in OAuth1Authentication. possibly do this polymorphically based on service
        private static OAuth2Credentials ParseToken(IRestResponse instance)
        {
            if (instance.ContentType.Contains(MimeTypeEnum.Json.EnumToString()))
            {
                return instance.Content.AsEntity<OAuth2Credentials>(false);
            }
            else
            {
                return HttpUtility.ParseQueryString(instance.Content)
                    .AsEntity<OAuth2Credentials>();
            }
        }
    }
}
