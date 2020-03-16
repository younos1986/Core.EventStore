using System;
using System.Reflection;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.SqlServer.DbContexts;
using Core.EventStore.EFCore.SqlServer.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Core.EventStore.EFCore.SqlServer.Autofac
{
    public static class Registration
    {

        public static ContainerBuilder UseeEfCore(this ContainerBuilder containerBuilder,
            Func<IComponentContext, EfCoreConfiguration> efCoreConfiguration)
        {
            containerBuilder.Register<EfCoreConfiguration>((Func<IComponentContext, EfCoreConfiguration>) (context =>
            {
                var configuration = efCoreConfiguration(context);
                containerBuilder.RegisterInstance(configuration).As<IEfCoreConfiguration>()
                    .IfNotRegistered(typeof(IEfCoreConfiguration)).SingleInstance();

                return configuration;
            })).As<IEfCoreConfiguration>().SingleInstance();
            
            
            
            containerBuilder.Register<EventStoreEfCoreDbContext>((Func<IComponentContext, EventStoreEfCoreDbContext>) (context =>
            {
                var configuration = efCoreConfiguration(context);

                var dbContext =
                    new EventStoreEfCoreDbContext(DbContextOptionsFactory.Get(configuration.ConnectionString));
                
                return dbContext;
            })).As<EventStoreEfCoreDbContext>().IfNotRegistered(typeof(EventStoreEfCoreDbContext)).SingleInstance();
            
            return containerBuilder;
        }

        public static ContainerBuilder KeepPositionInEfCore(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder KeepIdempotenceInEfCore(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();

            return containerBuilder;
        }
    }
}