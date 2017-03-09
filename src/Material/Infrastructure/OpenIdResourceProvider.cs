using System;
using System.Collections.Generic;

namespace Material.Infrastructure
{
    public abstract class OpenIdResourceProvider : OAuth2ResourceProvider
    {
        public abstract Uri OpenIdDiscoveryUrl { get; }
        public virtual List<string> ValidIssuers { get; }

        public const string OpenIdScope = "openid";
    }
}
