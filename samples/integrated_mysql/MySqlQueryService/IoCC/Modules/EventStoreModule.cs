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
                        var configuration = context.Resolve<IConfiguration>();
                        var mySqlConnectionString = configuration.GetValue<string>("CoreEventStore:MySqlConfig:ConnectionString");
                        
                        var  mySqlConfiguration = new MySqlConfiguration();
                        mySqlConfiguration.ConnectionString = mySqlConnectionString;
                        return mySqlConfiguration;
                        
                    })
                    .KeepPositionInMySql()
                    .KeepIdempotenceInMySql();
            }
        }
    }
