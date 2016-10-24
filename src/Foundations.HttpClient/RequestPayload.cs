using System;
using System.Net.Http;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.ParameterHandlers;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient
{
    public class RequestPayload
    {
        private Body _content;
        private IParameterHandler _parameterHandler = 
            new QuerystringParameterHandler();

        public HttpValueCollection QueryParameters { get; } =
            new HttpValueCollection();

        public void AddContent(Body content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            _content = content;

            _parameterHandler = new QuerystringParameterHandler();
        }

        public void AddParameters(
            HttpValueCollection parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            QueryParameters.Add(parameters);
        }

        public void AddParameter(string key, string value)
        {
            QueryParameters.Add(key, value);
        }

        public void SetParameterHandling(HttpParameterType parameterType)
        {
            if (parameterType == HttpParameterType.Body && _content == null)
            {
                _parameterHandler = new BodyParameterHandler();
            }

            //TODO: either support header or remove header from enum
            if (parameterType == HttpParameterType.Header)
            {
                throw new NotSupportedException(
                    StringResource.HeaderParametersNotSupported);
            }
        }

        public void Attach(HttpRequestMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (_content != null)
            {
                var serializedContent = HttpConfiguration
                    .ContentSerializers[_content.MediaType]
                    .Serialize(_content.Content);

                message.Content = new StringContent(
                    serializedContent,
                    _content.Encoding,
                    _content.MediaType.EnumToString());
            }

            _parameterHandler.AddParameters(
                message,
                QueryParameters);
        }
    }
}
