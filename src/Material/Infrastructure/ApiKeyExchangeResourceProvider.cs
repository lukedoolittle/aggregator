using System;

namespace Material.Infrastructure
{
    public abstract class ApiKeyExchangeResourceProvider : ApiKeyResourceProvider
    {
        public abstract Uri TokenUrl { get; }

        public abstract string TokenName { get; }
    }
}
