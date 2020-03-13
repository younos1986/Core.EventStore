using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using IntegrationEvents;
using Microsoft.Extensions.Configuration;
using PostgresQueryService.InvokerPipelines;

namespace PostgresQueryService.IoCC.Modules
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
                        subscriptionConfiguration.AddEvent<CustomerCreatedForPostgres>(nameof(CustomerCreatedForPostgres));
                        subscriptionConfiguration.AddEvent<CustomerModified>(nameof(CustomerModified));
                    }, new CustomProjectorInvoker())
                    .UseePostgreSQL(configuration =>
                    {
                        
                        configuration.ConnectionString = "Host=localhost;Port=5432;Database=eventstoredb;Username=postgres;password=TTTttt456";
                        //configuration.DefaultSchema = "essch";
                    })
                    .KeepPositionInPostgreSQL()
                    .KeepIdempotenceInPostgreSQL();
            }
        }
    }
