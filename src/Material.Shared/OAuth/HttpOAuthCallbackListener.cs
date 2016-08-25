using System;
using System.Threading.Tasks;
using Foundations.Http;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class HttpOAuthCallbackListener : IOAuthCallbackListener
    {
        private readonly HttpServer _httpServer;
        private readonly IOAuthCallbackHandler _handler;

        public HttpOAuthCallbackListener(
            HttpServer httpServer, 
            IOAuthCallbackHandler handler)
        {
            _httpServer = httpServer;
            _handler = handler;
        }

        public void Listen<TToken>(
            Uri callbackUri,
            TaskCompletionSource<TToken> completionSource) 
            where TToken : TokenCredentials
        {
#pragma warning disable 4014
            _httpServer.CreateServer(
                (request, response) =>
                {
                    response.WriteHtmlString(FRAGMENT_HTML);
                    try
                    {
                        var result = _handler
                            .ParseAndValidateCallback<TToken>(
                                request.Uri);
                        if (result != null)
                        {
                            completionSource.SetResult(result);
                        }
                    }
                    catch (Exception e)
                    {
                        completionSource.SetException(e);
                    }
                })
                .Listen(callbackUri);
#pragma warning restore 4014

            completionSource.Task.ContinueWith(t => _httpServer.Close());
        }

        private const string FRAGMENT_HTML =
        @"<html>
	                <head>
		                <script>
			                window.onload = function() {
				                if (window.location.hash && !window.location.search.slice(1))
				                {
					                var url = window.location;
                                    var newQuerystring = url.hash.substr(1);
                                    var newUrl = url.pathname + '?' + newQuerystring;
					
					                if(!parameterNullOrEmpty(""access_token"", newUrl) || 
					                   !parameterNullOrEmpty(""accessToken"", newUrl)) {
						                var xmlHttp = new XMLHttpRequest();
                                        xmlHttp.open('GET', newUrl, true);
						                xmlHttp.send(null);
					                }
                                }
                                window.close();
			                }
			
			                function parameterNullOrEmpty(name, url)
                                {
                                    name = name.replace(""/[\[\]]/g"", ""\\$&"");
                                    var regex = new RegExp(""[?&]"" + name + ""(=([^&#]*)|&|#|$)"");
                                    var results = regex.exec(url);
                                    if (!results) return true;
                                    if (!results[2]) return true;
                                    return false;
                                }
		                </script>
	                </head>
	                <body>Thanks for sharing!</body>
                </html>";
    }
}
