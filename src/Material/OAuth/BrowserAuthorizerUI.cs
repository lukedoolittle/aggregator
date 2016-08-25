using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure
{
    public class BrowserAuthorizerUI : IOAuthAuthorizerUI
    {
        private readonly IBrowser _browser;
        private readonly IOAuthCallbackListener _listener;

        public AuthenticationInterfaceEnum BrowserType =>
            AuthenticationInterfaceEnum.Dedicated;

        public BrowserAuthorizerUI(
            IOAuthCallbackListener listener,
            IBrowser browser)
        {
            _listener = listener;
            _browser = browser;
        }

        public Task<TToken> Authorize<TToken>(
            Uri callbackUri, 
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            var taskCompletion = new TaskCompletionSource<TToken>();

            _listener.Listen(callbackUri, taskCompletion);

            _browser.Launch(authorizationUri);

            return taskCompletion.Task;
        }
    }
}
