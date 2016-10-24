using System;
using System.Net;
using System.Threading.Tasks;

namespace Foundations.Http
{
    public class ServerExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }

    public class HttpServer : IDisposable
    {
        private readonly HttpListener _listener;

        private bool _isRunning;
        private Action<IncomingMessage, ServerResponse> _requestListener;

        public event EventHandler<ServerExceptionEventArgs> ServerException;

        public HttpServer()
        {
            _listener = new HttpListener();
        }

        public HttpServer CreateServer(
            Action<IncomingMessage, ServerResponse> requestListener)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                var request = new IncomingMessage(context.Request);
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

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~HttpServer()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free managed resources
                (_listener as IDisposable).Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }
    }
}
