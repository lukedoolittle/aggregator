using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class PostParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message, 
            MediaType contentType,
            IEnumerable<KeyValuePair<string, string>> parameters)
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
