using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Extensions;
using OAuthRequest = Material.Domain.Core.OAuthRequest;

namespace Material.Authorization
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

            return CreateBuilder(request).ResultAsync<TResponse>();
        }

        public Task<HttpResponse> ForProtectedResource<TRequest>(TRequest request) 
            where TRequest : OAuthRequest
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return CreateBuilder(request).ExecuteAsync();
        }

        private HttpRequestBuilder CreateBuilder<TRequest>(TRequest request)
            where TRequest : OAuthRequest
        {
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
                .Content(request.Content)
                .OverrideResponseMediaType(
                    request.OverriddenResponseMediaType);
        }
    }
}
