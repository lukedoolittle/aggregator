using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class Username : IAuthenticatorParameter
    {
        public string Name { get; }
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public Username(
            string username, 
            string usernameKey)
        {
            Name = usernameKey;
            Value = username;
        }
    }
}
