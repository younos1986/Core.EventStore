using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using IntegrationEvents;
using PostgresQueryService.InvokerPipelines;

namespace PostgresQueryService.IoCC.Modules
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
