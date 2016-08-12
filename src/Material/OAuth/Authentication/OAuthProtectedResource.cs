using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Foundations.Http;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using HttpRequestException = Material.Exceptions.HttpRequestException;

namespace Material.OAuth
{
    public class OAuthProtectedResource : IOAuthProtectedResource
    {
        private readonly IAuthenticator _authenticator;
        private readonly OAuthParameterTypeEnum _parameterHandling;

        public OAuthProtectedResource(
            string accessToken,
            string accessTokenName)
        {
            _authenticator = new OAuth2ProtectedResource(
                accessToken,
                accessTokenName);

            _parameterHandling = OAuthParameterTypeEnum.Querystring;
        }

        public OAuthProtectedResource(
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            OAuthParameterTypeEnum parameterHandling)
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
            HttpStatusCode expectedResponse = HttpStatusCode.OK,
            MediaTypeEnum expectedResponseType = MediaTypeEnum.Json)
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

            var request = new HttpRequest(baseUrl)
                .Request(
                    new HttpMethod(httpMethod),
                    path)
                .ResponseMediaType(expectedResponseType)
                .Headers(headers)
                .Parameters(querystringParameters)
                .Segments(pathParameters)
                .Authenticator(_authenticator);

            if (_parameterHandling == OAuthParameterTypeEnum.Body)
            {
                request.WithQueryParameters();
            }

            var response = await request
                .ExecuteAsync()
                .ConfigureAwait(false);

            if (response.StatusCode != expectedResponse)
            {
                throw new HttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Reason));
            }

            return await response
                .ContentAsync<TResponse>()
                .ConfigureAwait(false);
        }
    }
}
