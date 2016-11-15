﻿using System;
using System.Threading.Tasks;
using Foundations.Http;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class BrowserAuthorizerUI<TCredentials> :
        OAuthAuthorizationUITemplateBase<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly HttpServer _httpServer;
        private readonly Uri _callbackUri;

        public BrowserAuthorizerUI(
            HttpServer httpServer,
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri,
            AuthorizationInterface @interface,
            Action<Action> runOnMainThread,
            Func<bool> isOnline) : 
                base(
                    handler,
                    callbackUri,
                    @interface, 
                    runOnMainThread,
                    isOnline)
        {
            _httpServer = httpServer;
            _callbackUri = callbackUri;
        }

        protected override Task MakeAuthorizationRequest(
            Uri authorizationUri, 
            Func<Uri, object, bool> callbackHandler)
        {
#pragma warning disable 4014
            _httpServer.CreateServer(
                (request, response) =>
                {
                    response.WriteHtmlString(FRAGMENT_HTML);

                    var success = callbackHandler(request.Uri, null);

                    if (success)
                    {
                        _httpServer.Close();
                    }
                })
                .Listen(_callbackUri);
#pragma warning restore 4014

            Platform.Current.Launch(authorizationUri);

            return Task.FromResult(true);
        }

        protected override void CleanupView(object view)
        { }


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