using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundations.Collections;
using Foundations.Enums;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
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
        private readonly MediaType _responseContentType;

        private readonly DefaultingDictionary<string, Encoding> _encodings =
            new DefaultingDictionary<string, Encoding>(s => Encoding.UTF8)
            {
                {ContentTypeEncodingEnum.UTF16BigEndian.EnumToString(),
                    Encoding.BigEndianUnicode},
                {ContentTypeEncodingEnum.UTF16LittleEndian.EnumToString(),
                    Encoding.Unicode}
            };

        public HttpResponse(
            HttpResponseMessage response,
            IEnumerable cookies,
            MediaType expectedResponseType)
        {
            _content = response.Content;
            Headers = response.Headers;
            StatusCode = response.StatusCode;
            Reason = response.ReasonPhrase;
            Cookies = cookies.Cast<Cookie>();
            _responseContentType = expectedResponseType != MediaType.Undefined
                ? expectedResponseType
                : response
                        .Content
                        .Headers
                        .ContentType
                        .MediaType
                        .StringToEnum<MediaType>();
        }

        public async Task<string> ContentAsync()
        {
            var buffer = await _content
                .ReadAsByteArrayAsync()
                .ConfigureAwait(false);

            var responseString = 
                _encodings[_content.Headers.ContentType.CharSet]
                .GetString(buffer, 0, buffer.Length);

            return responseString;
        }

        public async Task<T> ContentAsync<T>()
        {
            var result = await ContentAsync()
                .ConfigureAwait(false);

            var serializer = HttpConfiguration.ContentSerializers[
                _responseContentType];

            var datetimeFormatter = typeof(T)
                .GetCustomAttributes<DatetimeFormatter>()
                .FirstOrDefault()
                ?.Formatter;

            return serializer.Deserialize<T>(
                result, 
                datetimeFormatter);
        }
    }
}
