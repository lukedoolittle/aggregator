using System;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2Nonce : IAuthenticatorParameter
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public string Name => OAuth2Parameter.Nonce.EnumToString();
        public string Value => _securityStrategy.CreateSecureParameter(_userId, Name);
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2Nonce(
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
