using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.Http;
using Quantfabric.Test.Material.OAuth2Server;

namespace Quantfabric.Test.Integration
{
    public class OAuthTestingServer<TToken> : IDisposable
    {
        private readonly HttpServer _server = new HttpServer();
        private readonly IDictionary<string, IOAuthHandler> _handlers = 
            new Dictionary<string, IOAuthHandler>();

        public IDictionary<string, List<TToken>> Tokens { get; } = 
            new Dictionary<string, List<TToken>>();

        public OAuthTestingServer<TToken> AddApplicationId(string appId)
        {
            Tokens.Add(appId, new List<TToken>());

            return this;
        }

        public OAuthTestingServer<TToken> AddHandler(
            Uri targetUri,
            IOAuthHandler handler)
        {
            _handlers.Add(targetUri.ToString(), handler);

            return this;
        }

        #region Server

        public Task<Exception> Start(int port)
        {
#pragma warning disable 4014
            _server.CreateServer((message, response) =>
            {
                var uri = message.Uri.NoQuerystring();

                if (_handlers.ContainsKey(uri))
                {
                    _handlers[uri].HandleRequest(message, response);
                }
                else
                {
                    throw new Exception($"Request for {uri} was not handled");
                }
            }).Listen(new Uri($"http://localhost:{port}"));
#pragma warning restore 4014


            var taskCompletion = new TaskCompletionSource<Exception>();

            _server.ServerException += (sender, args) =>
            {
                taskCompletion.SetResult(args.Exception);
            };

            return taskCompletion.Task;
        }

        public void Stop()
        {
            _server.Close();
        }

        #endregion Server

        public void Dispose()
        {
            (_server as IDisposable).Dispose();
        }
    }
}
