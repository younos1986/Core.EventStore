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

        public static ContainerBuilder UseeEfCore(this ContainerBuilder containerBuilder, Func<IComponentContext, EfCoreConfiguration> efCoreConfiguration)
        {
            containerBuilder.Register<IEfCoreConfiguration>((Func<IComponentContext, IEfCoreConfiguration>) (context =>
            {
                EfCoreConfiguration configuration = efCoreConfiguration(context);
                containerBuilder.RegisterInstance(configuration).As<IEfCoreConfiguration>().IfNotRegistered(typeof(IEfCoreConfiguration)).SingleInstance();

                // if (configuration.DbContext == null)
                // {
                //     containerBuilder
                //         .RegisterType<EventStoreEfCoreDbContext>()
                //         .WithParameter("options",
                //             parameterValue: DbContextOptionsFactory.Get(configuration.ConnectionString))
                //         .InstancePerLifetimeScope();
                // }
                // else
                // {
                //     containerBuilder
                //         .RegisterInstance(configuration.DbContext)
                //         .As<EventStoreEfCoreDbContext>()
                //         .InstancePerLifetimeScope();
                // }
                
                return configuration;
            })).As<IEfCoreConfiguration>().SingleInstance();
            
            return containerBuilder;
        }

        public static ContainerBuilder KeepPositionInEfCore(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<EventStoreEfCoreDbContext>()
                .WithParameter("options",
                    parameterValue: DbContextOptionsFactory.Get("Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;"))
                .IfNotRegistered(typeof(EventStoreEfCoreDbContext)).SingleInstance();
            
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();
            
            return containerBuilder;
        }
        
        public static ContainerBuilder KeepIdempotenceInEfCore(this ContainerBuilder containerBuilder)
        {
        
            containerBuilder
                .RegisterType<EventStoreEfCoreDbContext>()
                .WithParameter("options",
                    parameterValue: DbContextOptionsFactory.Get("Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;"))
                .IfNotRegistered(typeof(EventStoreEfCoreDbContext)).SingleInstance();
            
            containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();
            
            return containerBuilder;
        }
    }
}
