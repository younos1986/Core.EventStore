using System;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.PostgreSQL.DbContexts;
using Core.EventStore.EFCore.PostgreSQL.Implementations;

namespace Core.EventStore.EFCore.PostgreSQL.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder KeepPositionInPostgreSQL(this ContainerBuilder containerBuilder, Action<EfCoreConfiguration> mongoConfiguration = null)
        {
            EfCoreConfiguration configuration = new EfCoreConfiguration();
            if (mongoConfiguration != null)
            {
                
                mongoConfiguration.Invoke(configuration);
                containerBuilder.RegisterInstance(configuration).As<IEfCoreConfiguration>()
                    .IfNotRegistered(typeof(IEfCoreConfiguration)).SingleInstance();
            }
            
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();
            
            
            
            // containerBuilder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            containerBuilder
                .RegisterType<EventStoreEfCoreDbContext>()
                .WithParameter("options", parameterValue: DbContextOptionsFactory.Get(configuration .ConnectionString))
                .InstancePerLifetimeScope();
            

            return containerBuilder;
        }
        
        public static ContainerBuilder KeepIdempotenceInPostgreSQL(this ContainerBuilder containerBuilder, Action<EfCoreConfiguration> mongoConfiguration = null)
        {
            if (mongoConfiguration != null)
            {
                EfCoreConfiguration configuration = new EfCoreConfiguration();
                mongoConfiguration.Invoke(configuration);
                containerBuilder.RegisterInstance(configuration).As<IEfCoreConfiguration>()
                    .IfNotRegistered(typeof(IEfCoreConfiguration)).SingleInstance();
            }

            containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();
            
            return containerBuilder;
        }
    }
}
