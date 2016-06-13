using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Configuration.Registration;
using Aggregator.Task;
using SimpleCQRS.Autofac;

namespace Aggregator.Configuration
{
    public class Bootstrapper : CQRSAutofacBootstrapper
    {
        protected override IServiceLocator BuildContainer(
            ContainerBuilder builder, 
            IEnumerable<Assembly> assemblies)
        {
            AddAllStartupTasks();
            builder.RegisterModule(new ConventionRegistrationModule(_typeAssemblies));
            return base.BuildContainer(builder, assemblies);
        }

        public IServiceLocator RunAndSchedule()
        {
            var serviceLocator = base.Run();

            serviceLocator.GetInstance<Scheduler>().Start();

            return serviceLocator;
        }
    }
}
