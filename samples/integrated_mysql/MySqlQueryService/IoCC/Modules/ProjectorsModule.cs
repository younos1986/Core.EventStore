using Autofac;
using Core.EventStore.Contracts;
using IntegrationEvents;
using MySqlQueryService.Projectors;

namespace MySqlQueryService.IoCC.Modules
{
    public class ProjectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerCreatedEventProjector>().As<IProjector<CustomerCreatedForMySql>>();
            //builder.RegisterType<CustomerInsertedEventProjector>().As<IProjector<CustomerCreated>>();
        }
    }
}