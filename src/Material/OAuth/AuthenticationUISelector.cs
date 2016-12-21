using Material.Enums;
using Material.Infrastructure;

namespace Material.OAuth
{
    public class AuthenticationUISelector
    {
        private readonly bool _canProvideSecureBrowsing;

        public AuthenticationUISelector(bool canProvideSecureBrowsing)
        {
            _canProvideSecureBrowsing = canProvideSecureBrowsing;
        }

        public AuthorizationInterface GetOptimalOAuth1Interface<TResourceProvider>(
            TResourceProvider provider,
            AuthorizationInterface selectedInterface)
            where TResourceProvider : OAuth1ResourceProvider
        {
            if (selectedInterface != AuthorizationInterface.NotSpecified)
            {
                return selectedInterface;
            }
            else
            {
                if (provider.SupportsCustomUrlScheme)
                {
                    return _canProvideSecureBrowsing ? 
                        AuthorizationInterface.SecureEmbedded : 
                        AuthorizationInterface.Dedicated;
                }
                else
                {
                    return AuthorizationInterface.Embedded;
                }
            }
        }

        public AuthorizationInterface GetOptimalOAuth2Interface<TResourceProvider>(
            TResourceProvider provider,
            AuthorizationInterface selectedInterface)
            where TResourceProvider : OAuth2ResourceProvider
        {
            if (selectedInterface != AuthorizationInterface.NotSpecified)
            {
                return selectedInterface;
            }
            else
            {
                if (provider.SupportsCustomUrlScheme)
                {
                    return _canProvideSecureBrowsing ?
                        AuthorizationInterface.SecureEmbedded :
                        AuthorizationInterface.Dedicated;
                }
                else
                {
                    return AuthorizationInterface.Embedded;
                }
            }
        }
    }
}
