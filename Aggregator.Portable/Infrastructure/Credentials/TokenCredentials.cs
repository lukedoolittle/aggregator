using System.Collections.Generic;

namespace Aggregator.Infrastructure.Credentials
{
    public abstract class TokenCredentials
    {
        public abstract bool HasValidProperties { get; }
        public abstract bool IsTokenExpired { get; }
        public Dictionary<string, string> AdditionalTokenParameters { get; }

        protected TokenCredentials()
        {
            AdditionalTokenParameters = new Dictionary<string, string>();
        }
    }
}
