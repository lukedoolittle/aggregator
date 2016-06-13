using System;
using System.Collections.Generic;
using Aggregator.Domain.Write;

namespace Aggregator.Infrastructure
{
    public abstract class OAuth1Service : Service
    {
        public abstract Uri RequestUrl { get; }
        public abstract Uri CallbackUrl { get; }
        public abstract Uri AuthorizeUrl { get; }
        public abstract Uri AccessUrl { get; }
        public virtual Dictionary<string, string> Headers => new Dictionary<string, string>();
    }
}
