﻿using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Domain.Core;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Canonicalizers;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Algorithms;
using Material.Workflow.AuthenticatorParameters;

namespace Material.Workflow.Facade
{
    public class OAuth1AuthorizationFacade : 
        IOAuthAuthorizationUriFacade, 
        IOAuthAccessTokenFacade<OAuth1Credentials>
    {
        private readonly OAuth1ResourceProvider _resourceProvider;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly IOAuthAuthorizationAdapter _oauth;
        private readonly Uri _callbackUri;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly ISigningAlgorithm _signingAlgorithm;
        private readonly ICryptoStringGenerator _stringGenerator;
        private readonly IHttpRequestCanonicalizer _canonicalizer;

        public OAuth1AuthorizationFacade(
            OAuth1ResourceProvider resourceProvider,
            string consumerKey,
            string consumerSecret,
            Uri callbackUri,
            IOAuthAuthorizationAdapter oauth, 
            IOAuthSecurityStrategy securityStrategy,
            ISigningAlgorithm signingAlgorithm,
            ICryptoStringGenerator stringGenerator, 
            IHttpRequestCanonicalizer canonicalizer)
        {
            if (resourceProvider == null) throw new ArgumentNullException(nameof(resourceProvider));
            if (consumerKey == null) throw new ArgumentNullException(nameof(consumerKey));
            if (consumerSecret == null) throw new ArgumentNullException(nameof(consumerSecret));
            if (callbackUri == null) throw new ArgumentNullException(nameof(callbackUri));
            if (oauth == null) throw new ArgumentNullException(nameof(oauth));
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (signingAlgorithm == null) throw new ArgumentNullException(nameof(signingAlgorithm));
            if (stringGenerator == null) throw new ArgumentNullException(nameof(stringGenerator));
            if (canonicalizer == null) throw new ArgumentNullException(nameof(canonicalizer));

            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _resourceProvider = resourceProvider;
            _oauth = oauth;
            _securityStrategy = securityStrategy;
            _callbackUri = callbackUri;
            _signingAlgorithm = signingAlgorithm;
            _stringGenerator = stringGenerator;
            _canonicalizer = canonicalizer;
        }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <param name="requestId">Unique ID for request</param>
        /// <returns>Authorization uri</returns>
        public async Task<Uri> GetAuthorizationUriAsync(string requestId)
        {
            var builder = CreateBuilder()
                .AddParameter(new OAuth1Callback(
                    _callbackUri, 
                    requestId))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    _consumerSecret,
                    _signingAlgorithm,
                    _canonicalizer));

            var credentials = (await _oauth
                .GetToken<OAuth1Credentials>(
                    _resourceProvider.RequestUrl,
                    builder,
                    new Dictionary<HttpRequestHeader, string>(), 
                    _resourceProvider.ParameterType)
                .ConfigureAwait(false))
                .SetConsumerProperties(
                    _consumerKey, 
                    _consumerSecret);

            if (!credentials.CallbackConfirmed)
            {
                //Warning this violates the spec https://tools.ietf.org/html/rfc5849
            }

            _securityStrategy.SetSecureParameter(
                requestId,
                OAuth1Parameter.OAuthToken.EnumToString(),
                credentials.OAuthToken);
            _securityStrategy.SetSecureParameter(
                requestId,
                OAuth1Parameter.OAuthTokenSecret.EnumToString(),
                credentials.OAuthSecret);

            var authorizationPath =
                _oauth.GetAuthorizationUri(
                    _resourceProvider.AuthorizationUrl,
                    new AuthenticatorBuilder().AddParameter(
                        new OAuth1Token(
                            _securityStrategy, 
                            requestId)));

            return authorizationPath;
        }

        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="intermediateResult">Intermediate credentials received from OAuth1 callback</param>
        /// <param name="requestId">Unique ID for request</param>
        /// <returns>Access token credentials</returns>
        public async Task<OAuth1Credentials> GetAccessTokenAsync(
            OAuth1Credentials intermediateResult,
            string requestId)
        {
            if (intermediateResult == null) throw new ArgumentNullException(nameof(intermediateResult));

            if (intermediateResult.IsErrorResult)
            {
                return intermediateResult;
            }

            var oauthSecret = _securityStrategy.GetSecureParameter(
                requestId,
                OAuth1Parameter.OAuthTokenSecret.EnumToString());

            var builder = CreateBuilder()
                .AddParameter(new OAuth1Verifier(
                    intermediateResult.Verifier))
                .AddParameter(new OAuth1Token(
                    _securityStrategy,
                    requestId))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    _consumerSecret,
                    oauthSecret,
                    _signingAlgorithm,
                    _canonicalizer));

            return (await _oauth
                .GetToken<OAuth1Credentials>(
                    _resourceProvider.TokenUrl,
                    builder,
                    new Dictionary<HttpRequestHeader, string>(), 
                    _resourceProvider.ParameterType)
                .ConfigureAwait(false))
                .SetConsumerProperties(
                    _consumerKey,
                    _consumerSecret)
                .SetParameterHandling(
                    _resourceProvider.ParameterType);
        }

        private AuthenticatorBuilder CreateBuilder()
        {
            return new AuthenticatorBuilder()
                .AddParameter(new OAuth1ConsumerKey(_consumerKey))
                .AddParameter(new OAuth1Timestamp())
                .AddParameter(new OAuth1Nonce(_stringGenerator))
                .AddParameter(new OAuth1Version())
                .AddParameter(new OAuth1SignatureMethod(_signingAlgorithm));
        }
    }
}
