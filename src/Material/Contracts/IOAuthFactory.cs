using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthFactory
    {
        IOAuthProtectedResource GetOAuth(OAuth2Credentials credentials);

        IOAuthProtectedResource GetOAuth(OAuth1Credentials credentials);

        IOAuth1Authentication GetOAuth1();

        IOAuth2Authentication GetOAuth2();
    }
}
