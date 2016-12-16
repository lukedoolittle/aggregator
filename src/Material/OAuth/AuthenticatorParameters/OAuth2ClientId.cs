using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2ClientId : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.ClientId.EnumToString();
        public string Value { get; }

        public OAuth2ClientId(string clientId)
        {
            Value = clientId;
        }
    }
}
