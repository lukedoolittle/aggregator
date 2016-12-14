using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;
using Material.OAuth.Workflow;

namespace Material.Infrastructure.Identities
{
    public class TwitterIdentity : IOAuth1Identity
    {
        public async Task<JsonWebToken> AppendIdentity(
            JsonWebToken token, 
            OAuth1Credentials credentials)
        {
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterVerifyCredentials, TwitterVerifyCredentialsResponse>()
                .ConfigureAwait(false);

            token.Claims.Subject = response.Id.ToString();

            return token;
        }
    }
}
