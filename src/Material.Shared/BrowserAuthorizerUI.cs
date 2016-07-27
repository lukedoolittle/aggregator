using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Foundations.Http;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure
{
    public class BrowserAuthorizerUI : IOAuthAuthorizerUI
    {
        private readonly HttpServer _httpServer;
        private readonly IOAuthCallbackHandler _handler;

        public AuthenticationInterfaceEnum BrowserType =>
            AuthenticationInterfaceEnum.Dedicated;

        public BrowserAuthorizerUI(
            HttpServer httpServer,
            IOAuthCallbackHandler handler)
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
                    response.WriteHtmlString(FRAGMENT_HTML);
                    try
                    {
                        var result = _handler
                            .ParseAndValidateCallback<TToken>(
                                request.Uri);
                        taskCompletion.SetResult(result);
                    }
                    catch (Exception e)
                    {
                        taskCompletion.SetException(e);
                    }
                })
                .Listen(callbackUri);
#pragma warning restore 4014


            if (!Platform.IsOnline)
            {
                throw new NoConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            Process.Start(authorizationUri.ToString());

            taskCompletion.Task.ContinueWith(t => _httpServer.Close());

            return taskCompletion.Task;
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
			            }
			
			            function parameterNullOrEmpty(name, url)
                            {
                                name = name.replace(/[\[\]]/ g, ""\\$&"");
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
