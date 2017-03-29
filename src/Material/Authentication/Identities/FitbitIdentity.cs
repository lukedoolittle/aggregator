using System.Threading.Tasks;
using Material.Application;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Domain.Responses;
using Material.Domain.Requests;

namespace Material.Authentication.Identities
{
    public class FitbitIdentity : IOAuthIdentity<OAuth2Credentials>
    {
        public async Task<JsonWebToken> AppendIdentity(
            JsonWebToken token, 
            OAuth2Credentials credentials)
        {
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<FitbitProfile, FitbitProfileResponse>()
                .ConfigureAwait(false);

            token.Claims.Subject = response.User.EncodedId;

            return token;
        }
    }
}
