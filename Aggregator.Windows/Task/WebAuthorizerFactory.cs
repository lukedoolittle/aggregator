using System;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure;
using Aggregator.Task.Http;

namespace Aggregator.Task.Factories
{
    public class WebAuthorizerFactory : IWebAuthorizerFactory
    {
        public IWebAuthorizer GetAuthorizer<TService>(
            object context,
            AuthenticationInterfaceEnum browserType)
            where TService : Service
        {
            if (browserType == AuthenticationInterfaceEnum.Embedded)
            {
                throw new NotSupportedException();
            }

            return new BrowserWebAuthorizer(new HttpServer());
        }
    }
}
