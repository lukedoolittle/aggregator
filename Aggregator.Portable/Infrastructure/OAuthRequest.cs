using System;
using System.Collections.Generic;
using Aggregator.Domain.Write;

namespace Aggregator.Infrastructure
{
    public abstract class OAuthRequest : Request
    {
        protected OAuthRequest()
        {
            AdditionalQuerystringParameters = new Dictionary<string, string>();
            AdditionalUrlSegmentParameters = new Dictionary<string, string>();

            QuerystringParameters = AdditionalQuerystringParameters;
        }

        public abstract string Host { get; }
        public string Path => IsBulkRequest ? BulkPath : SinglePath;
        public abstract string SinglePath { get; }
        public virtual string BulkPath { get; }
        public abstract string HttpMethod { get; }
        public abstract string RequestFilterKey { get; }

        public virtual Dictionary<string, string> Headers { get; }
        public virtual Dictionary<string, string> AdditionalQuerystringParameters { get; }
        public Dictionary<string, string> QuerystringParameters { get; } 
        public virtual Dictionary<string, string> AdditionalUrlSegmentParameters { get; }

        public bool IsBulkRequest;
    }
}
