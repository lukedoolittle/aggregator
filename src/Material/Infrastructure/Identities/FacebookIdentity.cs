using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;
using Material.OAuth.Workflow;

namespace Material.Infrastructure.Identities
{
    public class FacebookIdentity : IOAuth2Identity
    {
        public async Task<JsonWebToken> AppendIdentity(
            JsonWebToken token, 
            OAuth2Credentials credentials)
        {
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
                .ConfigureAwait(false);

            token.Claims.Subject = response.Id;

            return token;
        }
    }
}
