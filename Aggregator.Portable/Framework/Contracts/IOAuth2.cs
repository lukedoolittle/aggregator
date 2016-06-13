using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Credentials;

namespace Aggregator.Framework.Contracts
{
    public interface IOAuth2
    {
        Uri GetAuthorizationPath(
            Uri authorizeUrl,
            string clientId,
            string scope,
            Uri redirectUri,
            string state,
            ResponseTypeEnum responseType,
            Dictionary<string, string> parameters);

        Task<OAuth2Credentials> GetRefreshToken(
            Uri accessUrl,
            string clientId,
            string clientSecret,
            string refreshToken,
            bool hasBasicAuthorization);

        Task<OAuth2Credentials> GetAccessToken(
            Uri accessUrl,
            string clientId,
            string clientSecret,
            Uri callbackUrl,
            string code,
            string scope,
            bool hasBasicAuthorization);
    }
}
