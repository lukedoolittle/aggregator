using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Enums;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Contracts;

namespace Material.OAuth.Authentication
{
    public class OAuthProtectedResourceAdapter : IOAuthProtectedResourceAdapter
    {
        private readonly IAuthenticator _authenticator;
        private readonly HttpParameterType _parameterHandling;

        public OAuthProtectedResourceAdapter(
            IAuthenticator authenticator, 
            HttpParameterType parameterHandling)
        {
            _authenticator = authenticator;
            _parameterHandling = parameterHandling;
        }

        public Task<TResponse> ForProtectedResource<TResponse>(
            string host,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> additionalQuerystringParameters,
            IDictionary<string, string> urlPathParameters,
            object body,
            MediaType bodyType)
        {
            return ForProtectedResource<TResponse>(
                host, 
                path, 
                httpMethod,
                headers,
                additionalQuerystringParameters,
                urlPathParameters, 
                body, 
                bodyType, 
                HttpStatusCode.OK);
        }

        //TODO: parameters shouldn't be dictionaries because there can be duplicates
        public async Task<TResponse> ForProtectedResource<TResponse>(
            string host,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> additionalQuerystringParameters,
            IDictionary<string, string> urlPathParameters,
            object body,
            MediaType bodyType,
            HttpStatusCode expectedResponse)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            using (var requestBuilder = new HttpRequestBuilder(host))
            {
                return await requestBuilder
                    .Request(httpMethod, path, _parameterHandling)
                    .ResponseMediaType(MediaType.Json)
                    .Headers(headers)
                    .Parameters(additionalQuerystringParameters)
                    .Segments(urlPathParameters.ToHttpValueCollection())
                    .Authenticator(_authenticator)
                    .ThrowIfNotExpectedResponseCode(expectedResponse)
                    .Content(body, bodyType)
                    .ResultAsync<TResponse>()
                    .ConfigureAwait(false);
            }
        }
    }
}
