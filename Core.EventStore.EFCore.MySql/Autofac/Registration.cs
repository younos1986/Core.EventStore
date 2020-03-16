using System;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.DbContexts;
using Core.EventStore.MySql.EFCore.Implementations;

namespace Core.EventStore.MySql.EFCore.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder UseeMySql(this ContainerBuilder containerBuilder, Func<IComponentContext, MySqlConfiguration> mySqlConfiguration)
        {
            containerBuilder.Register<MySqlConfiguration>((Func<IComponentContext, MySqlConfiguration>) (context =>
            {
                var configuration = mySqlConfiguration(context);
                return configuration;
            })).As<IMySqlConfiguration>().SingleInstance();
            
            containerBuilder.Register<EventStoreMySqlDbContext>((Func<IComponentContext, EventStoreMySqlDbContext>) (context =>
            {
                var configuration = mySqlConfiguration(context);
                var dbContext = new EventStoreMySqlDbContext(DbContextOptionsFactory.Get(configuration.ConnectionString) , configuration);
                return dbContext;
            })).As<EventStoreMySqlDbContext>().IfNotRegistered(typeof(EventStoreMySqlDbContext)).SingleInstance();
            
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