using System;
using System.Collections.Generic;

namespace Material.Domain.Core
{
    public abstract class OpenIdResourceProvider : OAuth2ResourceProvider
    {
        public abstract Uri OpenIdDiscoveryUrl { get; }
        public virtual List<string> ValidIssuers { get; }

        public const string OpenIdScope = "openid";
    }
}
