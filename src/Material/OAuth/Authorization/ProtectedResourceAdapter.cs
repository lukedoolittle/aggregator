using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Infrastructure;

namespace Material.OAuth.Authorization
{
    public class ProtectedResourceAdapter : IProtectedResourceAdapter
    {
        private readonly IAuthenticator _authenticator;
        private readonly HttpParameterType _parameterHandling;

        public ProtectedResourceAdapter(
            IAuthenticator authenticator, 
            HttpParameterType parameterHandling)
        {
            _authenticator = authenticator;
            _parameterHandling = parameterHandling;
        }

        public Task<TResponse> ForProtectedResource<TRequest, TResponse>(
            TRequest request)
            where TRequest : OAuthRequest
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return new HttpRequestBuilder(request.GetModifiedHost())
                .Request(
                    request.HttpMethod,
                    request.Path,
                    _parameterHandling)
                .ResponseMediaTypes(
                    request.Consumes)
                .Headers(
                    request.Headers)
                .Parameters(
                    request.QuerystringParameters)
                .Segments(
                    request.PathParameters.ToHttpValueCollection())
                .Authenticator(_authenticator)
                .ThrowIfNotExpectedResponseCode(
                    request.ExpectedStatusCodes)
                .Content(
                    request.Body,
                    request.BodyType)
                .OverrideResponseMediaType(
                    request.OverriddenResponseMediaType)
                .ResultAsync<TResponse>();
        }
    }
}
