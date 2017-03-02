using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2State : IAuthenticatorParameter
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public string Name => OAuth2Parameter.State.EnumToString();
        public string Value => _securityStrategy.CreateSecureParameter(_userId, Name);
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2State(
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
