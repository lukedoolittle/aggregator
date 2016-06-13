using Autofac;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test
{
    public class TestingMessageBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrackingMessageBusDecorator>()
                .AsSelf()
                .As<IMessageBus>()
                .As<ICommandSender>()
                .As<IEventPublisher>()
                .As<ISubscriptionManager>()
                .SingleInstance();
        }
    }
}
