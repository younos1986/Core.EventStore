using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.EFCore.SqlServer.Autofac;
using EfCoreQueryService.InvokerPipelines;
using IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EfCoreQueryService.IoCC.Modules
{
        public class EventStoreModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterEventStore(context =>
                    {
                        var configuration = context.Resolve<IConfiguration>();
                        var eventStoreConnectionString = configuration.GetValue<string>("CoreEventStore:EventStoreConfig:ConnectionString");
                        var init = new InitializationConfiguration()
                        {
                            EventStoreConnectionString = eventStoreConnectionString,
                        };
                        return init;
                    })
                    .SubscribeRead(subscriptionConfiguration =>
                    {
                        subscriptionConfiguration.AddEvent<CustomerCreatedForEfCore>(nameof(CustomerCreatedForEfCore));
                    }, new CustomProjectorInvoker())
                    .UseeEfCore(context =>
                    {
                        var configuration = context.Resolve<IConfiguration>();

                        var efCoreConfiguration = new EfCoreConfiguration();
                        efCoreConfiguration.ConnectionString  = configuration.GetValue<string>("CoreEventStore:SqlServerConfig:ConnectionString"); 
                        efCoreConfiguration.DefaultSchema = configuration.GetValue<string>("CoreEventStore:SqlServerConfig:DefaultSchema");

                        return efCoreConfiguration;
                    })
                    .KeepPositionInEfCore()
                    .KeepIdempotenceInEfCore();
                
            }
        }
    }
