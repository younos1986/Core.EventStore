using Autofac;
using Core.EventStore.Contracts;
using IntegrationEvents;
using PostgresQueryService.Projectors;

namespace PostgresQueryService.IoCC.Modules
{
    public class ProjectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerCreatedEventProjector>().As<IProjector<CustomerCreatedForPostgres>>();
            //builder.RegisterType<CustomerInsertedEventProjector>().As<IProjector<CustomerCreated>>();
        }
    }
}