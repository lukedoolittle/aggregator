using System.Collections.Generic;
using System.Net.Http;
using Foundations.Http;

namespace Foundations.HttpClient.ParameterHandlers
{
    public interface IParameterHandler
    {
        void AddParameters(
            HttpRequestMessage message,
            MediaTypeEnum contentType,
            IEnumerable<KeyValuePair<string, string>> parameters);
    }
}
