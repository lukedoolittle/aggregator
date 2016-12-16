namespace Foundations.HttpClient.Authenticators
{
    public interface IAuthenticatorParameter
    {
        string Name { get; }
        string Value { get; }
    }
}
