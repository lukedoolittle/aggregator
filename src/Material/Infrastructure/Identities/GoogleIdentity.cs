﻿using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;
using Material.OAuth.Workflow;

namespace Material.Infrastructure.Identities
{
    public class GoogleIdentity : IOAuth2Identity
    {
        public async Task<JsonWebToken> AppendIdentity(
            JsonWebToken token, 
            OAuth2Credentials credentials)
        {
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<GoogleProfile, GoogleProfileResponse>()
                .ConfigureAwait(false);

            token.Claims.Subject = response.Id;

            return token;
        }
    }
}
