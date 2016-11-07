using System;
using Foundations.Collections;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.ParameterHandlers;

namespace Foundations.HttpClient.Request
{
    public class RequestPayload
    {
        private IRequestContent _content;
        private IParameterHandler _parameterHandler = 
            new QuerystringParameterHandler();

        public HttpValueCollection QueryParameters { get; } =
            new HttpValueCollection();

        public void AddContent(IRequestContent content)
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

        public void Attach(RequestParameters message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (_content != null)
            {
                message.Content = _content.GetContent();
            }

            _parameterHandler.AddParameters(
                message,
                QueryParameters);
        }
    }
}
