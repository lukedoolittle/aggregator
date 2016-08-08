namespace Foundations.HttpClient.Authenticators
{
    public interface IAuthenticator
    {
        void Authenticate(HttpRequest request);
    }
}
