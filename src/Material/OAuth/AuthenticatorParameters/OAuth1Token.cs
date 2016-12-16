using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1Token : IAuthenticatorParameter
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public string Name => OAuth1Parameter.OAuthToken.EnumToString();
        public string Value => _securityStrategy.CreateOrGetSecureParameter(_userId, Name);

        public OAuth1Token(
            IOAuthSecurityStrategy strategy, 
            string userId)
        {
            _securityStrategy = strategy;
            _userId = userId;
        }
    }
}
