using System.Net.Http;

namespace Foundations.HttpClient.Request
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
