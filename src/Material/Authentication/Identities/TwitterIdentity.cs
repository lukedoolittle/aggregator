using System.Threading.Tasks;
using Material.Application;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Domain.Responses;
using Material.Domain.Requests;

namespace Material.Authentication.Identities
{
    public class TwitterIdentity : IOAuthIdentity<OAuth1Credentials>
    {
        public async Task<JsonWebToken> AppendIdentity(
            JsonWebToken token, 
            OAuth1Credentials credentials)
        {
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<TwitterVerifyCredentials, TwitterVerifyCredentialsResponse>()
                .ConfigureAwait(false);

            token.Claims.Subject = response.Id.ToString();

            return token;
        }
    }
}
