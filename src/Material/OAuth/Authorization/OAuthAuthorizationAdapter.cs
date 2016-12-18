using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authorization
{
    public class OAuthAuthorizationAdapter : IOAuthAuthorizationAdapter
    {
        public Task<TCredentials> GetToken<TCredentials>(
            Uri url,
            IAuthenticator authenticator,
            IDictionary<HttpRequestHeader, string> headers,
            HttpParameterType parameterHandling)
            where TCredentials : TokenCredentials
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (authenticator == null) throw new ArgumentNullException(nameof(authenticator));

            return new HttpRequestBuilder(url.NonPath())
                .PostTo(
                    url.AbsolutePath,
                    parameterHandling)
                .Authenticator(authenticator)
                .Headers(headers)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ResultAsync<TCredentials>();
        }

        public Uri GetAuthorizationUri(
            Uri authorizeUri,
            IAuthenticator authenticator)
        {
            if (authorizeUri == null) throw new ArgumentNullException(nameof(authorizeUri));
            if (authenticator == null) throw new ArgumentNullException(nameof(authenticator));

            return new HttpRequestBuilder(authorizeUri.NonPath())
                .PostTo(authorizeUri.AbsolutePath)
                .Authenticator(authenticator)
                .GenerateRequestUri();
        }
    }
}
