using System.Collections.Generic;
using System.Net.Http;
using Foundations.Http;
using Foundations.HttpClient.Extensions;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class GetParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message, 
            MediaTypeEnum contentType,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            message.RequestUri = message
                                    .RequestUri
                                    .AddEncodedQuerystring(
                                        parameters);
        }
    }
}
