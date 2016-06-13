using System;
using System.Threading.Tasks;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Credentials;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Authentication
{
    public class OAuth2AuthenticationTask<TService> : 
        AuthenticationTaskBase<OAuth2Credentials, TService>
        where TService : Service
    {
        private readonly OAuth2Credentials _credentials;
        private readonly OAuth2Service _service;
        private readonly IOAuth2 _oauth;
        protected override ResponseTypeEnum _responseType { get; }

        public OAuth2AuthenticationTask(
            OAuth2Service service,
            OAuth2Credentials credentials,
            IOAuth2 oauth,
            IWebAuthorizer authorizer,
            ICommandSender sender,
            Guid aggregateId,
            int originalVersion,
            bool update = false) : 
                base(
                aggregateId, 
                originalVersion, 
                sender,
                service.CallbackUrl, 
                authorizer,
                update)
        {
            _credentials = credentials;
            _service = service;
            _oauth = oauth;

            _responseType = string.IsNullOrEmpty(credentials.ClientSecret) ? 
                ResponseTypeEnum.Token : 
                ResponseTypeEnum.Code;
        }

        protected override Task<Uri> GetAuthorizationPath()
        {
            var authorizationPath =
                _oauth.GetAuthorizationPath(
                    _service.AuthorizeUrl,
                    _credentials.ClientId,
                    _service.Scope,
                    _service.CallbackUrl,
                    Guid.NewGuid().ToString(),
                    _responseType,
                    _service.Parameters);

            return System.Threading.Tasks.Task.FromResult(authorizationPath);
        }

        protected override async Task<OAuth2Credentials> GetAccessTokenFromResult(
            OAuth2Credentials result)
        {
            var accessToken = await _oauth.GetAccessToken(
                _service.AccessUrl,
                _credentials.ClientId,
                _credentials.ClientSecret,
                _service.CallbackUrl,
                result.Code,
                _service.Scope,
                _service.HasBasicAuthorization)
                .ConfigureAwait(false);

            return accessToken
                .SetTokenName(_service.TokenName)
                .SetClientProperties(
                    _credentials.ClientId, 
                    _credentials.ClientSecret);
        }
    }
}
