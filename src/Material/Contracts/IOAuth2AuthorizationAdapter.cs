using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuth2AuthorizationAdapter
    {
        Uri GetAuthorizationUri(
            Uri authorizeUrl,
            string clientId,
            string scope,
            Uri redirectUri,
            string state,
            OAuth2ResponseType responseType,
            Dictionary<string, string> queryParameters);

        Task<OAuth2Credentials> GetRefreshToken(
            Uri accessUrl,
            string clientId,
            string clientSecret,
            string refreshToken,
            Dictionary<HttpRequestHeader, string> headers);

        Task<OAuth2Credentials> GetClientAccessToken(
            Uri accessUri,
            string clientId,
            string clientSecret);

        Task<OAuth2Credentials> GetJsonWebToken(
            Uri accessUrl,
            JsonWebToken jsonWebToken,
            CryptoKey privateKey,
            string clientId);

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
