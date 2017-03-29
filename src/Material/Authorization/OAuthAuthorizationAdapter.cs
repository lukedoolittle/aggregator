using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Extensions;

namespace Material.Authorization
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
                .GetFrom(authorizeUri.AbsolutePath)
                .Authenticator(authenticator)
                .GenerateRequestUri();
        }
    }
}
