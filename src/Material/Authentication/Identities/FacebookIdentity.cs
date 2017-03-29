using System.Threading.Tasks;
using Material.Application;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Domain.Responses;
using Material.Domain.Requests;

namespace Material.Authentication.Identities
{
    public class FacebookIdentity : IOAuthIdentity<OAuth2Credentials>
    {
        public async Task<JsonWebToken> AppendIdentity(
            JsonWebToken token, 
            OAuth2Credentials credentials)
        {
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
                .ConfigureAwait(false);

            token.Claims.Subject = response.Id;

            return token;
        }
    }
}
