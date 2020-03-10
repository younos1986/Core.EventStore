using Autofac;
using Core.EventStore.Contracts;
using EfCoreQueryService.Projectors;
using IntegrationEvents;

namespace EfCoreQueryService.IoCC.Modules
{
    public class ProjectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerCreatedEventProjector>().As<IProjector<CustomerCreatedForEfCore>>();
            //builder.RegisterType<CustomerInsertedEventProjector>().As<IProjector<CustomerCreated>>();
        }
    }
}