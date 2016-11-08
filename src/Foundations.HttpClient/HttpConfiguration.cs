using System;
using System.Collections.Concurrent;
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
        /// Creates the default HttpClient instance internal to HttpRequestBuilder
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Func<RequestParameters, Tuple<System.Net.Http.HttpClient, HttpClientHandler>> HttpClientFactory { get; set; } = //-V3070
            (request) => ClientPool.GetClient(request.Address, MessageHandlerFactory);

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


    public static class ClientPool
    {
        private static readonly object _syncLock = new object();
        private static readonly ConcurrentDictionary<string, Tuple<System.Net.Http.HttpClient, HttpClientHandler>> _clients =
            new ConcurrentDictionary<string, Tuple<System.Net.Http.HttpClient, HttpClientHandler>>();

        //Per discussions http://byterot.blogspot.com/2016/07/singleton-httpclient-dns.html IDisposable does not need implementing
        //with HttpClient
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Tuple<System.Net.Http.HttpClient, HttpClientHandler> GetClient(
            Uri uri,
            Func<HttpClientHandler> clientHandlerFactory)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (clientHandlerFactory == null) throw new ArgumentNullException(nameof(clientHandlerFactory));

            var key = uri.ToString();

            lock (_syncLock)
            {
                if (!_clients.ContainsKey(key))
                {
                    var handler = clientHandlerFactory();
                    var client = new System.Net.Http.HttpClient(handler);
                    _clients.AddOrUpdate(
                        key,
                        new Tuple<System.Net.Http.HttpClient, HttpClientHandler>(client, handler),
                        (s, pair) => { throw new NotSupportedException(); });
                }

                return _clients[key];
            }
        }
    }
}
