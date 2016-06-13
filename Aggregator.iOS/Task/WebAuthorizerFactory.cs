using System;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure;
using Aggregator.Task.Http;
using Aggregator.View.WebAuthorization;
using UIKit;

namespace Aggregator.Task
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
                    return new UIWebViewWebAuthorizer(context as UIViewController);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
