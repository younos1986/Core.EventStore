using Autofac;
using Autofac.Builder;
using Core.EventStore.Builders;
using Core.EventStore.Dependencies;
using Core.EventStore.Invokers;
using Core.EventStore.Managers;
using Core.EventStore.Registration;
using EventStore.ClientAPI;
using System;
using Core.EventStore.IdempotencyServices;
using Core.EventStore.Services;

namespace Core.EventStore.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder RegisterEventStore(this ContainerBuilder containerBuilder , Action<InitializationConfiguration> initializationConfiguration)
        {
            InitializationConfiguration configuration = new InitializationConfiguration();
            initializationConfiguration.Invoke(configuration);

            IEventStoreConnection EventStoreConnection = EventStoreConnectionBuilder.Build(configuration);
            
            containerBuilder.RegisterInstance(configuration).As<InitializationConfiguration>().IfNotRegistered(typeof(InitializationConfiguration)).SingleInstance();
            containerBuilder.RegisterInstance(EventStoreConnection).As<IEventStoreConnection>().SingleInstance();
            containerBuilder.RegisterType<EventStoreConnectionManager>().As<IEventStoreConnectionManager>().SingleInstance();
            containerBuilder.RegisterType<PersistentSubscriptionClient>().As<IPersistentSubscriptionClient>().SingleInstance();//.PreserveExistingDefaults();
            containerBuilder.RegisterType<EventStoreDbContext>().As<IEventStoreDbContext>().SingleInstance();
            containerBuilder.RegisterType<EventStoreReader>().As<IEventStoreReader>().SingleInstance();
            
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
      
        public static ContainerBuilder KeepPositionInMongo(this ContainerBuilder containerBuilder, Action<MongoConfiguration> mongoConfiguration = null)
        {
            if (mongoConfiguration != null)
            {
                MongoConfiguration configuration = new MongoConfiguration();
                mongoConfiguration.Invoke(configuration);
                containerBuilder.RegisterInstance(configuration).As<IMongoConfiguration>()
                    .IfNotRegistered(typeof(IMongoConfiguration)).SingleInstance();
            }
            
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();

            return containerBuilder;
        }
        
        public static ContainerBuilder KeepIdempotencyInMongo(this ContainerBuilder containerBuilder, Action<MongoConfiguration> mongoConfiguration = null)
        {
            if (mongoConfiguration != null)
            {
                MongoConfiguration configuration = new MongoConfiguration();
                mongoConfiguration.Invoke(configuration);
                containerBuilder.RegisterInstance(configuration).As<IMongoConfiguration>()
                    .IfNotRegistered(typeof(IMongoConfiguration)).SingleInstance();
            }

            containerBuilder.RegisterType<IdempotencyWriterService>().As<IIdempotencyWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotencyReaderService>().As<IIdempotencyReaderService>().SingleInstance();
            
            return containerBuilder;
        }
    }
}
