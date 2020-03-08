﻿using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Mongo.Autofac;
using IntegrationEvents;
using MongoQueryService.InvokerPipelines;

namespace MongoQueryService.IoCC.Modules
{
    public class EventStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterEventStore(initializationConfiguration =>
            {
                initializationConfiguration.Username = "admin";
                initializationConfiguration.Password = "changeit";
                initializationConfiguration.DefaultPort = 1113;
            
                //initializationConfiguration.IsDockerized = true;
                //initializationConfiguration.DockerContainerName = "eventstore";
            
                initializationConfiguration.IsDockerized = false;
                initializationConfiguration.ConnectionUri = "127.0.0.1";
            })
                .SubscribeRead(subscriptionConfiguration =>
                {
                    subscriptionConfiguration.AddEvent<CustomerCreated>(nameof(CustomerCreated));
                    subscriptionConfiguration.AddEvent<CustomerModified>(nameof(CustomerModified));
                }, new CustomProjectorInvoker())
                .KeepPositionInMongo(configuration =>
                    {
                        configuration.ConnectionString = "mongodb://127.0.0.1";
                        configuration.DatabaseName = "TestDB";
                    })
                .KeepIdempotenceInMongo();
        }
    }
}