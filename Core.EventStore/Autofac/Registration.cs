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


        public static ContainerBuilder RegisterEventStore(this ContainerBuilder containerBuilder , Action<InitializationConfiguration> initializationConfiguration)
        {
            InitializationConfiguration configuration = new InitializationConfiguration();
            initializationConfiguration.Invoke(configuration);

            EventStoreConnection = EventStoreConnectionBuilder.Build(configuration);
            
            containerBuilder.RegisterInstance(EventStoreConnection).As<IEventStoreConnection>().SingleInstance();
            containerBuilder.RegisterType<EventStoreConnectionManager>().As<IEventStoreConnectionManager>().SingleInstance();
            containerBuilder.RegisterType<PersistentSubscriptionClient>().As<IPersistentSubscriptionClient>().SingleInstance();//.PreserveExistingDefaults();
            containerBuilder.RegisterType<EventStoreDbContext>().As<IEventStoreDbContext>().SingleInstance();
            
            return containerBuilder;
        }


        public static ContainerBuilder SubscribeRead(this ContainerBuilder containerBuilder,  Action<SubscriptionConfiguration> subscriptionConfiguration, ProjectorInvoker projectorInvoker = null)
        {
            SubscriptionConfiguration configuration = new SubscriptionConfiguration();
            subscriptionConfiguration.Invoke(configuration);
            containerBuilder.RegisterInstance(configuration).As<ISubscriptionConfiguration>().SingleInstance();
            
            if (projectorInvoker == null)
                containerBuilder.RegisterType<EventInvoker>().As<ProjectorInvoker>().SingleInstance();
            else
                containerBuilder.RegisterInstance(projectorInvoker).As<ProjectorInvoker>().SingleInstance();    
            
            return containerBuilder;
        }
      
        public static ContainerBuilder SubscribeReadWithTracking(this ContainerBuilder containerBuilder, Action<SubscriptionConfiguration> subscriptionConfiguration, ProjectorInvoker projectorInvoker = null)
        {
            return containerBuilder;
        }

    }
}
