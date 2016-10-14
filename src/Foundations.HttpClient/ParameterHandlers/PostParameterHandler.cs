using System;
using System.Collections.Generic;
using System.Net.Http;
using Foundations.Extensions;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class PostParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message, 
            MediaType contentType,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (message.Content != null)
            {
                message.RequestUri = message
                    .RequestUri
                    .AddEncodedQuerystring(
                        parameters);
            }
            else
            {
                if (contentType == MediaType.Form)
                {
                    message.Content = new FormUrlEncodedContent(parameters);
                }
                else if (contentType == MediaType.Json)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
