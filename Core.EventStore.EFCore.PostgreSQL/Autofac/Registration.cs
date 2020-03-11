using System;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.PostgreSQL.DbContexts;
using Core.EventStore.EFCore.PostgreSQL.Implementations;

namespace Core.EventStore.EFCore.PostgreSQL.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder UseePostgreSQL(this ContainerBuilder containerBuilder, Action<PostgreSqlConfiguration> efCoreConfiguration)
        {
            PostgreSqlConfiguration configuration = new PostgreSqlConfiguration();

            efCoreConfiguration.Invoke(configuration);
            containerBuilder.RegisterInstance(configuration).As<IPostgreSqlConfiguration>()
                .IfNotRegistered(typeof(IPostgreSqlConfiguration)).SingleInstance();
            
            containerBuilder
                .RegisterType<EventStorePostgresDbContext>()
                .WithParameter("options", parameterValue: DbContextOptionsFactory.Get(configuration .ConnectionString))
                .InstancePerLifetimeScope();
            
            return containerBuilder;
        }
        
        public static ContainerBuilder KeepPositionInPostgreSQL(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();
            
            return containerBuilder;
        }
        
        public static ContainerBuilder KeepIdempotenceInPostgreSQL(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();
            
            return containerBuilder;
        }
    }
}
