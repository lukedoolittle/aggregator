using System.Collections.Generic;
using Foundations.HttpClient.Cryptography.Enums;

namespace Material.OAuth.Authentication
{
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// All the algorithms the current instance can support
        /// https://threatpost.com/critical-vulnerabilities-affect-json-web-token-libraries/111943/
        /// </summary>
        public static List<JsonWebTokenAlgorithm> WhitelistedAlgorithms { get; } = 
            new List<JsonWebTokenAlgorithm>
            {
                JsonWebTokenAlgorithm.RS256
            };

        /// <summary>
        /// Length the issues authentication token is valid for
        /// </summary>
        public static int AuthenticationTokenTimeoutInMinutes { get; set; } = 60;
    }
}
