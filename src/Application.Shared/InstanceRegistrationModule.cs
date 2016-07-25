using Autofac;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Logging;
using Aggregator.Infrastructure.Messaging;
using Aggregator.Infrastructure.Repositories;
using Aggregator.Task;
using Material.Contracts;
using SimpleCQRS.Framework.Contracts;
using SimpleCQRS.Infrastructure;
using Module = Autofac.Module;
#if __MOBILE__
using Aggregator.Infrastructure.Adapters;
using Couchbase.Lite;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Robotics.Mobile.Core.Bluetooth.LE;
#endif
#if __IOS__
using CoreLocation;
#endif

namespace Aggregator.Configuration.Registration
{
    public class InstanceRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(CouchbaseDatabase<>))
                .As(typeof(IDatabase<>))
                .InstancePerDependency();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

            builder.RegisterType<LoggingMessageBusDecorator>()
                .As<IMessageBus>()
                .As<IEventPublisher>()
                .As<ICommandSender>()
                .As<ISubscriptionManager>()
                .SingleInstance();

            builder.RegisterType<LoggingSchedulerDecorator>()
                .As<Scheduler>()
                .SingleInstance();

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
            builder.RegisterType<BluetoothAdapter>().As<IBluetoothAdapter>().SingleInstance();
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

#if __IOS__
            new CLLocationManager().RequestAlwaysAuthorization();
            CrossGeolocator.Current.AllowsBackgroundUpdates = true;
#endif
        }
    }
}
