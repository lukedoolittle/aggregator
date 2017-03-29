using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Material.Framework.Enums;
using Material.HttpClient.Authenticators;

namespace Material.Contracts
{
    public interface IOAuthAuthorizationAdapter
    {
        Task<TCredentials> GetToken<TCredentials>(
            Uri url,
            IAuthenticator authenticator,
            IDictionary<HttpRequestHeader, string> headers,
            HttpParameterType parameterHandling)
            where TCredentials : TokenCredentials;

        Uri GetAuthorizationUri(
            Uri authorizeUri,
            IAuthenticator authenticator);
    }
}
