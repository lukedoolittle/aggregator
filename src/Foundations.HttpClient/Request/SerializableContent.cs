using System.Net.Http;
using System.Text;
using Foundations.Enums;
using Foundations.Extensions;

namespace Foundations.HttpClient.Request
{
    public class SerializableContent : IRequestContent
    {
        private readonly object _content;
        private readonly Encoding _encoding;
        private readonly MediaType _mediaType;

        public SerializableContent(
            object content, 
            Encoding encoding, 
            MediaType mediaType)
        {
            _content = content;
            _encoding = encoding;
            _mediaType = mediaType;
        }

        public HttpContent GetContent()
        {
            var serializedContent = HttpConfiguration
                .ContentSerializers[_mediaType]
                .Serialize(_content);

            return new StringContent(
                serializedContent,
                _encoding,
                _mediaType.EnumToString());
        }
    }
}
