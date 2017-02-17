using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Foundations.Collections;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.ParameterHandlers;

namespace Foundations.HttpClient.Request
{
    public class RequestPayload
    {
        private readonly IList<IRequestContent> _contents = 
           new List<IRequestContent>();

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

            _contents.Add(content);

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
            if (parameterType == HttpParameterType.Body && _contents.Count == 0)
            {
                _parameterHandler = new BodyParameterHandler();
            }

            //either support header or remove header from enum
            if (parameterType == HttpParameterType.Header)
            {
                throw new NotSupportedException(
                    StringResource.HeaderParametersNotSupported);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public void AttachContent(RequestParameters message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (_contents.Count == 1)
            {
                message.Content = _contents.First().GetContent();
            }
            else if (_contents.Count > 1)
            {
                var multipartContent = new MultipartFormDataContent();

                foreach (var item in _contents)
                {
                    multipartContent.Add(item.GetContent());
                }

                message.Content = multipartContent;
            }
        }

        public void AttachParameters(RequestParameters message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            _parameterHandler.AddParameters(
                message,
                QueryParameters);
        }
    }
}
