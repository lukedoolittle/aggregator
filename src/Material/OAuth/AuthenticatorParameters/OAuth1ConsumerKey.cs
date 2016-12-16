using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
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
