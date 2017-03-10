using System.Threading.Tasks;
using Material.Application;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
namespace Material.Infrastructure.Identities
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
