using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Enums;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Foundations.HttpClient.Extensions;

namespace Material.Infrastructure.OAuth
{
    public class OAuthProtectedResourceAdapter : IOAuthProtectedResourceAdapter
    {
        private readonly IAuthenticator _authenticator;
        private readonly HttpParameterType _parameterHandling;

        public OAuthProtectedResourceAdapter(
            string accessToken,
            string accessTokenName,
            HttpParameterType parameterHandling)
        {
            _authenticator = new OAuth2ProtectedResource(
                accessToken,
                accessTokenName);

            _parameterHandling = parameterHandling;
        }

        public OAuthProtectedResourceAdapter(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            HttpParameterType parameterHandling)
        {
            _authenticator = new OAuth1ProtectedResource(
                consumerKey, 
                consumerSecret, 
                oauthToken,
                oauthSecret);

            _parameterHandling = parameterHandling;
        }

        //TODO: parameters shouldn't be dictionaries because there can be duplicates
        public async Task<TResponse> ForProtectedResource<TResponse>(
            string baseUrl,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> querystringParameters,
            IDictionary<string, string> pathParameters,
            object body,
            MediaType bodyType,
            HttpStatusCode expectedResponse = HttpStatusCode.OK,
            MediaType expectedResponseType = MediaType.Json)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            using (var requestBuilder = new HttpRequestBuilder(baseUrl))
            {
                return await requestBuilder
                    .Request(httpMethod, path, _parameterHandling)
                    .ResponseMediaType(expectedResponseType)
                    .Headers(headers)
                    .Parameters(querystringParameters)
                    .Segments(pathParameters.ToHttpValueCollection())
                    .Authenticator(_authenticator)
                    .ThrowIfNotExpectedResponseCode(expectedResponse)
                    .Content(body, bodyType)
                    .ResultAsync<TResponse>()
                    .ConfigureAwait(false);
            }
        }
    }
}
