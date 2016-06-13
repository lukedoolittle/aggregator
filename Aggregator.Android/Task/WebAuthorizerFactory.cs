using System;
using Android.Content;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure;
using Aggregator.Task.Http;
using Aggregator.View.WebAuthorization;

namespace Aggregator.Task.Factories
{
    public class WebAuthorizerFactory : IWebAuthorizerFactory
    {
        public IWebAuthorizer GetAuthorizer<TService>(
            object context,
            AuthenticationInterfaceEnum browserType) 
            where TService : Service
        {
            switch (browserType)
            {
                case AuthenticationInterfaceEnum.Dedicated:
                    return new BrowserWebAuthorizer(new HttpServer());
                    break;
                 case AuthenticationInterfaceEnum.Embedded:
                    return new WebViewWebAuthorizer(context as Context);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}