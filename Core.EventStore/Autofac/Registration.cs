using Autofac;
using Autofac.Builder;
using Core.EventStore.Builders;
using Core.EventStore.Dependencies;
using Core.EventStore.Invokers;
using Core.EventStore.Managers;
using Core.EventStore.Registration;
using EventStore.ClientAPI;
using System;
using Microsoft.Extensions.DependencyInjection;

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


            
            containerBuilder.RegisterInstance(EventStoreConnection).As<IEventStoreConnection>().SingleInstance();
            containerBuilder.RegisterType<EventStoreConnectionManager>().As<IEventStoreConnectionManager>().SingleInstance();
            containerBuilder.RegisterType<PersistentSubscriptionClient>().As<IPersistentSubscriptionClient>().SingleInstance();
            containerBuilder.RegisterType<EventStoreDbContext>().As<IEventStoreDbContext>().SingleInstance();
            

            //containerBuilder.RegisterType<EventStoreDbContext>().As<IEventStoreDbContext>()
            //    .WithParameter(new TypedParameter(typeof(IEventStoreConnection), EventStoreConnection)).SingleInstance();

            if (projectorInvoker == null)
            {
                projectorInvoker = new EventInvoker();
            }
            containerBuilder.RegisterInstance(projectorInvoker).As<ProjectorInvoker>().SingleInstance();


            return containerBuilder;
        }


        public static ContainerBuilder SubscribeRead(this ContainerBuilder containerBuilder,  Action<SubscriptionConfiguration> subscriptionConfiguration)
        {
            SubscriptionConfiguration configuration = new SubscriptionConfiguration();
            subscriptionConfiguration.Invoke(configuration);

            //var container = containerBuilder.Build();
            //var projectorInvoker = container.Resolve<ProjectorInvoker>();
            //var eventStoreConnectionManager = container.Resolve<IEventStoreConnectionManager>();
            //var persistentSubscriptionClient = serviceProvider.GetRequiredService<IPersistentSubscriptionClient>();
            //var persistentSubscriptionClient = container.Resolve<IPersistentSubscriptionClient>();
            //persistentSubscriptionClient.Start();
            return containerBuilder;
        }
        
      
        public static ContainerBuilder SubscribeReadWithTracking(this ContainerBuilder containerBuilder, Action<SubscriptionConfiguration> subscriptionConfiguration, ProjectorInvoker projectorInvoker = null)
        {
            SubscriptionConfiguration configuration = new SubscriptionConfiguration();
            subscriptionConfiguration.Invoke(configuration);

            if (projectorInvoker == null)
                projectorInvoker = new EventInvoker();

            //var persistentSubscriptionClient = new PersistentSubscriptionClient(EventStoreConnection, projectorInvoker);

            //persistentSubscriptionClient.Start();

            return containerBuilder;
        }

    }
}
