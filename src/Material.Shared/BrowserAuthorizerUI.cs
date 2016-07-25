using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Foundations.Http;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.OAuth;

namespace Material.Infrastructure
{
    public class BrowserAuthorizerUI : IOAuthAuthorizerUI
    {
        private readonly HttpServer _httpServer;
        private readonly OAuthCallbackHandler _handler;

        public AuthenticationInterfaceEnum BrowserType =>
            AuthenticationInterfaceEnum.Dedicated;

        public BrowserAuthorizerUI(
            HttpServer httpServer,
            OAuthCallbackHandler handler)
        {
            _httpServer = httpServer;
            _handler = handler;
        }

        public Task<TToken> Authorize<TToken>(
            Uri callbackUri, 
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            var taskCompletion = new TaskCompletionSource<TToken>();
            //TODO: fix magic strings

#pragma warning disable 4014
            _httpServer.CreateServer(
                (request, response) =>
                {
                    //TODO: magic strings
                    response.WriteHtml("fragment.html");
                    var result = _handler
                        .ParseAndValidateCallback<TToken>(
                            request.Uri);
                    if (result != null)
                    {
                        taskCompletion.SetResult(result);
                    }
                })
                .Listen(callbackUri);
#pragma warning restore 4014


            if (!Platform.IsOnline)
            {
                throw new ConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            Process.Start(authorizationUri.ToString());

            taskCompletion.Task.ContinueWith(t => _httpServer.Close());

            return taskCompletion.Task;
        }
    }
}
