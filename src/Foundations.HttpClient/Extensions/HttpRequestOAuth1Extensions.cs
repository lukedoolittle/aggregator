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
    }
}
