using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Aggregator.Framework;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Exceptions;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Http;

namespace Aggregator.Infrastructure
{
    public class BrowserWebAuthorizer : WebAuthorizerBase, IWebAuthorizer
    {
        private readonly HttpServer _httpServer;

        public AuthenticationInterfaceEnum BrowserType { get; }

        public BrowserWebAuthorizer(HttpServer httpServer)
        {
            _httpServer = httpServer;
            BrowserType = AuthenticationInterfaceEnum.Dedicated;
        }

        public Task<TToken> Authorize<TToken>(
            Uri callbackUri, 
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            var taskCompletion = new TaskCompletionSource<TToken>();
            //TODO: fix magic strings

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _httpServer.CreateServer(
                (request, response) =>
                {
                    HandleResponse(taskCompletion, request.Uri);
                    response.WriteHtml("Infrastructure/fragment.html");
                })
                .Listen(callbackUri);
#pragma warning restore 4014

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException();
            }

            Process.Start(authorizationUri.ToString());

            taskCompletion.Task.ContinueWith(t => _httpServer.Close());

            return taskCompletion.Task;
        }
    }
}
