using Aggregator.Infrastructure.Credentials;

namespace Aggregator.Framework.Contracts
{
    public interface IOAuthFactory
    {
        IOAuth GetOAuth(OAuth2Credentials credentials);

        IOAuth GetOAuth(OAuth1Credentials credentials);

        IOAuth1 GetOAuth1();

        IOAuth2 GetOAuth2();
    }
}
