using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuth2Authentication
    {
        Uri GetAuthorizationUri(
            Uri authorizeUrl,
            string clientId,
            string scope,
            Uri redirectUri,
            string state,
            ResponseTypeEnum responseType,
            Dictionary<string, string> queryParameters);

        Task<OAuth2Credentials> GetRefreshToken(
            Uri accessUrl,
            string clientId,
            string clientSecret,
            string refreshToken,
            Dictionary<HttpRequestHeader, string> headers);

        Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl,
            string clientId,
            string clientSecret,
            Uri callbackUrl,
            string code,
            string scope,
            Dictionary<HttpRequestHeader, string> headers);
    }
}
