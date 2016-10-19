using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Serialization;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient
{
    public static class HttpConfiguration
    {
        public static Func<HttpClientHandler> MessageHandlerFactory = 
            () => new HttpClientHandler();

        public static IDictionary<MediaType, ISerializer> ContentSerializers =
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
