using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Microsoft.Extensions.Configuration;

namespace MongoCommandService.IoCC.Modules
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
            });
            
            // builder.RegisterEventStore(initializationConfiguration =>
            // {
            //     initializationConfiguration.Username = "admin";
            //     initializationConfiguration.Password = "changeit";
            //     initializationConfiguration.DefaultPort = 1113;
            //     //initializationConfiguration.IsDockerized = true;
            //     //initializationConfiguration.DockerContainerName = "eventstore";
            //
            //     initializationConfiguration.IsDockerized = false;
            //     initializationConfiguration.ConnectionUri = "127.0.0.1";
            // });
        }
    }
}
