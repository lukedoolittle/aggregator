using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Authentication;
using Aggregator.Infrastructure.Credentials;

namespace Aggregator.Task
{
    public class OAuthFactory : IOAuthFactory
    {
        public IOAuth GetOAuth(OAuth2Credentials credentials)
        {
            return new OAuth(
                credentials.AccessToken,
                credentials.TokenName,
                credentials.AdditionalTokenParameters);
        }

        public IOAuth GetOAuth(OAuth1Credentials credentials)
        {
            return new OAuth(
                credentials.ConsumerKey,
                credentials.ConsumerSecret,
                credentials.OAuthToken,
                credentials.OAuthSecret,
                credentials.ParameterHandling,
                credentials.AdditionalTokenParameters);
        }

        public IOAuth1 GetOAuth1()
        {
            return new OAuth1();
        }

        public IOAuth2 GetOAuth2()
        {
            return new OAuth2();
        }
    }
}
