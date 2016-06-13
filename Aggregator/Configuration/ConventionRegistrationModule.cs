using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Features.ResolveAnything;
using BatmansBelt;
using Aggregator.Framework;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Logging;
using Aggregator.Infrastructure.Messaging;
using Aggregator.Infrastructure.Repositories;
using Aggregator.Task;
using SimpleCQRS.Autofac;
using SimpleCQRS.Framework.Contracts;
using SimpleCQRS.Infrastructure;
using Module = Autofac.Module;
#if __MOBILE__
using Aggregator.Infrastructure.ComponentManagers;
using Aggregator.Framework.Contracts;
using Couchbase.Lite;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Robotics.Mobile.Core.Bluetooth.LE;
#endif

namespace Aggregator.Configuration.Registration
{
    public class ConventionRegistrationModule : Module
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public ConventionRegistrationModule(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_assemblies.ToArray())
                .AsImplementedInterfaces()
                .AsSelf()
                .Except<LoggingScheduler>(
                    a => a
                        .As<Scheduler>()
                        .SingleInstance())
                .Except<LoggingMessageBus>(
                    a => a
                        .As<IMessageBus>()
                        .As<IEventPublisher>()
                        .As<ICommandSender>()
                        .As<ISubscriptionManager>()
                        .SingleInstance())
#if __MOBILE__
                .Except<BluetoothManager>(a => a.As<IBluetoothManager>().SingleInstance())
#else
                .Except<Couchbase.CouchbaseClient>()
#endif
                .Except<CouchbaseView>()
                .Except<ConsoleLogger>()
                .Except<DatabaseLogger>();

            builder.RegisterGeneric(typeof(CouchbaseDatabase<>))
                .As(typeof(IDatabase<>))
                .InstancePerDependency();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

#if DEBUG
            var loggingSettings = new LoggingSettings(
                logErrors: true,
                logInfos: true);
            builder.Register(c =>
                    new ConsoleLogger(loggingSettings))
                .As<ILogger>();
#else
            //var loggingSettings = new LoggingSettings(
            //    logErrors: true,
            //    logInfos: false);
            //builder.Register(c => 
            //        new DatabaseLogger(
            //            c.Resolve<IDatabase<LogEntry>>(), 
            //            loggingSettings))
            //    .As<ILogger>();
#endif

            builder.Register(c =>
                new CouchbaseView(
                    ApplicationSettings.CouchbaseDesignDoc,
                    ApplicationSettings.CouchbaseView));

#if __MOBILE__
            builder.RegisterInstance(Manager.SharedInstance.GetDatabase("default"));
            builder.RegisterInstance(CrossGeolocator.Current).As<IGeolocator>();
#else
            builder.RegisterInstance(new Couchbase.CouchbaseClient()).SingleInstance();
#endif
#if __IOS__
            builder.RegisterInstance(Adapter.Current)
                .As<IAdapter>()
                .SingleInstance();
#endif
#if __ANDROID__
            builder.RegisterType<Adapter>()
                .As<IAdapter>()
                .SingleInstance();
#endif
        }
    }
}
