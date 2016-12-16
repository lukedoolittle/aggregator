using System;
using System.Globalization;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1Timestamp : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Timestamp.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1Timestamp(int unixTimestamp)
        {
            Value = unixTimestamp.ToString(CultureInfo.InvariantCulture);
        }

        public OAuth1Timestamp(DateTime timestamp) : 
            this((int)timestamp.ToUnixTimeSeconds())
        { }

        public OAuth1Timestamp() :
            this(DateTime.UtcNow)
        { }
    }
}
