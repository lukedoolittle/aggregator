using Foundations.Http;

namespace Quantfabric.Test.Material.OAuth2Server
{
    public interface IOAuthHandler
    {
        void HandleRequest(
            IncomingMessage request, 
            ServerResponse response);
    }
}
