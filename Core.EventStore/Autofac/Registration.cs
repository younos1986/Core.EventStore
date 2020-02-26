using Autofac;
using Core.EventStore.Builders;
using Core.EventStore.Dependencies;
using Core.EventStore.Invokers;
using Core.EventStore.Registration;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventStore.Autofac
{
    public static class Registration
    {

        public static IEventStoreConnection EventStoreConnection { get; private set; }


        public static ContainerBuilder RegisterEventStore(this ContainerBuilder containerBuilder , Action<InitializationConfiguration> initializationConfiguration, ProjectorInvoker projectorInvoker = null)
        {
            InitializationConfiguration configuration = new InitializationConfiguration();
            initializationConfiguration.Invoke(configuration);


            EventStoreConnection = EventStoreConnectionBuilder.Build(configuration);

            containerBuilder.RegisterType<EventStoreDbContext>().As<IEventStoreDbContext>()
                .WithParameter(new TypedParameter(typeof(IEventStoreConnection), EventStoreConnection));

            if (projectorInvoker == null)
            {
                projectorInvoker = new EventInvoker();
            }

            containerBuilder.RegisterInstance(projectorInvoker).As<ProjectorInvoker>();


            return containerBuilder;
        }


        public static void SubscribeRead(this ContainerBuilder containerBuilder, Action<SubscriptionConfiguration> subscriptionConfiguration )
        {
            SubscriptionConfiguration configuration = new SubscriptionConfiguration();
            subscriptionConfiguration.Invoke(configuration);



            var projectorInvoker = containerBuilder.Build().Resolve<ProjectorInvoker>();

            PersistentSubscriptionClient persistentSubscriptionClient = new PersistentSubscriptionClient(EventStoreConnection, projectorInvoker);


            persistentSubscriptionClient.Start();
        }



        public static ContainerBuilder SubscribeReadWithTracking(this ContainerBuilder containerBuilder, Action<SubscriptionConfiguration> subscriptionConfiguration, ProjectorInvoker projectorInvoker = null)
        {
            SubscriptionConfiguration configuration = new SubscriptionConfiguration();
            subscriptionConfiguration.Invoke(configuration);

            if (projectorInvoker == null)
                projectorInvoker = new EventInvoker();

            var persistentSubscriptionClient = new PersistentSubscriptionClient(EventStoreConnection, projectorInvoker);

            persistentSubscriptionClient.Start();

            return containerBuilder;
        }

    }
}
