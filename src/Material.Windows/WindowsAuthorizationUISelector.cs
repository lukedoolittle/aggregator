using System;
using Material.Contracts;
using Material.Domain.Core;
using Material.Framework.Enums;

namespace Material
{
    public class WindowsAuthorizationUISelector : IAuthorizationUISelector
    {
        //For a windows console app we leave the interface as unspecified because there
        //really isn't a way to specify interfaces (there is only 1)

        public AuthorizationInterface GetOptimalOAuth1Interface<TResourceProvider>(
            TResourceProvider provider,
            AuthorizationInterface selectedInterface)
            where TResourceProvider : OAuth1ResourceProvider
        {
            return AuthorizationInterface.NotSpecified;
        }

        public AuthorizationInterface GetOptimalOAuth2Interface<TResourceProvider>(
            TResourceProvider provider,
            AuthorizationInterface selectedInterface, 
            Uri callbackUri) 
            where TResourceProvider : OAuth2ResourceProvider
        {
            return AuthorizationInterface.NotSpecified;
        }
    }
}
