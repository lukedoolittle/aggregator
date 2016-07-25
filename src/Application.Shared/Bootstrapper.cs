using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aggregator.Framework.Logging;
using Aggregator.Infrastructure.Messaging;
using Aggregator.Infrastructure.Repositories;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Task;
using Autofac.Features.ResolveAnything;
using Foundations.Bootstrap;
using SimpleCQRS.Framework.Contracts;

#if __MOBILE__
using Aggregator.Infrastructure.Adapters;
#endif

namespace Aggregator.Configuration
{
    public class Bootstrapper : AutofacBootstrapperBase
    {
        protected List<string> _blacklistedAssemblies = new List<string>
        {
            "RestSharp",
            "Autofac",
            "Newtonsoft.Json",
            "Stateless",
            "System.ServiceModel.Internals",
            "SQLitePCL.ugly",
            "SQLitePCL.raw",
            "Robotics.Mobile.Core.Droid",
            "Robotics.Mobile.Core",
            "Plugin.Settings",
            "Plugin.Settings.Abstractions",
            "Plugin.Permissions",
            "Plugin.Permissions.Abstractions",
            "Plugin.Geolocator",
            "Plugin.Geolocator.Abstractions",
            "Plugin.CurrentActivity",
            "Plugin.Connectivity",
            "Plugin.Connectivity.Abstractions",
            "ICSharpCode.SharpZipLib.Portable",
            "crypto",
            "Couchbase.Lite.Storage.SystemSQLite",
            "Couchbase.Lite",
            "Autofac.Extras.CommonServiceLocator"
        };

        public AutofacBootstrapperBase DefaultConfig()
        {
            AddAllLoadedAssemblies(_blacklistedAssemblies.ToArray());
            AddAllModules();
            AddAllStartupTasks();

            return this;
        }

        protected override IServiceLocator BuildContainer(
            ContainerBuilder builder, 
            IEnumerable<Assembly> assemblies)
        {
            AssemblyScanningRegistration(builder, assemblies);

            builder.RegisterAssemblyGenericInterfaceImplementors(
                assemblies,
                typeof(IEventHandler<>));

            builder.RegisterAssemblyGenericInterfaceImplementors(
                assemblies,
                typeof(ICommandHandler<>));

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            return base.BuildContainer(builder, assemblies);
        }

        private void AssemblyScanningRegistration(
            ContainerBuilder builder, 
            IEnumerable<Assembly> assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsImplementedInterfaces()
                .AsSelf()
                .Except<LoggingSchedulerDecorator>()
                .Except<LoggingMessageBusDecorator>()
#if __MOBILE__
                .Except<BluetoothAdapter>()
#else
                .Except<Couchbase.CouchbaseClient>()
#endif
                .Except<CouchbaseView>()
                .Except<ConsoleLogger>()
                .Except<DatabaseLogger>();
        }
    }
}
