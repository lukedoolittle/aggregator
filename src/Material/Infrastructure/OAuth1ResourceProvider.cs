using System;
using Foundations.HttpClient.Enums;

namespace Material.Infrastructure
{
    public abstract class OAuth1ResourceProvider : ResourceProvider
    {
        public abstract Uri RequestUrl { get; }
        public abstract Uri AuthorizationUrl { get; }
        public abstract Uri TokenUrl { get; }
        public virtual OAuthParameterTypeEnum ParameterType => OAuthParameterTypeEnum.Header;
    }
}
