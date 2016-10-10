using Foundations.Http;

namespace Quantfabric.Test.Material.OAuth2Server
{
    public interface IOAuthHandler
    {
        void HandleRequest(
            IncommingMessage request, 
            ServerResponse response);
    }
}
