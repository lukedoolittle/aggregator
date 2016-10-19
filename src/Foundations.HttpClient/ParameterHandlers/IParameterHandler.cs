using System.Collections.Generic;
using System.Net.Http;

namespace Foundations.HttpClient.ParameterHandlers
{
    public interface IParameterHandler
    {
        void AddParameters(
            HttpRequestMessage message,
            IEnumerable<KeyValuePair<string, string>> parameters);
    }
}
