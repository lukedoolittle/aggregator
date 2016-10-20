using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations;
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
        private readonly HttpParameterType _parameterHandling = 
            HttpParameterType.Querystring;

        public OAuthProtectedResourceAdapter(
            string accessToken,
            string accessTokenName)
        {
            _authenticator = new OAuth2ProtectedResource(
                accessToken,
                accessTokenName);
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

            return await (await new HttpRequestBuilder(baseUrl)
                .Request(httpMethod, path, _parameterHandling)
                .ResponseMediaType(expectedResponseType)
                .Headers(headers)
                .Parameters(querystringParameters)
                .Segments(pathParameters)
                .Authenticator(_authenticator)
                .ThrowIfNotExpectedResponseCode(expectedResponse)
                .Content(body, bodyType)
                .ExecuteAsync()
                .ConfigureAwait(false))
                .ContentAsync<TResponse>()
                .ConfigureAwait(false);
        }
    }
}
