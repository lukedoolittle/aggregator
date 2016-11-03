using System;

namespace Material.Infrastructure
{
    //TODO: should override GetHashCode() for this value object
    public abstract class ApiKeyExchangeResourceProvider : ApiKeyResourceProvider
    {
        public abstract Uri TokenUrl { get; }

        public abstract string TokenName { get; }
    }
}
