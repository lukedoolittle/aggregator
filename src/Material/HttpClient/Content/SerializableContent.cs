using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.Framework.Metadata;

namespace Material.HttpClient.Content
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
            if (content == null) throw new ArgumentNullException(nameof(content));

            _content = content;
            _encoding = encoding;
            _mediaType = mediaType;
        }

        public HttpContent GetContent()
        {
            var datetimeFormatter = _content.GetType()
                .GetCustomAttributes<ClassDateTimeFormatterAttribute>()
                .FirstOrDefault()
                ?.Formatter;

            var serializedContent = HttpConfiguration
                .ContentSerializers[_mediaType]
                .Serialize(_content, datetimeFormatter);

            return new StringContent(
                serializedContent,
                _encoding,
                _mediaType.EnumToString());
        }
    }
}
