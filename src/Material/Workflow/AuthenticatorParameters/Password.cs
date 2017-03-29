using Material.Framework.Enums;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class Password : IAuthenticatorParameter
    {
        public string Name { get; }
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public Password(
            string password, 
            string passwordKey)
        {
            Name = passwordKey;
            Value = password;
        }
    }
}
