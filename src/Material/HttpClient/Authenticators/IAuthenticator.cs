namespace Material.HttpClient.Authenticators
{
    public interface IAuthenticator
    {
        void Authenticate(HttpRequestBuilder requestBuilder);
    }
}
