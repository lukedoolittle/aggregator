

namespace Foundations.HttpClient.Authenticators
{
    public interface IRequestSigningAlgorithm
    {
        void SignRequest(HttpRequestBuilder builder);
    }
}
