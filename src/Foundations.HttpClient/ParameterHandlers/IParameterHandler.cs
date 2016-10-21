using System.Collections.Generic;
using System.Net.Http;
using Foundations.Collections;

namespace Foundations.HttpClient.ParameterHandlers
{
    public interface IParameterHandler
    {
        void AddParameters(
            HttpRequestMessage message,
            HttpValueCollection parameters);
    }
}
