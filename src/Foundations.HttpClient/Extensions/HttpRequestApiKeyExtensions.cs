using System;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Extensions
{
    public static class HttpRequestApiKeyExtensions
    {
        public static HttpRequestBuilder ForApiKeyProtectedResource(
            this HttpRequestBuilder instance,
            string keyName,
            string keyValue,
            HttpParameterType keyType)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var authenticator = new ApiKeyProtectedResource(
                keyName,
                keyValue,
                keyType);

            return instance.Authenticator(authenticator);
        }
    }
}
