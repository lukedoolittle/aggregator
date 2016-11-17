using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{
    public abstract class OAuthAuthorizationUITemplateBase<TCredentials> :
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
            _runOnMainThread = runOnMainThread;
            _isOnline = isOnline;
            BrowserType = browserType;
        }

        public virtual Task<TCredentials> Authorize(
            Uri authorizationUri, 
            string userId)
        {
            return Authorize(
                authorizationUri, 
                _callbackUri, 
                userId);
        }

        protected async Task<TCredentials> Authorize(
            Uri authorizationUri,
            Uri callbackUri,
            string userId)
        {
            var taskCompletion = new TaskCompletionSource<TCredentials>();

            var action = new Action(async () =>
            {
                if (!_isOnline())
                {
                    throw new NoConnectivityException(
                        StringResources.OfflineConnectivityException);
                }

                await MakeAuthorizationRequest(
                    authorizationUri,
                    (uri, view) =>
                    {
                        if (uri != null &&
                            uri.AbsolutePath.ToString().StartsWith(
                                callbackUri.AbsolutePath.ToString()))
                        {
                            try
                            {
                                var result = _handler
                                    .ParseAndValidateCallback(
                                        uri,
                                        userId);

                                if (result == null)
                                {
                                    return false;
                                }
                                taskCompletion.SetResult(result);
                                CleanupView(view);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                taskCompletion.SetException(ex);
                                throw;
                            }
                        }

                        return false;
                    }).ConfigureAwait(false);
            });

            if (_runOnMainThread != null)
            {
                _runOnMainThread(action);
            }
            else
            {
                action();
            }

            return await taskCompletion
                .Task
                .ConfigureAwait(false);
        }

        //is there some way to make the object in the callback handler typesafe?
        protected abstract Task MakeAuthorizationRequest(
            Uri authorizationUri,
            Func<Uri, object, bool> callbackHandler);

        protected abstract void CleanupView(object view);
    }
}
