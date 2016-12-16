using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authorization
{
    public class OAuth2AuthorizationAdapter : IOAuth2AuthorizationAdapter
    {
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

        public async Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl, 
            IAuthenticator authenticator,
            IDictionary<HttpRequestHeader, string> headers)
        {
            if (accessUrl == null) throw new ArgumentNullException(nameof(accessUrl));
            if (authenticator == null) throw new ArgumentNullException(nameof(authenticator));

            return await new HttpRequestBuilder(accessUrl.NonPath())
                .PostTo(accessUrl.AbsolutePath)
                .Authenticator(authenticator)
                .Headers(headers)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ResultAsync<OAuth2Credentials>()
                .ConfigureAwait(false);
        }
    }
}
