using System;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2CodeVerifier : IAuthenticatorParameter
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public string Name => OAuth2Parameter.Verifier.EnumToString();
        public string Value => _securityStrategy.GetSecureParameter(_userId, Name);
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
