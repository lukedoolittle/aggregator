using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2CodeVerifier : IAuthenticatorParameter
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public string Name => OAuth2Parameter.Verifier.EnumToString();
        public string Value => _securityStrategy.CreateOrGetSecureParameter(_userId, Name);
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2CodeVerifier(
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
