using System;
using System.Collections.Generic;
using System.Net.Http;
using Foundations.Http;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class PostParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message, 
            MediaTypeEnum contentType,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (contentType == MediaTypeEnum.Form)
            {
                message.Content = new FormUrlEncodedContent(parameters);
            }
            else if (contentType == MediaTypeEnum.Json)
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
