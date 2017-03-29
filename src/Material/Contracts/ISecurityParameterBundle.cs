using System.Collections.Generic;
using Material.HttpClient.Authenticators;

namespace Material.Contracts
{
    public interface ISecurityParameterBundle
    {
        IEnumerable<IAuthenticatorParameter> GetBundle(
            IOAuthSecurityStrategy securityStrategy, 
            string userId);
    }
}
