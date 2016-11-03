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

        public async Task<TResponse> ForProtectedResource<TResponse>(
            string host,
            string path,
            string httpMethod,
            IEnumerable<MediaType> responseTypes,
            IDictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> additionalQuerystringParameters,
            IDictionary<string, string> urlPathParameters,
            object body,
            MediaType bodyType,
            IEnumerable<HttpStatusCode> expectedResponse,
            MediaType? overriddenMediaType)
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
                    .ResponseMediaTypes(responseTypes)
                    .Headers(headers)
                    .Parameters(additionalQuerystringParameters)
                    .Segments(urlPathParameters.ToHttpValueCollection())
                    .Authenticator(_authenticator)
                    .ThrowIfNotExpectedResponseCode(expectedResponse)
                    .Content(body, bodyType)
                    .OverrideResponseMediaType(overriddenMediaType)
                    .ResultAsync<TResponse>()
                    .ConfigureAwait(false);
            }
        }
    }
}
