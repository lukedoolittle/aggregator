using System;
using System.Threading.Tasks;

namespace Foundations.Http
{
    public class HttpServer : IDisposable
    {

        public HttpServer()
        {
        }

        public HttpServer CreateServer(
            Action<IncommingMessage, ServerResponse> requestListener)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public async Task Listen(Uri address)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
