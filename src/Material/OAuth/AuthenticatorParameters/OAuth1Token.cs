using System;
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
        private readonly string _oauthToken;

        public string Name => OAuth1Parameter.OAuthToken.EnumToString();
        public string Value => _securityStrategy?.CreateOrGetSecureParameter(_userId, Name) ?? _oauthToken;
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
