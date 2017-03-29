using System;

namespace Material.Domain.Core
{
    public abstract class ApiKeyExchangeResourceProvider : ApiKeyResourceProvider
    {
        public abstract Uri TokenUrl { get; }

        public abstract string TokenName { get; }

        public virtual Uri OpenIdDiscoveryUrl { get; }
    }
}
