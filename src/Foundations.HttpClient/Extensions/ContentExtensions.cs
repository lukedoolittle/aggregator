using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Foundations.Enums;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient.Extensions
{
    public static class ContentExtensions
    {
        public static HttpRequestBuilder JsonContent(
            this HttpRequestBuilder instance, 
            object content)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return instance.Content(
                content,
                MediaType.Json);
        }

        public static HttpRequestBuilder Content(
            this HttpRequestBuilder instance,
            IEnumerable<BodyContent> newContent)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (newContent == null) throw new ArgumentNullException(nameof(newContent));

            foreach (var content in newContent)
            {
                instance.Content(
                    content.Content, 
                    content.MediaType, 
                    content.Encoding);
            }

            return instance;
        }

        public static HttpRequestBuilder Content(
            this HttpRequestBuilder instance,
            object newContent,
            MediaType mediaType,
            Encoding encoding)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var bytes = newContent as byte[];
            if (bytes != null)
            {
                return instance.RawContent(
                    bytes,
                    mediaType);
            }

            var stream = newContent as Stream;
            if (stream != null)
            {
                return instance.StreamingContent(
                    stream,
                    mediaType);
            }

            return instance.SerializableContent(
                newContent,
                mediaType,
                encoding);
        }

        public static HttpRequestBuilder Content(
            this HttpRequestBuilder instance, 
            object newContent,
            MediaType mediaType)
        {
            return Content(instance, newContent, mediaType, Encoding.UTF8);
        }

        public static HttpRequestBuilder ResponseMediaTypes(
            this HttpRequestBuilder instance,
            IEnumerable<MediaType> mediaTypes)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (mediaTypes == null) throw new ArgumentNullException(nameof(mediaTypes));

            foreach (var mediaType in mediaTypes)
            {
                instance.Accepts(mediaType);
            }

            return instance;
        }
    }
}
