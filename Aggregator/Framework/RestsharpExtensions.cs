using System.Net;
using BatmansBelt.Extensions;
using BatmansBelt.Serialization;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Serialization;
using RestSharp;
using RestSharp.Extensions.MonoHttp;

namespace Aggregator.Framework.Extensions
{
    public static class RestsharpExtensions
    {
        public const string OAUTH_TOKEN = "oauth_token";
        public const string REDIRECT_URI = "redirect_uri";
        public const string CLIENT_ID = "client_id";
        public const string CLIENT_SECRET = "client_secret";
        public const string SCOPE = "scope";
        public const string GRANT_TYPE = "grant_type";
        public const string RESPONSE_TYPE = "response_type";
        public const string STATE = "state";
        public const string BEARER_HEADER = "Bearer";
        public const string BASIC_HEADER = "Basic";

        public static void AddCodeParamter(
            this IRestRequest instance, 
            string code)
        {
            instance.AddParameter(
                ResponseTypeEnum.Code.EnumToString(), 
                code);
        }

        public static void AddRedirectUriParameter(
            this IRestRequest instance,
            string redirectUri)
        {
            instance.AddParameter(
                REDIRECT_URI,
                redirectUri);
        }

        public static void AddClientIdParameter(
            this IRestRequest instance,
            string clientId)
        {
            instance.AddParameter(
                CLIENT_ID,
                clientId);
        }

        public static void AddClientSecretParameter(
            this IRestRequest instance,
            string clientSecret)
        {
            instance.AddParameter(
                CLIENT_SECRET,
                clientSecret);
        }

        public static void AddScopeParameter(
            this IRestRequest instance,
            string scope)
        {
            instance.AddParameter(
                SCOPE,
                scope);
        }

        public static void AddStateParameter(
            this IRestRequest instance,
            string state)
        {
            instance.AddParameter(
                STATE,
                state);
        }

        public static void AddResponseTypeParameter(
            this IRestRequest instance,
            ResponseTypeEnum responseType)
        {
            instance.AddParameter(
                RESPONSE_TYPE,
                responseType.EnumToString());
        }

        public static void AddGrantTypeParameter(
            this IRestRequest instance,
            GrantTypeEnum grantType)
        {
            instance.AddParameter(
                GRANT_TYPE,
                grantType.EnumToString());
        }

        public static void AddRefreshTokenParameter(
            this IRestRequest instance,
            string refreshToken)
        {
            instance.AddParameter(
                GrantTypeEnum.RefreshToken.EnumToString(),
                refreshToken);
        }

        public static void AddBearerHeader(
            this IRestRequest instance,
            string bearerToken)
        {
            instance.AddHeader(
                HttpRequestHeader.Authorization.ToString(),
                $"{BEARER_HEADER} {bearerToken}");
        }

        public static void AddBasicAuthorizationHeader(
            this IRestRequest instance,
            string clientId,
            string clientSecret)
        {
            var key = $"{clientId}:{clientSecret}".ToBase64String();
            instance.AddHeader(
                HttpRequestHeader.Authorization.ToString(),
                $"{BASIC_HEADER} {key}");
        }

        public static void AddOAuthTokenParameter(
            this IRestRequest instance,
            string oauthToken)
        {
            instance.AddParameter(
                OAUTH_TOKEN,
                oauthToken);
        }

        public static TToken ParseToken<TToken>(
            this IRestResponse instance)
        {
            if (instance.ContentType.Contains(MimeTypeEnum.Json.EnumToString()))
            {
                return instance.Content.AsEntity<TToken>(false);
            }
            else
            {
                return HttpUtility.ParseQueryString(instance.Content)
                    .AsEntity<TToken>();
            }
        }
    }
}
