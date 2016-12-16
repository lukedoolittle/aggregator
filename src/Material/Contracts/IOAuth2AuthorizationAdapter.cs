using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.HttpClient.Authenticators;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuth2AuthorizationAdapter
    {
        Uri GetAuthorizationUri(
            Uri authorizeUri,
            IAuthenticator authenticator);

        Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl,
            IAuthenticator authenticator,
            IDictionary<HttpRequestHeader, string> headers);
    }
}
