using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Foundations.Http;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class GetParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message, 
            MediaTypeEnum contentType,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var builder = new StringBuilder();

            var separator = "";
            foreach (var kvp in parameters)
            {
                builder.Append($"{separator}{kvp.Key}={kvp.Value}");
                separator = "&";
            }

            var uriBuilder = new UriBuilder(message.RequestUri)
            {
                Query = builder.ToString()
            };
            message.RequestUri = uriBuilder.Uri;
        }
    }
}
