using System;
using Foundations.HttpClient.Authenticators;

namespace Foundations.HttpClient.Extensions
{
    public static class HttpRequestOAuth1Extensions
    {
        public static HttpRequestBuilder ForOAuth1RequestToken(
            this HttpRequestBuilder instance,
            string consumerKey,
            string consumerSecret,
            Uri callbackUri)
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

        public static HttpRequestBuilder ForOAuth1AccessToken(
            this HttpRequestBuilder instance,
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

        public static HttpRequestBuilder ForOAuth1ProtectedResource(
            this HttpRequestBuilder instance,
            string consumerKey,
            string consumerSecret,
            string oauthToken,
            string oauthSecret)
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            var authenticator = new OAuth1ProtectedResource(
                consumerKey, 
                consumerSecret, 
                oauthToken, 
                oauthSecret);

            return instance.Authenticator(authenticator);
        }
    }
}
