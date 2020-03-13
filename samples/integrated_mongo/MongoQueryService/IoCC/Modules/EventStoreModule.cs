using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Mongo.Autofac;
using IntegrationEvents;
using Microsoft.Extensions.Configuration;
using MongoQueryService.InvokerPipelines;

namespace MongoQueryService.IoCC.Modules
{
    public class EventStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            IConfiguration configuration = null;
            builder.RegisterEventStore(initializationConfiguration =>
                {
                    configuration = initializationConfiguration.Resolve<IConfiguration>();
                    var eventStoreConnectionString = configuration.GetValue<string>("CoreEventStore:EventStoreConfig:ConnectionString");
                    var init = new InitializationConfiguration()
                    {
                        EventStoreConnectionString = eventStoreConnectionString,
                    };
                    return init;
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