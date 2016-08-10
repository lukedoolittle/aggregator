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

namespace Material.OAuth
{
    public class OAuth1AuthenticationPortable : IOAuth1Authentication
    {
        public async Task<OAuth1Credentials> GetRequestToken(
            Uri requestUrl, 
            string consumerKey, 
            string consumerSecret, 
            OAuthParameterTypeEnum parameterHandling,
            Uri callbackUrl)
        {
            if (requestUrl == null)
            {
                throw new ArgumentNullException(nameof(requestUrl));
            }
            if (callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            var request = new HttpRequest(requestUrl.NonPath())
                .PostTo(requestUrl.AbsolutePath)
                .ForOAuth1RequestToken(
                    consumerKey,
                    consumerSecret,
                    callbackUrl.ToString());

            if (parameterHandling == OAuthParameterTypeEnum.Body)
            {
                request.WithQueryParameters();
            }

            var response = await request
                .ExecuteAsync()
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Reason));
            }

            var credentials = await response
                .ContentAsync<OAuth1Credentials>()
                .ConfigureAwait(false);

            return credentials.SetConsumerProperties(
                consumerKey, 
                consumerSecret);
        }

        public Uri GetAuthorizationUri(
            string oauthToken,
            Uri authorizeUri)
        {
            if (authorizeUri == null)
            {
                throw new ArgumentNullException(nameof(authorizeUri));
            }

            if (string.IsNullOrEmpty(oauthToken))
            {
                throw new ArgumentNullException(nameof(authorizeUri));
            }

            var builder = new UriBuilder(authorizeUri.NonPath());
            builder.Path += authorizeUri.AbsolutePath.TrimStart('/');

            var query = new HttpValueCollection
            {
                { OAuth2ParameterEnum.OAuthToken.EnumToString(), oauthToken}
            };

            builder.Query += query.ToString();

            return builder.Uri;
        }

        public async Task<OAuth1Credentials> GetAccessToken(
            Uri accessUri, 
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            string verifier,
            OAuthParameterTypeEnum parameterHandling, 
            IDictionary<string, string> queryParameters)
        {
            if (accessUri == null)
            {
                throw new ArgumentNullException(nameof(accessUri));
            }

            var request = new HttpRequest(accessUri.NonPath())
                .PostTo(accessUri.AbsolutePath)
                .ForOAuth1AccessToken(
                    consumerKey,
                    consumerSecret,
                    oauthToken,
                    oauthSecret,
                    verifier);

            foreach (var querystringParameter in queryParameters)
            {
                request.Parameter(
                    querystringParameter.Key,
                    querystringParameter.Value);
            }

            if (parameterHandling == OAuthParameterTypeEnum.Body)
            {
                request.WithQueryParameters();
            }

            var response = await request
                .ExecuteAsync()
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Reason));
            }

            var credentials = await response
                .ContentAsync<OAuth1Credentials>()
                .ConfigureAwait(false);

            return credentials.SetConsumerProperties(
                consumerKey,
                consumerSecret);
        }
    }
}
