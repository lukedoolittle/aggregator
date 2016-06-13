using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aggregator.Infrastructure.Credentials;

namespace Aggregator.Framework.Contracts
{
    public interface IOAuth1
    {
        Task<OAuth1Credentials> GetRequestToken(
            Uri requestUrl,
            string consumerKey,
            string consumerSecret,
            Uri callbackUrl);

        Uri GetAuthorizationPath(
            string oauthToken,
            Uri authorizeUri);

        Task<OAuth1Credentials> GetAccessToken(
            Uri accessUri,
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            string verifier,
            Dictionary<string, string> additionalQuerystringParameters);
    }
}
