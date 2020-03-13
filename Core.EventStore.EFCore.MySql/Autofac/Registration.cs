using System;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.DbContexts;
using Core.EventStore.MySql.EFCore.Implementations;

namespace Core.EventStore.MySql.EFCore.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder UseeMySql(this ContainerBuilder containerBuilder, Func<IComponentContext, IMySqlConfiguration> mySqlConfiguration)
        {
            
            containerBuilder.Register<IMySqlConfiguration>((Func<IComponentContext, IMySqlConfiguration>) (context =>
            {
                IMySqlConfiguration configuration = mySqlConfiguration(context);
                containerBuilder.RegisterInstance(configuration).As<IMySqlConfiguration>().IfNotRegistered(typeof(IMySqlConfiguration)).SingleInstance();

                
                return configuration;
            })).As<IMySqlConfiguration>().SingleInstance();
            
            // MySqlConfiguration configuration = new MySqlConfiguration();
            // mySqlConfiguration.Invoke(configuration);
            // containerBuilder.RegisterInstance(configuration).As<IMySqlConfiguration>()
            //     .IfNotRegistered(typeof(IMySqlConfiguration)).SingleInstance();
            
            
            
            return containerBuilder;
        }
        
        public static ContainerBuilder KeepPositionInMySql(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<EventStoreMySqlDbContext>()
                .WithParameter("options",
                    parameterValue: DbContextOptionsFactory.Get(
                        "server=localhost;Database=EventStoreDb;uid=root;pwd=TTTttt456;sslmode=none;"))
                .IfNotRegistered(typeof(EventStoreMySqlDbContext)).SingleInstance();
            
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();
            
            return containerBuilder;
        }
        
        public static ContainerBuilder KeepIdempotenceInMySql(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<EventStoreMySqlDbContext>()
                .WithParameter("options",
                    parameterValue: DbContextOptionsFactory.Get(
                        "server=localhost;Database=EventStoreDb;uid=root;pwd=TTTttt456;sslmode=none;"))
                .IfNotRegistered(typeof(EventStoreMySqlDbContext)).SingleInstance();
            
            containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();
            
            return containerBuilder;
        }
    }
}
