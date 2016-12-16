using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2Code : IAuthenticatorParameter
    {
        public string Name => throw
        new Exception();
        public string Value { get; }

        public OAuth2Code(string code)
        {
            Value = code;
        }
    }
}
