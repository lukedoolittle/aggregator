using System;
using Material.Domain.Credentials;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using Material.Authentication.Validation;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Extensions;
using Material.Workflow.AuthenticatorParameters;

namespace Material.Authorization
{
    public class ApiKeyJsonWebTokenExchangeAuthorizationAdapter
    {
        public async Task<OAuth2Credentials> GetAccessToken(
            Uri requestUri,
            string apiKeyName,
            string apiKeyValue,
            HttpParameterType apiKeyType,
            string tokenName,
            Uri discoveryUri)
        {
            var builder = new AuthenticatorBuilder()
                .AddParameter(new ApiKey(
                    apiKeyName, 
                    apiKeyValue,
                    apiKeyType));

            var result = (await new HttpRequestBuilder(requestUri.NonPath())
                .PostTo(requestUri.AbsolutePath)
                .Authenticator(builder)
                .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                .ResultAsync()
                .ConfigureAwait(false));

            var token = result.ToWebToken();

            //Right now there is no given endpoint for these tokens from Microsoft
            //so exclude the signature validation
            var tokenValidation = new CompositeJsonWebTokenAuthenticationValidator()
                .AddValidator(new JsonWebTokenExpirationValidator())
                .IsTokenValid(token);

            if (!tokenValidation.IsTokenValid)
            {
                throw new SecurityException(tokenValidation.Reason);
            }

            return new OAuth2Credentials()
                .SetAccessToken(result)
                .SetTokenName(tokenName)
                .SetExpiresIn(token.Claims.ExpirationTime);
        }
    }
}
