using System;

namespace Material.Infrastructure
{
    public abstract class ApiKeyExchangeResourceProvider : ApiKeyResourceProvider
    {
        public abstract Uri TokenUrl { get; }

        public abstract string TokenName { get; }

        public virtual Uri OpenIdDiscoveryUrl { get; }
    }
}
