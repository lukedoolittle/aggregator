using Material.Framework.Collections;
using Material.HttpClient.Content;

namespace Material.HttpClient.ParameterHandlers
{
    public interface IParameterHandler
    {
        void AddParameters(
            RequestParameters message,
            HttpValueCollection parameters);
    }
}
