using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OAuthFactory : IOAuthFactory
    {
        public IOAuthProtectedResource GetOAuth(OAuth2Credentials credentials)
        {
            return new OAuthProtectedResource(
                credentials.AccessToken,
                credentials.TokenName);
        }

        public IOAuthProtectedResource GetOAuth(OAuth1Credentials credentials)
        {
            return new OAuthProtectedResource(
                credentials.ConsumerKey,
                credentials.ConsumerSecret,
                credentials.OAuthToken,
                credentials.OAuthSecret,
                credentials.ParameterHandling);
        }

        public IOAuth1Authentication GetOAuth1()
        {
            return new OAuth1Authentication();
        }

        public IOAuth2Authentication GetOAuth2()
        {
            return new OAuth2Authentication();
        }
    }
}
