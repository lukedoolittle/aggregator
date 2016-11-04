namespace Quantfabric.Test.Material.OAuthServer
{
    public class OAuth1Token
    {
        public string OAuthToken { get; private set; }
        public string OAuthSecret { get; private set; }

        public OAuth1Token(string oauthToken, string oauthSecret)
        {
            OAuthToken = oauthToken;
            OAuthSecret = oauthSecret;
        }

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
