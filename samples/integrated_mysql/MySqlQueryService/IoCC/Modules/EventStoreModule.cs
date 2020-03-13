using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.MySql.EFCore.Autofac;
using IntegrationEvents;
using Microsoft.Extensions.Configuration;
using MySqlQueryService.InvokerPipelines;

namespace MySqlQueryService.IoCC.Modules
{
        public class EventStoreModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterEventStore(initializationConfiguration =>
                    {
                        var configuration = initializationConfiguration.Resolve<IConfiguration>();
                        var eventStoreConnectionString = configuration.GetValue<string>("CoreEventStore:EventStoreConfig:ConnectionString");
                        var init = new InitializationConfiguration()
                        {
                            EventStoreConnectionString = eventStoreConnectionString,
                        };
                        return init;
                    })
                    .SubscribeRead(subscriptionConfiguration =>
                    {
                        subscriptionConfiguration.AddEvent<CustomerCreatedForMySql>(nameof(CustomerCreatedForMySql));
                        subscriptionConfiguration.AddEvent<CustomerModified>(nameof(CustomerModified));
                    }, new CustomProjectorInvoker())
                    .UseeMySql(context =>
                    {
                        IMySqlConfiguration configuration = new MySqlConfiguration();
                        configuration.ConnectionString = "server=localhost;Database=EventStoreDb;uid=root;pwd=TTTttt456;sslmode=none;";
                        return configuration;
                        
                    })
                    .KeepPositionInMySql()
                    .KeepIdempotenceInMySql();
            }
        }
    }
