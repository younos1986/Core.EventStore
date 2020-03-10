using Autofac;
using Core.EventStore.Autofac;

namespace MySqlCommandService.IoCC.Modules
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
            });
        }
    }
}
