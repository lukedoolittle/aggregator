using System;
using System.Net;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Foundations.HttpClient.Request;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class ApiKeyJsonWebTokenExchangeAdapter
    {
        public async Task<OAuth2Credentials> GetAccessToken(
            Uri requestUri,
            string apiKeyName,
            string apiKeyValue,
            HttpParameterType apiKeyType,
            string tokenName)
        {
            using (var requestBuilder = new HttpRequestBuilder(requestUri.NonPath()))
            {
                var result = (await requestBuilder
                    .PostTo(requestUri.AbsolutePath)
                    .ForApiKey(
                        apiKeyName,
                        apiKeyValue,
                        apiKeyType)
                    .ThrowIfNotExpectedResponseCode(HttpStatusCode.OK)
                    .ResultAsync()
                    .ConfigureAwait(false));

                var token = JsonWebToken.FromString(result);

                //TODO: validate JWT with primary key

                return new OAuth2Credentials()
                    .SetAccessToken(result)
                    .SetTokenName(tokenName)
                    .SetExpiresIn(token.Claims.ExpirationTime);
            }
        }
    }
}
