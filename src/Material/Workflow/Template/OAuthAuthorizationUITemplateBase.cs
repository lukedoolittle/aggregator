﻿using System;
using Material.Domain.Credentials;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Exceptions;
using Material.Framework.Extensions;

namespace Material.Workflow.Template
{
    public abstract class OAuthAuthorizationUITemplateBase<TCredentials, TView> :
        IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthCallbackHandler<TCredentials> _handler;
        private readonly Uri _callbackUri;
        private readonly Action<Action> _runOnMainThread;
        private readonly Func<bool> _isOnline;

        public AuthorizationInterface BrowserType { get; }

        protected OAuthAuthorizationUITemplateBase(
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri,
            AuthorizationInterface browserType,
            Action<Action> runOnMainThread,
            Func<bool> isOnline)
        {
            _handler = handler;
            _callbackUri = callbackUri;
            _runOnMainThread = runOnMainThread ?? (action => { action(); });
            _isOnline = isOnline;
            BrowserType = browserType;
        }

        public virtual Task<TCredentials> Authorize(
            Uri authorizationUri, 
            string requestId)
        {
            var credentialsCompletion = new TaskCompletionSource<TCredentials>();

            _runOnMainThread(() =>
            {
                if (!_isOnline())
                {
                    throw new NoConnectivityException(
                        StringResources.OfflineConnectivityException);
                }

                MakeAuthorizationRequest(
                    authorizationUri,
                    credentialsCompletion,
                    (uri, view) =>
                    {
                        if (uri.IsSubpathOf(_callbackUri))
                        {
                            try
                            {
                                var result = _handler
                                    .ParseAndValidateCallback(
                                        uri);

                                if (!result.ContainsResult)
                                {
                                    return false;
                                }
                                credentialsCompletion.SetResult(
                                    result.Credentials);
                                CleanupView(view);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                credentialsCompletion.SetException(ex);
                                throw;
                            }
                        }

                        return false;
                    });
            });

            return credentialsCompletion.Task;
        }

        protected abstract void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, TView, bool> callbackHandler);

        protected abstract void CleanupView(TView view);
    }
}
