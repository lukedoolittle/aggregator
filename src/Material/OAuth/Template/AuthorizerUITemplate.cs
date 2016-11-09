using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth.Template
{
    public abstract class AuthorizerUITemplate<TCredentials> :
        IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthCallbackHandler<TCredentials> _handler;
        private readonly Uri _callbackUri;
        private readonly Action<Action> _runOnMainThread;

        public AuthorizationInterface BrowserType { get; }

        protected AuthorizerUITemplate(
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri,
            AuthorizationInterface browserType,
            Action<Action> runOnMainThread)
        {
            _handler = handler;
            _callbackUri = callbackUri;
            _runOnMainThread = runOnMainThread;
            BrowserType = browserType;
        }

        protected void RespondToUri(
            Uri uri, 
            string userId,
            TaskCompletionSource<TCredentials> completionSource,
            Action success)
        {
            if (completionSource == null) throw new ArgumentNullException(nameof(completionSource));

            try
            {
                var result = _handler
                    .ParseAndValidateCallback(
                        uri,
                        userId);
                if (result != null)
                {
                    completionSource.SetResult(result);
                    success?.Invoke();
                }
            }
            catch (Exception ex)
            {
                completionSource.SetException(ex);
                throw;
            }
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

            _runOnMainThread(async () =>
            {
                await CreateViews().ConfigureAwait(false);

                MakeAuthorizationRequest(uri =>
                {
                    if (uri.ToString().StartsWith(callbackUri.ToString()))
                    {
                        try
                        {
                            var result = _handler
                                .ParseAndValidateCallback(
                                    uri,
                                    userId);

                            if (result != null)
                            {
                                taskCompletion.SetResult(result);
                                DismissViews();
                            }
                        }
                        catch (Exception ex)
                        {
                            taskCompletion.SetException(ex);
                            throw;
                        }
                    }
                });
            });

            return await taskCompletion
                .Task
                .ConfigureAwait(false);
        }

        protected virtual void MakeAuthorizationRequest(
            Action<Uri> callbackHandler)
        {
            throw new NotImplementedException();
        }

        protected virtual Task CreateViews()
        {
            throw new NotImplementedException();
        }

        protected virtual void DismissViews()
        {
            throw new NotImplementedException();
        }
    }
}
