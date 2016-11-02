using System.Net.Http;

namespace Foundations.HttpClient.Request
{
    public interface IRequestContent
    {
        HttpContent GetContent();
    }
}
