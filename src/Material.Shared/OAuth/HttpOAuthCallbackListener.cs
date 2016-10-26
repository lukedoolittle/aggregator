#if __WINDOWS__
using System;
using System.Threading.Tasks;
using Foundations.Http;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class HttpOAuthCallbackListener<TCredentials> : 
        IOAuthCallbackListener<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly HttpServer _httpServer;
        private readonly IOAuthCallbackHandler<TCredentials> _handler;

        public HttpOAuthCallbackListener(
            HttpServer httpServer, 
            IOAuthCallbackHandler<TCredentials> handler)
        {
            _httpServer = httpServer;
            _handler = handler;
        }

        public void Listen(
            Uri callbackUri,
            string userId,
            TaskCompletionSource<TCredentials> completionSource) 
        {
#pragma warning disable 4014
            _httpServer.CreateServer(
                (request, response) =>
                {
                    response.WriteHtmlString(FRAGMENT_HTML);
                    try
                    {
                        var result = _handler
                            .ParseAndValidateCallback(
                                request.Uri,
                                userId);
                        if (result != null)
                        {
                            completionSource.SetResult(result);
                        }
                    }
                    catch (Exception e)
                    {
                        completionSource.SetException(e);
                        throw;
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
				                if (window.location.hash)
				                {
					                var url = window.location;
                                    var fragmentQuerystring = url.hash.substr(1);
                                    var currentQuerystring = url.search.substr(1);
                                    var newUrl = url.pathname + '?' + fragmentQuerystring;
                                    if (currentQuerystring)
                                        newUrl = newUrl + '&' + currentQuerystring;
					
					                if(!parameterNullOrEmpty(""access_token"", newUrl) || 
					                   !parameterNullOrEmpty(""accessToken"", newUrl)) {
						                var xmlHttp = new XMLHttpRequest();
                                        xmlHttp.open('GET', newUrl, true);
						                xmlHttp.send(null);
					                }
                                }
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
#endif