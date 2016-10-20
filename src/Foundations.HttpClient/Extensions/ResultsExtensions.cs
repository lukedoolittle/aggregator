using System;
using System.Threading.Tasks;

namespace Foundations.HttpClient.Extensions
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
    }
}
