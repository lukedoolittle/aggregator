using System;
using System.Collections.Generic;
using Material.Contracts;
using Material.HttpClient.Cryptography.Enums;

namespace Material
{
    public static class QuantfabricConfiguration
    {
        /// <summary>
        /// Default timeout for all oauth security parameters (state, nonce, etc)
        /// </summary>
        public static TimeSpan SecurityParameterTimeout { get; set; } = 
            TimeSpan.FromMinutes(2);

        /// <summary>
        /// Length the issues authentication token is valid for
        /// </summary>
        public static TimeSpan AuthenticationTokenTimeout { get; set; } = 
            TimeSpan.FromHours(1);

        /// <summary>
        /// All the algorithms the current instance can support
        /// https://threatpost.com/critical-vulnerabilities-affect-json-web-token-libraries/111943/
        /// </summary>
        public static List<JsonWebTokenAlgorithm> WhitelistedAuthenticationAlgorithms { get; } =
            new List<JsonWebTokenAlgorithm>
            {
                JsonWebTokenAlgorithm.RS256,
                JsonWebTokenAlgorithm.ES256
            };

        public static IOAuthAuthorizerUIFactory WebAuthorizationUIFactory { get; set; }

        public static IAuthorizationUISelector WebAuthenticationUISelector { get; set; }

        public static IBluetoothAuthorizerUIFactory BluetoothAuthorizationUIFactory { get; set; }
    }
}
