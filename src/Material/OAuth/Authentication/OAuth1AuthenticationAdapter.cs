using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth1AuthenticationAdapter : IOAuth1AuthenticationAdapter
    {
        public async Task<OAuth1Credentials> GetRequestToken(
            Uri requestUrl, 
            string consumerKey, 
            string consumerSecret, 
            HttpParameterType parameterHandling,
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

            using (var requestBuilder = new HttpRequestBuilder(requestUrl.NonPath()))
            {
                return (await requestBuilder
                            .PostTo(
                                requestUrl.AbsolutePath,
                                parameterHandling)
                            .ForOAuth1RequestToken(
                                consumerKey,
                                consumerSecret,
                                callbackUrl)
                            .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                            .ResultAsync<OAuth1Credentials>()
                            .ConfigureAwait(false))
                            .SetConsumerProperties(
                                consumerKey,
                                consumerSecret);
            }
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
                throw new ArgumentNullException(nameof(oauthToken));
            }

            var builder = new UriBuilder(authorizeUri.NonPath());
            builder.Path += authorizeUri.AbsolutePath.TrimStart('/');

            var query = new HttpValueCollection
            {
                { OAuth2Parameter.OAuthToken.EnumToString(), oauthToken}
            };

            builder.Query += query.ToString();

            return builder.Uri;
        }

        //TODO: parameters shouldn't be dictionaries because there can be duplicates
        public async Task<OAuth1Credentials> GetAccessToken(
            Uri accessUri, 
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            string verifier,
            HttpParameterType parameterHandling, 
            IDictionary<string, string> queryParameters)
        {
            if (accessUri == null)
            {
                throw new ArgumentNullException(nameof(accessUri));
            }

            using (var requestBuilder = new HttpRequestBuilder(accessUri.NonPath()))
            {
                return (await requestBuilder
                            .PostTo(
                                accessUri.AbsolutePath,
                                parameterHandling)
                            .ForOAuth1AccessToken(
                                consumerKey,
                                consumerSecret,
                                oauthToken,
                                oauthSecret,
                                verifier)
                            .Parameters(queryParameters)
                            .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                            .ResultAsync<OAuth1Credentials>()
                            .ConfigureAwait(false))
                            .SetConsumerProperties(
                                consumerKey,
                                consumerSecret);
            }
        }
    }
}
