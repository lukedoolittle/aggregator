using System;
using System.Collections.Generic;
using System.Net.Http;
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
            throw new NotImplementedException();
        }
    }
}
