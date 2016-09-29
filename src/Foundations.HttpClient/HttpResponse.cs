using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;
using Foundations.HttpClient.Metadata;

namespace Foundations.HttpClient
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string Reason { get; }
        public IEnumerable<Cookie> Cookies { get; }
        public HttpResponseHeaders Headers { get; }

        private readonly HttpContent _content;
        private readonly ISerializer _serializer;

        public HttpResponse(
            HttpContent content, 
            HttpResponseHeaders headers,
            HttpStatusCode statusCode,
            string reason,
            IEnumerable<Cookie> cookies,
            ISerializer serializer)
        {
            _content = content;
            Headers = headers;
            StatusCode = statusCode;
            Reason = reason;
            Cookies = cookies;
            _serializer = serializer;
        }

        public async Task<string> ContentAsync()
        {
            var buffer = await _content
                .ReadAsByteArrayAsync()
                .ConfigureAwait(false);

            var responseString = 
                GetEncoding(_content.Headers.ContentType)
                .GetString(buffer, 0, buffer.Length);

            return responseString;
        }

        public async Task<T> ContentAsync<T>()
        {
            var result = await ContentAsync()
                .ConfigureAwait(false);

            if (_serializer == null)
            {
                var mediaType = _content
                    .Headers
                    .ContentType
                    .MediaType;

                throw new SerializationException(
                    $"Cannot deserialize content with media type {mediaType}");
            }

            var datetimeFormatter = typeof(T)
                .GetCustomAttributes<DatetimeFormatter>()
                .FirstOrDefault()
                ?.Formatter;

            return _serializer.Deserialize<T>(
                result, 
                datetimeFormatter);
        }

        private static Encoding GetEncoding(MediaTypeHeaderValue header)
        {
            if (header.CharSet == ContentTypeEncodingEnum.UTF16BigEndian.EnumToString())
            {
                return Encoding.BigEndianUnicode;
            }
            else if (header.CharSet == ContentTypeEncodingEnum.UTF16LittleEndian.EnumToString())
            {
                return Encoding.Unicode;
            }
            else
            {
                return Encoding.UTF8;
            }
        }
    }
}
