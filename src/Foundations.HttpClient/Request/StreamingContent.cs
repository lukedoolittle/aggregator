using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Foundations.Enums;
using Foundations.Extensions;

namespace Foundations.HttpClient.Request
{
    public class StreamingContent : IRequestContent
    {
        private readonly Stream _content;
        private readonly MediaType _mediaType;

        public StreamingContent(
            Stream content, 
            MediaType mediaType)
        {
            _content = content;
            _mediaType = mediaType;
        }

        public HttpContent GetContent()
        {
            var content = new StreamContent(_content);
            content.Headers.ContentType = 
                new MediaTypeHeaderValue(_mediaType.EnumToString());
            return content;
        }
    }
}
