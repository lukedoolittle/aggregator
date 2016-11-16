using System;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace CodeGen
{
    public class DummyOAuth2ResourceProvider : OAuth2ResourceProvider
    {
        public override Uri TokenUrl { get; }
        public override List<string> AvailableScopes { get; }
        public override List<OAuth2FlowType> Flows { get; }
        public override List<GrantType> GrantTypes { get; }
    }

    public class DummyOAuth1ResourceProvider : OAuth1ResourceProvider
    {
        public override Uri RequestUrl { get; }
        public override Uri AuthorizationUrl { get; }
        public override Uri TokenUrl { get; }
        public override HttpParameterType ParameterType { get; }
    }
}
