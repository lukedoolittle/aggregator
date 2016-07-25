using System;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Configuration;
using Aggregator.Configuration.Registration;
using Aggregator.Task;

namespace Aggregator.Test.Helpers
{
    public class BootstrapFixture : IDisposable
    {
        public IServiceLocator Resolver { get; }

        public BootstrapFixture()
        {
            Resolver = new Bootstrapper().Run();
        }

        public virtual void Dispose()
        {
            UserSettings.UserId = Guid.Empty;
            (Resolver.GetMemberValue<IComponentContext>("_container") as IContainer).Dispose();
        }
    }
}
