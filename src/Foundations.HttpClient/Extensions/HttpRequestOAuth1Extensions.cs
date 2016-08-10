using System;
using Foundations.HttpClient.Authenticators;

namespace Foundations.HttpClient.Extensions
{
    public static class HttpRequestOAuth1Extensions
    {
        public static HttpRequest ForOAuth1RequestToken(
            this HttpRequest instance,
            string consumerKey,
            string consumerSecret,
            string callbackUri)
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            var authenticator = new OAuth1RequestToken(
                consumerKey,
                consumerSecret,
                callbackUri);

            return instance.Authenticator(authenticator);
        }

        public static HttpRequest ForOAuth1AccessToken(
            this HttpRequest instance,
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret,
            string verifier)
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            var authenticator = new OAuth1AccessToken(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                verifier);

            return instance.Authenticator(authenticator);
        }
    }
}
