using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using Foundations.Collections;
using Foundations.Enums;
using Foundations.HttpClient.Request;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient
{
    public static class HttpConfiguration
    {
        /// <summary>
        /// Pool of Uri-specific clients to pull HttpClients from
        /// </summary>
        public static IClientPool ClientPool { get; } = new ClientPool();

        /// <summary>
        /// Creates the default HttpClientHandler for the HttpClient instance internal to HttpRequestBuilder
        /// </summary>
        public static Func<HttpClientHandler> MessageHandlerFactory { get; set; } =
            () => new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

        /// <summary>
        /// Defines any default configurations applied to all HttpRequestBuilder instances
        /// </summary>
        public static Action<HttpRequestBuilder> DefaultBuilderSetup { get; set; } = 
            builder =>
            {
                builder.AcceptsDecompressionEncoding(DecompressionMethods.GZip);
                builder.AcceptsDecompressionEncoding(DecompressionMethods.Deflate);
            };

        /// <summary>
        /// Maps all request and response media types to their corresponding serializers
        /// </summary>
        public static IDictionary<MediaType, ISerializer> ContentSerializers { get; } =
            new DefaultingDictionary<MediaType, ISerializer>(type =>
            {
                throw new SerializationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResource.UnknownMediaType,
                        type));
            })
            {
                {MediaType.Json, new JsonSerializer()},
                {MediaType.TextJson, new JsonSerializer()},
                {MediaType.TextXJson, new JsonSerializer()},
                {MediaType.Xml, new XmlSerializer()},
                {MediaType.TextXml, new XmlSerializer()},
                {MediaType.Html, new HtmlSerializer()},
                {MediaType.Text, new HtmlSerializer()},
                {MediaType.Form, new HtmlSerializer()},
                {MediaType.Javascript, new JsonSerializer()},
                {MediaType.RunkeeperFitnessActivity, new JsonSerializer()}
            };

        /// <summary>
        /// Default media type if none is given and response does not contain a Content-Type header
        /// </summary>
        public static MediaType DefaultResponseMediaType { get; } = MediaType.Json;
    }
}
