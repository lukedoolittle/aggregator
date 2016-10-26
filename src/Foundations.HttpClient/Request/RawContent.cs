using System.Net.Http;
using System.Net.Http.Headers;
using Foundations.Enums;
using Foundations.Extensions;

namespace Foundations.HttpClient.Request
{
    public class RawContent : IRequestContent
    {
        private readonly byte[] _content;
        private readonly MediaType _mediaType;

        public RawContent(
            byte[] content, 
            MediaType mediaType)
        {
            _content = content;
            _mediaType = mediaType;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public HttpContent GetContent()
        {
            var content = new ByteArrayContent(_content);
            content.Headers.ContentType = 
                new MediaTypeHeaderValue(_mediaType.EnumToString());
            return content;
        }
    }
}
