using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1Version : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Version.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1Version(string version)
        {
            if (version == null) throw new ArgumentNullException(nameof(version));

            Value = version;
        }

        public OAuth1Version() : this("1.0")
        { }
    }
}
