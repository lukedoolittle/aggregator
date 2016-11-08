using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundations.HttpClient.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuth1AuthorizationAdapter
    {
        Task<OAuth1Credentials> GetRequestToken(
            Uri requestUrl,
            string consumerKey,
            string consumerSecret,
            HttpParameterType parameterHandling,
            Uri callbackUrl);

        Uri GetAuthorizationUri(
            string oauthToken,
            Uri authorizeUri);

        Task<OAuth1Credentials> GetAccessToken(
            Uri accessUri,
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            string verifier,
            HttpParameterType parameterHandling,
            IDictionary<string, string> queryParameters);
    }
}
