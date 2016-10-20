using System;

namespace Quantfabric.Test.OAuthServer
{
    public class OAuth1Token
    {
        public string OAuthToken { get; private set; }
        public string OAuthSecret { get; private set; }

        public void SetToken(string token)
        {
            OAuthToken = token;
        }

        public void SetSecret(string secret)
        {
            OAuthSecret = secret;
        }
    }
}
