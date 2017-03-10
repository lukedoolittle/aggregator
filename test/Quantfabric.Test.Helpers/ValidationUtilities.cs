using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.Integration
{
    public static class ValidationUtilities
    {
        public static bool IsValidOAuth1Token(
            OAuth1Credentials token,
            bool shouldContainUserId)
        {
            if (shouldContainUserId && string.IsNullOrEmpty(token?.ExternalUserId))
            {
                return false;
            }
            return !string.IsNullOrEmpty(token?.ConsumerKey) &&
                   !string.IsNullOrEmpty(token?.ConsumerSecret) &&
                   !string.IsNullOrEmpty(token?.OAuthToken) &&
                   !string.IsNullOrEmpty(token?.OAuthSecret);
        }

        public static bool IsValidOAuth2Token(OAuth2Credentials token)
        {
            return !string.IsNullOrEmpty(token?.AccessToken) &&
                   !string.IsNullOrEmpty(token.TokenName);
        }

        public static bool IsValidJsonWebToken(JsonWebToken token)
        {
            return token.Header != null &&
                   token.Claims != null &&
                   token.Signature != null &&
                   !string.IsNullOrEmpty(token.Claims.Audience) &&
                   !string.IsNullOrEmpty(token.Claims.Issuer) &&
                   !string.IsNullOrEmpty(token.Claims.Subject);
        }
    }
}
