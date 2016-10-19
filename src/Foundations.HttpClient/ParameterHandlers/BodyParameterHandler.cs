using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class BodyParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return;
            }

            message.Content = new FormUrlEncodedContent(parameters);
        }
    }
}
