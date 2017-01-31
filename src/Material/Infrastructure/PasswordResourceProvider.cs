using System;

namespace Material.Infrastructure
{
    public abstract class PasswordResourceProvider : ResourceProvider
    {
        public abstract Uri TokenUrl { get; }

        public abstract string UsernameKey { get; }

        public abstract string PasswordKey { get; }
    }
}
