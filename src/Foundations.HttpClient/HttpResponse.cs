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
        private readonly HttpContentHeaders _contentHeaders;
        private readonly HttpStatusCode _statusCode;
        private readonly string _reason;

        private readonly MediaTypeEnum _mediaType;
        private readonly Encoding _encoding;

        public HttpResponse(
            HttpContent content, 
            HttpResponseHeaders headers,
            HttpStatusCode statusCode,
            string reason)
        {
            _content = content;
            _headers = headers;
            _contentHeaders = _content.Headers;
            _statusCode = statusCode;
            _reason = reason;

            var charset = _contentHeaders.ContentType.CharSet;

            if (charset == ContentTypeEncodingEnum.UTF16BigEndian.EnumToString())
            {
                _encoding = Encoding.BigEndianUnicode;
            }
            else if (charset == ContentTypeEncodingEnum.UTF16LittleEndian.EnumToString())
            {
                _encoding = Encoding.Unicode;
            }
            else
            {
                _encoding = Encoding.UTF8;
            }

            _mediaType = _contentHeaders
                .ContentType
                .MediaType
                .StringToEnum<MediaTypeEnum>();
        }

        public async Task<string> ContentAsync()
        {
            var buffer = await _content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var responseString = _encoding.GetString(buffer, 0, buffer.Length);
            return responseString;
        }

        public async Task<T> ContentAsync<T>()
        {
            //TODO: possibly this should be done polymorphically
            //and should handle more scenarios
            var result = await ContentAsync()
                .ConfigureAwait(false);

            if (_mediaType == MediaTypeEnum.Json)
            {
                return result.AsEntity<T>(false);
            }
            else
            {
                return HttpUtility.ParseQueryString(result)
                    .AsEntity<T>();
            }
        }
    }
}
