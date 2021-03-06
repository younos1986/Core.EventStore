﻿using Autofac;
using Autofac.Builder;
using Core.EventStore.Builders;
using Core.EventStore.Dependencies;
using Core.EventStore.Invokers;
using Core.EventStore.Managers;
using Core.EventStore.Registration;
using EventStore.ClientAPI;
using System;
using Core.EventStore.Configurations;

namespace Core.EventStore.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder RegisterEventStore(this ContainerBuilder containerBuilder , Func<IComponentContext, InitializationConfiguration> initializationConfiguration)
        {
            InitializationConfiguration configuration = null;
            containerBuilder.Register<IEventStoreConnection>((Func<IComponentContext, IEventStoreConnection>) (context =>
            {
                configuration = initializationConfiguration(context);
                containerBuilder.RegisterInstance(configuration).As<InitializationConfiguration>().IfNotRegistered(typeof(InitializationConfiguration)).SingleInstance();
                
                
                IEventStoreConnection eventStoreConnection = EventStoreConnectionBuilder.Build(configuration);
                return eventStoreConnection;
            })).As<IEventStoreConnection>().SingleInstance();
            
            
            //containerBuilder.RegisterInstance(configuration).As<InitializationConfiguration>().IfNotRegistered(typeof(InitializationConfiguration)).SingleInstance();
            
            //InitializationConfiguration configuration = new InitializationConfiguration();
            //initializationConfiguration.Invoke(configuration);
            //IEventStoreConnection EventStoreConnection = EventStoreConnectionBuilder.Build(configuration);
            //containerBuilder.RegisterInstance(EventStoreConnection).As<IEventStoreConnection>().SingleInstance();
            
            
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
      
        // public static ContainerBuilder KeepPositionInMongo(this ContainerBuilder containerBuilder, Action<MongoConfiguration> mongoConfiguration = null)
        // {
        //     if (mongoConfiguration != null)
        //     {
        //         MongoConfiguration configuration = new MongoConfiguration();
        //         mongoConfiguration.Invoke(configuration);
        //         containerBuilder.RegisterInstance(configuration).As<IMongoConfiguration>()
        //             .IfNotRegistered(typeof(IMongoConfiguration)).SingleInstance();
        //     }
        //     
        //     containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
        //     containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();
        //
        //     return containerBuilder;
        // }
        //
        // public static ContainerBuilder KeepIdempotenceInMongo(this ContainerBuilder containerBuilder, Action<MongoConfiguration> mongoConfiguration = null)
        // {
        //     if (mongoConfiguration != null)
        //     {
        //         MongoConfiguration configuration = new MongoConfiguration();
        //         mongoConfiguration.Invoke(configuration);
        //         containerBuilder.RegisterInstance(configuration).As<IMongoConfiguration>()
        //             .IfNotRegistered(typeof(IMongoConfiguration)).SingleInstance();
        //     }
        //
        //     containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
        //     containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();
        //     
        //     return containerBuilder;
        // }
    }
}
