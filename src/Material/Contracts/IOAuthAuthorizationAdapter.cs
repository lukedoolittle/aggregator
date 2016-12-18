using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Infrastructure.Credentials;

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
