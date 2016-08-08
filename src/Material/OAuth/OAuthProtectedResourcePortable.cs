using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using HttpRequestException = Material.Exceptions.HttpRequestException;

namespace Material.OAuth
{
    public class OAuthProtectedResourcePortable : IOAuthProtectedResource
    {
        private readonly IAuthenticator _authenticator;

        public OAuthProtectedResourcePortable(
            string accessToken,
            string accessTokenName)
        {
            if (string.IsNullOrEmpty(accessTokenName))
            {
                throw new ArgumentNullException(nameof(accessTokenName));
            }
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _authenticator = new OAuth2ProtectedResource(
                accessToken,
                accessTokenName);
        }

        public async Task<string> ForProtectedResource(
            string baseUrl,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> querystringParameters,
            IDictionary<string, string> pathParameters,
            HttpStatusCode expectedResponse = HttpStatusCode.OK)
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

            var response = await new HttpRequest()
                .Request(
                    new HttpMethod(httpMethod),
                    path)
                .Headers(headers)
                .Parameters(querystringParameters)
                .Segments(pathParameters)
                .Authenticator(_authenticator)
                .ExecuteAsync(new Uri(baseUrl))
                .ConfigureAwait(false);

            if (response.StatusCode != expectedResponse)
            {
                throw new HttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Reason));
            }

            return await response
                .ContentAsync()
                .ConfigureAwait(false);
        }
    }
}
