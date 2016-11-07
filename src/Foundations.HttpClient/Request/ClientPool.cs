using System;
using System.Collections.Generic;

namespace Foundations.HttpClient.Request
{
    public class ClientPool : IClientPool
    {
        private static readonly object _syncLock = new object();
        private readonly Dictionary<string, ClientItems> _clients = 
            new Dictionary<string, ClientItems>();

        //Per discussions http://byterot.blogspot.com/2016/07/singleton-httpclient-dns.html IDisposable does not need implementing
        //with HttpClient
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public ClientItems GetClient(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var key = uri.ToString();

            lock (_syncLock)
            {
                if (!_clients.ContainsKey(key))
                {
                    var handler = HttpConfiguration.MessageHandlerFactory();
                    var client = new System.Net.Http.HttpClient(handler);
                    _clients.Add(key, new ClientItems(client, handler));
                }

                return _clients[key];
            }
        }
    }
}
