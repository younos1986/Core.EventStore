using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.EFCore.SqlServer.Autofac;
using EfCoreQueryService.InvokerPipelines;
using IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace EfCoreQueryService.IoCC.Modules
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
                    .UseeEfCore(configuration =>
                    {
                        configuration.ConnectionString = "Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;";
                        configuration.DefaultSchema = "dbo";
                        
                        //configuration.DbContext = new DbContext();   //implement
                        
                    })
                    .KeepPositionInEfCore()
                    .KeepIdempotenceInEfCore();
            }
        }
    }
