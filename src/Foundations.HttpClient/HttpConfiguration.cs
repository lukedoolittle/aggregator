using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using Foundations.Collections;
using Foundations.Enums;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient
{
    public static class HttpConfiguration
    {
        public static Func<HttpClientHandler> MessageHandlerFactory { get; set; } =
            () => new HttpClientHandler {CookieContainer = new CookieContainer()};

        public static Action<HttpRequestBuilder> DefaultBuilderSetup { get; set; } = 
            builder =>
            {
                builder.AcceptsDecompressionEncoding(DecompressionMethods.GZip);
                builder.AcceptsDecompressionEncoding(DecompressionMethods.Deflate);

                builder.Accepts(MediaType.Json);
                builder.Accepts(MediaType.Xml);
                builder.Accepts(MediaType.TextJson);
                builder.Accepts(MediaType.TextXml);
                builder.Accepts(MediaType.TextXJson);
                builder.Accepts(MediaType.Javascript);
            };

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
                {MediaType.Javascript, new JsonSerializer()}
            };
    }
}
