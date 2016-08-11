using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode => _statusCode;
        public string Reason => _reason;

        private readonly HttpContent _content;
        private readonly HttpResponseHeaders _headers;
        private readonly HttpStatusCode _statusCode;
        private readonly string _reason;
        private readonly ISerializer _serializer;

        public HttpResponse(
            HttpContent content, 
            HttpResponseHeaders headers,
            HttpStatusCode statusCode,
            string reason,
            ISerializer serializer)
        {
            _content = content;
            _headers = headers;
            _statusCode = statusCode;
            _reason = reason;
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

            return _serializer.Deserialize<T>(result);
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
