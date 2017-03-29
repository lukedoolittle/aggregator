using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
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
