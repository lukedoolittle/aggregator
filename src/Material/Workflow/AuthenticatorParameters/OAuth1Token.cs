using System;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth1Token : IAuthenticatorParameter
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;
        private readonly string _oauthToken;

        public string Name => OAuth1Parameter.OAuthToken.EnumToString();
        public string Value => _securityStrategy?.GetSecureParameter(_userId, Name) ?? _oauthToken;
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1Token(string oauthToken)
        {
            _oauthToken = oauthToken;
        }

        public OAuth1Token(
            IOAuthSecurityStrategy strategy, 
            string userId)
        {
            if (strategy == null) throw new ArgumentNullException(nameof(strategy));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            _securityStrategy = strategy;
            _userId = userId;
        }
    }
}
