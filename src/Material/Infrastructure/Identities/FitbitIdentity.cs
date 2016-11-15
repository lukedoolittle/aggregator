using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;

namespace Material.Infrastructure.Identities
{
    public class FitbitIdentity : IOAuth2Identity
    {
        public async Task<JsonWebToken> AppendIdentity(
            JsonWebToken token, 
            OAuth2Credentials credentials)
        {
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitProfile, FitbitProfileResponse>()
                .ConfigureAwait(false);

            token.Claims.Subject = response.User.EncodedId;

            return token;
        }
    }
}
