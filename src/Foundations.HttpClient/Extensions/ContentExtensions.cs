using System;
using System.Text;
using Foundations.Enums;

namespace Foundations.HttpClient.Extensions
{
    public static class ContentExtensions
    {
        public static HttpRequestBuilder JsonContent(
            this HttpRequestBuilder instance, 
            object content)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Content(
                content,
                MediaType.Json);
        }

        public static HttpRequestBuilder Content(
            this HttpRequestBuilder instance, 
            object newContent,
            MediaType mediaType)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var bytes = newContent as byte[];
            if (bytes != null)
            {
                return instance.RawContent(
                    bytes, 
                    mediaType);
            }
            else
            {
                return instance.Content(
                    newContent,
                    mediaType,
                    Encoding.UTF8);
            }
        }
    }
}
