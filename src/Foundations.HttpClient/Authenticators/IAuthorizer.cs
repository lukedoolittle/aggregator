namespace Foundations.HttpClient.Authenticators
{
    public interface IAuthorizer
    {
        void Authenticate(HttpRequestBuilder requestBuilder);
    }
}
