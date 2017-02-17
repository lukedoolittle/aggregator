using System.Text;
using Foundations.Enums;

namespace Foundations.HttpClient.Request
{
    public class BodyContent
    {
        public object Content { get; }
        public MediaType MediaType { get; }
        public Encoding Encoding { get; }

        public BodyContent(
            object content, 
            MediaType mediaType, 
            Encoding encoding)
        {
            Content = content;
            MediaType = mediaType;
            Encoding = encoding;
        }
    }
}
