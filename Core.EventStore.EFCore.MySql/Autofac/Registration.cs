using System;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.DbContexts;
using Core.EventStore.MySql.EFCore.Implementations;

namespace Core.EventStore.MySql.EFCore.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder UseeMySql(this ContainerBuilder containerBuilder, Action<MySqlConfiguration> efCoreConfiguration)
        {
            MySqlConfiguration configuration = new MySqlConfiguration();

            efCoreConfiguration.Invoke(configuration);
            containerBuilder.RegisterInstance(configuration).As<IMySqlConfiguration>()
                .IfNotRegistered(typeof(IMySqlConfiguration)).SingleInstance();
            
            containerBuilder
                .RegisterType<EventStoreMySqlDbContext>()
                .WithParameter("options", parameterValue: DbContextOptionsFactory.Get(configuration .ConnectionString))
                .InstancePerLifetimeScope();
            
            return containerBuilder;
        }
        
        public static ContainerBuilder KeepPositionInMySql(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();
            
            return containerBuilder;
        }
        
        public static ContainerBuilder KeepIdempotenceInMySql(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();
            
            return containerBuilder;
        }
    }
}
