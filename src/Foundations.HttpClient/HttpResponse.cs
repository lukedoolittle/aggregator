using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.Http;
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

        public Task<string> ContentAsync()
        {
            return _content.ReadAsStringAsync();
        }

        public async Task<T> ContentAsync<T>()
        {
            //TODO: possibly this should be done polymorphically
            var result = await ContentAsync()
                .ConfigureAwait(false);

            if (_content.Headers.ContentType.MediaType ==
                MediaTypeEnum.Json.EnumToString())
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
