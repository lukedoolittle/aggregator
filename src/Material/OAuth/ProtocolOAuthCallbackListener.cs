using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.OAuth
{
    public class ProtocolOAuthCallbackListener<TCredentials> :
        AuthorizerUITemplate<TCredentials>,
        IOAuthCallbackListener<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IProtocolLauncher _launcher;

        public ProtocolOAuthCallbackListener(
            IOAuthCallbackHandler<TCredentials> handler, 
            IProtocolLauncher launcher) : 
                base(handler)
        {
            _launcher = launcher;
        }

        public void Listen(
            Uri callbackUri, 
            string userId,
            TaskCompletionSource<TCredentials> completionSource) 
        {
            _launcher.ProtocolLaunch += (s, e) =>
            {
                if (e.Uri.ToString().Contains(callbackUri.ToString()))
                {
                    RespondToUri(
                        e.Uri,
                        userId,
                        completionSource,
                        () => { });
                }
            };
        }
    }
}
