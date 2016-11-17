using System;

namespace Material.Infrastructure
{
    public abstract class OpenIdResourceProvider : OAuth2ResourceProvider
    {
        public abstract Uri OpenIdDiscoveryUrl { get; }
    }
}
