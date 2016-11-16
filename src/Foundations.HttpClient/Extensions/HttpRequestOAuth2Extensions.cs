using System;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;

namespace Foundations.HttpClient.Extensions
{
    public static class HttpRequestOAuth2Extensions
    {
        public static HttpRequestBuilder ForOAuth2AccessToken(
            this HttpRequestBuilder instance,
            string clientId,
            string clientSecret,
            Uri redirectUrl,
            string code,
            string scope)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var authenticator = new OAuth2AccessToken(
                clientId, 
                clientSecret, 
                redirectUrl, 
                code, 
                scope);

            return instance.Authenticator(authenticator);
        }

        public static HttpRequestBuilder ForOAuth2ClientAccessToken(
            this HttpRequestBuilder instance,
            string clientId,
            string clientSecret)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var authenticator = new OAuth2ClientAccessToken(
                clientId,
                clientSecret);

            return instance.Authenticator(authenticator);
        }

        public static HttpRequestBuilder ForOAuth2RefreshToken(
            this HttpRequestBuilder instance,
            string clientId,
            string clientSecret,
            string refreshToken)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var authenticator = new OAuth2RefreshToken(
                clientId,
                clientSecret, 
                refreshToken);

            return instance.Authenticator(authenticator);
        }

        public static HttpRequestBuilder ForOAuth2JsonWebToken(
            this HttpRequestBuilder instance,
            string header,
            string claims,
            JsonWebTokenAlgorithm algorithm,
            CryptoKey privateKey,
            string clientId)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var authenticator = new OAuth2JsonWebToken(
                header,
                claims,
                algorithm,
                privateKey,
                clientId);

            return instance.Authenticator(authenticator);
        }

        public static HttpRequestBuilder ForOAuth2ProtectedResource(
            this HttpRequestBuilder instance,
            string accessToken,
            string accessTokenName)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var authenticator = new OAuth2ProtectedResource(
                accessToken,
                accessTokenName);

            return instance.Authenticator(authenticator);
        }
    }
}
