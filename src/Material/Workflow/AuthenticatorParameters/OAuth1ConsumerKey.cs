using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth1ConsumerKey : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.ConsumerKey.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1ConsumerKey(string consumerKey)
        {
            if (consumerKey == null) throw new ArgumentNullException(nameof(consumerKey));

            Value = consumerKey;
        }
    }
}
