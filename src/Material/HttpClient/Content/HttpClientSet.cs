using System.Net.Http;

namespace Material.HttpClient.Content
{
    public class HttpClientSet
    {
        public HttpClientSet(
            System.Net.Http.HttpClient client, 
            HttpClientHandler handler, 
            HttpRequestMessage message)
        {
            Client = client;
            Handler = handler;
            Message = message;
        }

        public System.Net.Http.HttpClient Client { get; }
        public HttpClientHandler Handler { get; }
        public HttpRequestMessage Message { get; }
    }
}
