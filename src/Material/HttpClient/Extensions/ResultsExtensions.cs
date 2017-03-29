using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Material.HttpClient.Extensions
{
    public static class ResultsExtensions
    {
        public static async Task<string> ResultAsync(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var result = await instance
                .ExecuteAsync()
                .ConfigureAwait(false);

            return await result
                .ContentAsync()
                .ConfigureAwait(false);
        }

        public static async Task<T> ResultAsync<T>(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var result = await instance
                .ExecuteAsync()
                .ConfigureAwait(false);

            return await result
                .ContentAsync<T>()
                .ConfigureAwait(false);
        }

        public static HttpRequestBuilder ThrowIfNotExpectedResponseCode(
            this HttpRequestBuilder instance,
            IEnumerable<HttpStatusCode> statusCodes)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (statusCodes == null) throw new ArgumentNullException(nameof(statusCodes));

            foreach (var code in statusCodes)
            {
                instance.ThrowIfNotExpectedResponseCode(code);
            }

            return instance;
        }
    }
}
