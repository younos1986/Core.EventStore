using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Microsoft.Extensions.Configuration;

namespace CommandService.IoCC.Modules
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
        }
    }
}
