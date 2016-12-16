using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1Verifier : IAuthenticatorParameter
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public string Name => OAuth1Parameter.Verifier.EnumToString();
        public string Value => _securityStrategy.CreateOrGetSecureParameter(_userId, Name);

        public OAuth1Verifier(
            IOAuthSecurityStrategy strategy, 
            string userId)
        {
            _securityStrategy = strategy;
            _userId = userId;
        }
    }
}
