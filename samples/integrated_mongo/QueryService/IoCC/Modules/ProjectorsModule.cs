using Autofac;
using Core.EventStore.Contracts;
using IntegrationEvents;
using QueryService.Projectors;

namespace QueryService.IoCC.Modules
{
    public class ProjectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerCreatedEventProjector>().As<IProjector<CustomerCreated>>();
            builder.RegisterType<CustomerInsertedEventProjector>().As<IProjector<CustomerCreated>>();
        }
    }
}