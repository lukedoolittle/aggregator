using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.Http;
using Foundations.HttpClient.Enums;
using Foundations.Serialization;

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

        public HttpResponse(
            HttpContent content, 
            HttpResponseHeaders headers,
            HttpStatusCode statusCode,
            string reason)
        {
            _content = content;
            _headers = headers;
            _statusCode = statusCode;
            _reason = reason;
        }

        public async Task<string> ContentAsync()
        {
            var buffer = await _content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var responseString = 
                GetEncoding(_content.Headers.ContentType)
                .GetString(buffer, 0, buffer.Length);
            return responseString;
        }

        public async Task<T> ContentAsync<T>()
        {
            //TODO: possibly this should be done polymorphically
            //and should handle more scenarios
            var result = await ContentAsync()
                .ConfigureAwait(false);

            var mediaType = GetMediaType(_content.Headers.ContentType);

            if (mediaType == MediaTypeEnum.Json)
            {
                return result.AsEntity<T>(false);
            }
            else
            {
                return HttpUtility.ParseQueryString(result)
                    .AsEntity<T>();
            }
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

        private static MediaTypeEnum GetMediaType(MediaTypeHeaderValue header)
        {
            return header
                .MediaType
                .StringToEnum<MediaTypeEnum>();
        }
    }
}
