using System;
using Foundations.HttpClient.Enums;

namespace Material.Infrastructure
{
    //TODO: should override GetHashCode() for this value object
    public abstract class OAuth1ResourceProvider : ResourceProvider
    {
        public abstract Uri RequestUrl { get; }
        public abstract Uri AuthorizationUrl { get; }
        public abstract Uri TokenUrl { get; }
        public abstract HttpParameterType ParameterType { get; }
    }
}
