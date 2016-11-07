using Foundations.Collections;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient.ParameterHandlers
{
    public interface IParameterHandler
    {
        void AddParameters(
            RequestParameters message,
            HttpValueCollection parameters);
    }
}
