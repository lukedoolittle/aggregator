using System;
using System.Net.Http;

namespace Foundations.HttpClient.Request
{
    public class ClientItems
    {
        public System.Net.Http.HttpClient Client { get; }
        public HttpClientHandler Handler { get; }

        public ClientItems(
            System.Net.Http.HttpClient client,
            HttpClientHandler handler)
        {
            Client = client;
            Handler = handler;
        }
    }

    public interface IClientPool
    {
        ClientItems GetClient(Uri uri);
    }
}
