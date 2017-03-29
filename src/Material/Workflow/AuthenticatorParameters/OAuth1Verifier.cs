using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth1Verifier : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Verifier.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1Verifier(string verifier)
        {
            Value = verifier;
        }
    }
}
