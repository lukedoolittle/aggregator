using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Template;

namespace Material.Infrastructure.OAuth
{
    public class ProtocolAuthorizerUI<TCredentials> :
        AuthorizerUITemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly Uri _callbackUri;

        public ProtocolAuthorizerUI(
            IOAuthCallbackHandler<TCredentials> callbackHandler,
            Uri callbackUri,
            AuthorizationInterface @interface,
            Action<Action> runOnMainThread) : 
                base(
                    callbackHandler, 
                    callbackUri,
                    @interface, 
                    runOnMainThread)
        {
            _callbackUri = callbackUri;
        }

        //TODO: fix this to properly inherit from AuthorizerUITemplate
        public override Task<TCredentials> Authorize(
            Uri authorizationUri,
            string userId)
        {
            var completionSource = new TaskCompletionSource<TCredentials>();

            Platform.Current.ProtocolLaunch += (s, e) =>
            {
                if (e.Uri.ToString().Contains(_callbackUri.ToString()))
                {
                    RespondToUri(
                        e.Uri,
                        userId,
                        completionSource,
                        () => { });
                }
            };

            Platform.Current.Launch(authorizationUri);

            return completionSource.Task;
        }
    }
}
