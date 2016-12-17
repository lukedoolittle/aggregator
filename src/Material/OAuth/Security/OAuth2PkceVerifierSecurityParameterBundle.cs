using System;
using System.Collections.Generic;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Security
{
    public class OAuth2PkceVerifierSecurityParameterBundle : ISecurityParameterBundle
    {
        public IEnumerable<IAuthenticatorParameter> GetBundle(
            IOAuthSecurityStrategy securityStrategy,
            string userId)
        {
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            return new List<IAuthenticatorParameter>
            {
                new OAuth2CodeVerifier(securityStrategy, userId)
            };
        }
    }
}
