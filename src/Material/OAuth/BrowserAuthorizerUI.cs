﻿using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure
{
    public class BrowserAuthorizerUI<TCredentials> : 
        IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly Uri _callbackUri;
        private readonly IBrowser _browser;
        private readonly IOAuthCallbackListener<TCredentials> _listener;

        public AuthenticationInterfaceEnum BrowserType =>
            AuthenticationInterfaceEnum.Dedicated;

        public BrowserAuthorizerUI(
            IOAuthCallbackListener<TCredentials> listener,
            IBrowser browser, Uri callbackUri)
        {
            _listener = listener;
            _browser = browser;
            _callbackUri = callbackUri;
        }

        public Task<TCredentials> Authorize(
            Uri authorizationUri,
            string userId)
        {
            var taskCompletion = new TaskCompletionSource<TCredentials>();

            _listener.Listen(
                _callbackUri, 
                userId,
                taskCompletion);

            _browser.Launch(authorizationUri);

            return taskCompletion.Task;
        }
    }
}
