using System;
using System.Linq;
using System.Net.Http;
using Foundations.Collections;
using Foundations.Extensions;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class QuerystringParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message, 
            HttpValueCollection parameters)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (parameters == null || !parameters.Any())
            {
                return;
            }

            message.RequestUri = message
                                    .RequestUri
                                    .AddEncodedQuerystring(
                                        parameters);
        }
    }
}
