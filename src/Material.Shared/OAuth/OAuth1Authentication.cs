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
using RestSharp.Authenticators.OAuth;

namespace Material.Infrastructure.OAuth
{
    public class OAuth1Authentication : IOAuth1Authentication
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

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException(nameof(consumerSecret));
            }

            var authenticator = OAuth1Authenticator.ForRequestToken(
                consumerKey,
                consumerSecret,
                callbackUrl.ToString());

            if (parameterHandling == OAuthParameterTypeEnum.Querystring)
            {
                authenticator.ParameterHandling = 
                    OAuthParameterHandling.UrlOrPostParameters;
            }

            var client = new RestClient(requestUrl.NonPath())
            {
                Authenticator = authenticator
            };

            var request = new RestRequest(requestUrl.AbsolutePath, Method.POST);

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

            return ParseToken(response)
                  .SetConsumerProperties(consumerKey, consumerSecret);
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

            var client = new RestClient(authorizeUri.NonPath());

            var request = new RestRequest(authorizeUri.AbsolutePath);
            request.AddParameter(
                OAuth2ParameterEnum.OAuthToken.EnumToString(), 
                oauthToken);

            return client.BuildUri(request);
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

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException(nameof(consumerSecret));
            }

            if (string.IsNullOrEmpty(oauthToken))
            {
                throw new ArgumentNullException(nameof(oauthToken));
            }

            if (string.IsNullOrEmpty(oauthSecret))
            {
                throw new ArgumentNullException(nameof(oauthSecret));
            }

            if (string.IsNullOrEmpty(verifier))
            {
                throw new ArgumentNullException(nameof(verifier));
            }

            var authenticator = OAuth1Authenticator.ForAccessToken(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                verifier);

            if (parameterHandling == OAuthParameterTypeEnum.Querystring)
            {
                authenticator.ParameterHandling =
                    OAuthParameterHandling.UrlOrPostParameters;
            }

            var client = new RestClient(accessUri.NonPath())
            {
                Authenticator = authenticator
            };

            var request = new RestRequest(
                accessUri.AbsolutePath, 
                Method.POST);

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            foreach (var querystringParameter in queryParameters)
            {
                request.AddParameter(
                    querystringParameter.Key, 
                    querystringParameter.Value);
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

            return ParseToken(response);
        }

        private static OAuth1Credentials ParseToken(IRestResponse instance)
        {
            if (instance.ContentType.Contains(MimeTypeEnum.Json.EnumToString()))
            {
                return instance.Content.AsEntity<OAuth1Credentials>(false);
            }
            else
            {
                var querystring = HttpUtility.ParseQueryString(instance.Content);

                return querystring.AsEntity<OAuth1Credentials>();
            }
        }
    }
}
