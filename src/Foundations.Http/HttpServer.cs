using System;
using System.Net;
using System.Threading.Tasks;

namespace Foundations.Http
{
    public class ServerExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }

    public delegate void ServerExceptionEventHandler(
        object sender, ServerExceptionEventArgs 
        eventArgs);

    public class HttpServer : IDisposable
    {
        private readonly HttpListener _listener;

        private bool _isRunning;
        private Action<IncommingMessage, ServerResponse> _requestListener;

        public event ServerExceptionEventHandler ServerException;

        public HttpServer()
        {
            _listener = new HttpListener();
        }

        public HttpServer CreateServer(
            Action<IncommingMessage, ServerResponse> requestListener)
        {
            _requestListener = requestListener;

            return this;
        }

        public void Close()
        {
            _isRunning = false;
            _listener.Prefixes.Clear();
            _listener.Stop();
        }

        public async Task Listen(Uri address)
        {
            //_listener.Prefixes.Add($"{address.Scheme}://{address.Authority}/");
            _listener.Prefixes.Add($"http://{address.Authority}/");

            _listener.Start();

            _isRunning = true;

            try
            {
                while (true)
                {
                    var context = await _listener.GetContextAsync().ConfigureAwait(false);
#pragma warning disable 4014
                    Task.Run(()=>ProcessRequest(context));
#pragma warning restore 4014
                }
            }
            catch (HttpListenerException)
            {
                if (_isRunning)
                {
                    throw;
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                var request = new IncommingMessage(context.Request);
                var response = new ServerResponse(context.Response);

                if (request.Url == @"/favicon.ico")
                {
                    response.WriteHead((int)HttpStatusCode.OK);
                    response.WriteHead(new HeaderPair("Content-Type", "image/x-icon"));
                    response.End();
                }
                else
                {
                    _requestListener(request, response);
                }
            }
            catch (Exception e)
            {
                OnServerException(new ServerExceptionEventArgs {Exception = e});
            }
        }

        protected virtual void OnServerException(ServerExceptionEventArgs e)
        {
            ServerException?.Invoke(this, e);
        }

        void IDisposable.Dispose()
        {
            (_listener as IDisposable).Dispose();
        }
    }
}
