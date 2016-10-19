using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Foundations.Extensions;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class QuerystringParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message, 
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
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
