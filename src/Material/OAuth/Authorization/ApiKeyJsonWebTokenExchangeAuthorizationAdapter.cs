﻿using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Authorization
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