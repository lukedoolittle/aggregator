using System;
using System.Threading.Tasks;
using Foundations.Http;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class BrowserAuthorizerUI<TCredentials> :
        AuthorizerUITemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly HttpServer _httpServer;
        private readonly Uri _callbackUri;

        public BrowserAuthorizerUI(
            HttpServer httpServer,
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri,
            AuthorizationInterface @interface,
            Action<Action> runOnMainThread) : 
                base(
                    handler,
                    callbackUri,
                    @interface, 
                    runOnMainThread)
        {
            _httpServer = httpServer;
            _callbackUri = callbackUri;
        }

        //TODO: fix this to properly inherit from AuthorizerUITemplate
        public override Task<TCredentials> Authorize(
            Uri authorizationUri,
            string userId)
        {
            var completionSource = new TaskCompletionSource<TCredentials>();

#pragma warning disable 4014
            _httpServer.CreateServer(
                (request, response) =>
                {
                    response.WriteHtmlString(FRAGMENT_HTML);

                    RespondToUri(
                        request.Uri,
                        userId,
                        completionSource,
                        () => { });
                })
                .Listen(_callbackUri);
#pragma warning restore 4014

            completionSource.Task.ContinueWith(t => _httpServer.Close());

            Platform.Current.Launch(authorizationUri);

            return completionSource.Task;
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
