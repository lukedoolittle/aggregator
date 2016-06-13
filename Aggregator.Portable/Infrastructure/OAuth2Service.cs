using System;
using System.Collections.Generic;
using Aggregator.Domain.Write;
using Aggregator.Framework.Enums;

namespace Aggregator.Infrastructure
{
    public abstract class OAuth2Service : Service
    {         
        public abstract Uri AuthorizeUrl { get; }
        public abstract Uri CallbackUrl { get; }
        public abstract Uri AccessUrl { get; }
        public abstract string Scope { get; }
        public abstract string TokenName { get; }
        public virtual bool HasBasicAuthorization => false;
        public virtual Dictionary<string, string> Parameters => new Dictionary<string, string>();
        public abstract ResponseTypeEnum[] AuthorizationGrants { get; }
    }
}
