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
    public class OAuth1AuthenticationTask<TService> : 
        AuthenticationTaskBase<OAuth1Credentials, TService>
        where TService : Service
    {
        private OAuth1Credentials _credentials;
        private readonly OAuth1Service _service;
        private readonly IOAuth1 _oauth;
        protected override ResponseTypeEnum _responseType { get; }

        public OAuth1AuthenticationTask(
            OAuth1Service service,
            OAuth1Credentials credentials,
            IOAuth1 oauth,
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

            _responseType = ResponseTypeEnum.Code;
        }

        protected override async Task<Uri> GetAuthorizationPath()
        {
            _credentials =
                await _oauth.GetRequestToken(
                    _service.RequestUrl,
                    _credentials.ConsumerKey,
                    _credentials.ConsumerSecret,
                    _service.CallbackUrl)
                .ConfigureAwait(false);

            var authorizationPath =
                _oauth.GetAuthorizationPath(
                    _credentials.OAuthToken,
                    _service.AuthorizeUrl);

            return authorizationPath;
        }

        protected override async Task<OAuth1Credentials> GetAccessTokenFromResult(OAuth1Credentials result)
        {
            var token = await _oauth.GetAccessToken(
                _service.AccessUrl,
                _credentials.ConsumerKey,
                _credentials.ConsumerSecret,
                _credentials.OAuthToken,
                _credentials.OAuthSecret,
                result.Verifier,
                result.AdditionalTokenParameters);

            return token
                .SetConsumerProperties(
                    _credentials.ConsumerKey,
                    _credentials.ConsumerSecret)
                .MergeAdditionalParameters(
                    result.AdditionalTokenParameters);
        }
    }
}
