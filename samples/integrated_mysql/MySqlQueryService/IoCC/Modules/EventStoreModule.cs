using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.MySql.EFCore.Autofac;
using IntegrationEvents;
using MySqlQueryService.InvokerPipelines;

namespace MySqlQueryService.IoCC.Modules
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
                        subscriptionConfiguration.AddEvent<CustomerCreatedForMySql>(nameof(CustomerCreatedForMySql));
                        subscriptionConfiguration.AddEvent<CustomerModified>(nameof(CustomerModified));
                    }, new CustomProjectorInvoker())
                    .UseeMySql(configuration =>
                    {
                        configuration.ConnectionString = "server=localhost;Database=EventStoreDb;uid=root;pwd=TTTttt456;sslmode=none;";
                    })
                    .KeepPositionInMySql()
                    .KeepIdempotenceInMySql();
            }
        }
    }
