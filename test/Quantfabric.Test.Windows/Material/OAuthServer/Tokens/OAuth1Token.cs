namespace Quantfabric.Test.Material.OAuthServer.Tokens
{
    public class OAuth1Token
    {
        public string OAuthToken { get; private set; }
        public string OAuthSecret { get; private set; }
        public string Verifier { get; private set; }
        public bool AreTemporaryCredentials { get; private set; }

        public OAuth1Token(
            string oauthToken, 
            string oauthSecret, 
            bool areTemporaryCredentials)
        {
            OAuthToken = oauthToken;
            OAuthSecret = oauthSecret;
            AreTemporaryCredentials = areTemporaryCredentials;
        }

        public void SetVerifier(string verifier)
        {
            Verifier = verifier;
        }
    }
}
